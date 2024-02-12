using WordsApi.Infrastructure.Extensions;
using WordsApi.Infrastructure.Auth;
using WordsApi;

var builder = WebApplication.CreateBuilder(args);

builder.AddDictionaries();
builder.AddAuthConfiguration();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.UseLogging();
builder.Services.AddOptions();

var app = builder.Build();

app.UseWebSockets();

app.UseMiddleware<AuthenticationHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<WebSocketHandler>();

app.Run();
