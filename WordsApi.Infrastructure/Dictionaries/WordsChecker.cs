using Microsoft.Extensions.Options;
using System.Reflection;
using WeCantSpell.Hunspell;
using WordsApi.Application.WordsChecker;
using WordsApi.Infrastructure.Options;

namespace WordsApi.Infrastructure.Dictionaries
{
    public class WordsChecker : IWordsChecker
    {
        public WordsChecker(IOptionsMonitor<DictionaryResourcesOptions> dictionaryResources)
        {
            string root = Assembly.GetExecutingAssembly().Location;
            string path = Path.GetFullPath(Path.Combine(root, dictionaryResources.CurrentValue.Path));
            _dictionary = WordList.CreateFromFiles(path);
        }

        private readonly WordList _dictionary;

        public bool CheckWord(string word)
        {
            return _dictionary.Check(word, CancellationToken.None);
        }
    }
}
