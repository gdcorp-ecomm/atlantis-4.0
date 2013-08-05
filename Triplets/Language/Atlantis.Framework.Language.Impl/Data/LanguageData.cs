using System;
using System.Configuration;
using System.Web;
using System.Collections.Generic;
using System.IO;
using Atlantis.Framework.Parsers.LanguageFile;
using System.Linq;

namespace Atlantis.Framework.Language.Impl.Data
{

  //refactor this to use the parser
  internal class LanguageData
  {
    private readonly Dictionary<string, PhraseDictionary> _phraseData = new Dictionary<string, PhraseDictionary>();
    private const string _searchPattern = "*.language";
    private const string _appSettingKey = "LanguageFileLocation";
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
                var pd = PhraseDictionary.Parse(dataLines, fileInfo.DictionaryName, fileInfo.Language);
                _phraseData.Add(fileInfo.DictionaryName, pd);
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
      var configPath = ConfigurationManager.AppSettings[_appSettingKey];
      var appFiles = new List<string>();
        
      if (HttpContext.Current != null)
      {
        appFiles = !string.IsNullOrEmpty(configPath) ? GetFilesFromDirectory(HttpContext.Current.Server.MapPath(configPath)).ToList() : GetFilesFromDirectory(HttpContext.Current.Server.MapPath(_appDataLocation)).ToList();
      }

      var assemblyFiles = GetFilesFromDirectory(assemblyPath);
      return assemblyFiles.Union(appFiles);
    }

    private static IEnumerable<string> GetFilesFromDirectory(string path)
    {
      return (!string.IsNullOrEmpty(path) && Directory.Exists(path)) ? Directory.GetFiles(path, _searchPattern, SearchOption.AllDirectories) : new string[0];
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
