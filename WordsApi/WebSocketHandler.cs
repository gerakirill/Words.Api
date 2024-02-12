using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using WordsApi.Application.Behavior.Abstractions;
using WordsApi.Application.WordsChecker;
using WordsApi.Creation;
using WordsApi.Infrastructure.Auth;
using WordsApi.Messaging;
using WordsApi.EnumExtensions;

namespace WordsApi;

public class WebSocketHandler(
    RequestDelegate next,
    IWordsChecker wordsChecker,
    ILogger<WebSocketHandler> logger)
{
    public async Task Invoke(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            await HandleWebSocketConnection(context, webSocket);
        }
        else
        {
            await next(context);
        }
    }

    private async Task HandleWebSocketConnection(HttpContext context, WebSocket webSocket)
    {
        try
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result;

            do
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result is { MessageType: WebSocketMessageType.Text, CloseStatus: null })
                {
                    string request = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    var message = JsonSerializer.Deserialize<Message>(request);

                    if (message is null)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Bad request");
                        return;
                    }

                    var attribute = message.Type.GetAttribute<AuthorizeAttribute>();
                    if (!AuthorizationHandler.Handle(attribute, context.User))
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Forbidden");
                        return;
                    }

                    var handler = new MessageHandlerFactory(wordsChecker).CreateHandler(message);
                    if (handler.IsSuccess)
                    {
                        var response = handler.Data.Handle();
                        byte[] responseBuffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));
                        await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer, 0, responseBuffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    else
                    {
                        switch (handler.OperationResult)
                        {
                            case OperationResult.Success:
                                break;
                            case OperationResult.BadCommand:
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                await context.Response.WriteAsync(JsonSerializer.Serialize(handler.Errors));
                                break;
                            case OperationResult.NotFound:
                                break;
                        }
                    }

                    
                }
            }
            while (!result.CloseStatus.HasValue);

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
        catch (Exception ex)
        {
            logger.LogError("Unhandled exception occured: {ExceptionMessage}. Exception: {Exception}", ex.Message, ex);
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await context.Response.WriteAsync("Service is currently unavailable");
        }
    }
}