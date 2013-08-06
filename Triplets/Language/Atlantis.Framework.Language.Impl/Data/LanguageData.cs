using System;
using System.Configuration;
using System.Web;
using System.Collections.Generic;
using System.IO;
using Atlantis.Framework.Parsers.LanguageFile;
using System.Linq;

namespace Atlantis.Framework.Language.Impl.Data
{
  internal class LanguageData
  {
    private readonly Dictionary<string, PhraseDictionary> _phraseData = new Dictionary<string, PhraseDictionary>();
    private const string _searchPattern = "*.language";
    private const string _LanguageDataLocation = "Atlantis.Framework.Language.LanguageDataLocation";
    private const string _appDataLocation = "~/App_Data/Language";

    internal LanguageData()
    {
      LoadLanguageData();
    }

    private void LoadLanguageData()
    {
      var languageFiles = GetLanguageFileNames();
      foreach (var languageFile in languageFiles)
      {
        if (File.Exists(languageFile))
        {
          var fileInfo = new PhraseFileInfo(languageFile);
          if (fileInfo.IsLanguageDataValid)
          {
            IEnumerable<string> dataLines = File.ReadAllLines(languageFile);
            PhraseDictionary dictionary;
            if (!_phraseData.TryGetValue(fileInfo.DictionaryName, out dictionary))
            {
              dictionary = new PhraseDictionary();
              PhraseDictionary.Parse(dictionary, dataLines, fileInfo.DictionaryName, fileInfo.Language);
              _phraseData.Add(fileInfo.DictionaryName, dictionary);
            }
            else
            {
              PhraseDictionary.Parse(dictionary, dataLines, fileInfo.DictionaryName, fileInfo.Language);
            }

          }
        }
      }
    }

    private IEnumerable<string> GetLanguageFileNames()
    {
      var pathUri = new Uri(Path.GetDirectoryName(GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      var assemblyFiles = GetFilesFromDirectory(assemblyPath);
      var appFiles = GetFilesFromConfiguration();
      return assemblyFiles.Union(appFiles);
    }

    private static IEnumerable<string> GetFilesFromConfiguration()
    {
      var configPath = ConfigurationManager.AppSettings[_LanguageDataLocation];
      var path = (!string.IsNullOrEmpty(configPath)) ? configPath : _appDataLocation;
      var useHttp = path.StartsWith("~");

      var appFiles = (useHttp && HttpContext.Current != null)
                        ? GetFilesFromDirectory(HttpContext.Current.Server.MapPath(path)).ToList()
                        : GetFilesFromDirectory(path).ToList();
      return appFiles;
    }

    private static IEnumerable<string> GetFilesFromDirectory(string path)
    {
      return (Directory.Exists(path)) ? Directory.GetFiles(path, _searchPattern, SearchOption.AllDirectories) : new string[0];
    }

    internal string FindPhrase(string dictionary, string phraseKey, int contextId, string countrySite, string language)
    {
      var result = string.Empty;
      var predicate = new PhrasePredicate(contextId, countrySite, language);
      PhraseDictionary phraseDictionary;
      if (_phraseData.TryGetValue(dictionary, out phraseDictionary))
      {
        Phrase phrase = phraseDictionary.FindPhrase(phraseKey, predicate);
        if (phrase != null)
        {
          result = phrase.PhraseText;
        }
      }

      return result;
    }

  }
}
