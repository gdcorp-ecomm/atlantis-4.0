using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class TLDLanguageResponseData : IResponseData
  {
    private AtlantisException _exception;
    private string _xmlData;

    private List<RegistryLanguage> _tldLanguageData;
    private Dictionary<string, RegistryLanguage> _tldLanguagesDataByName;
    private Dictionary<int, RegistryLanguage> _tldLanguagesDataById;

    public IEnumerable<RegistryLanguage> RegistryLanguages
    {
      get { return _tldLanguageData; }
    }

    public RegistryLanguage GetLanguageDataByName(string languageName)
    {
      RegistryLanguage result;
      _tldLanguagesDataByName.TryGetValue(languageName.ToLowerInvariant(), out result);
      return result;
    }

    public RegistryLanguage GetLanguageDataById(int languageId)
    {
      RegistryLanguage result;
      _tldLanguagesDataById.TryGetValue(languageId, out result);
      return result;
    }

    public static TLDLanguageResponseData FromException(RequestData requestData, Exception ex)
    {
      return new TLDLanguageResponseData(requestData, ex);
    }

    private TLDLanguageResponseData(RequestData requestData, Exception ex)
    {
      string message = ex.Message + ex.StackTrace;
      string inputData = requestData.ToXML();
      _exception = new AtlantisException(requestData, "TLDLanguageResponseData.ctor", message, inputData);
    }

    public static TLDLanguageResponseData FromDataCacheElement(XElement dataCacheElement)
    {
      return new TLDLanguageResponseData(dataCacheElement);
    }

    private TLDLanguageResponseData(XElement dataCacheElement)
    {
      _xmlData = dataCacheElement.ToString();
      _tldLanguageData = new List<RegistryLanguage>();
      _tldLanguagesDataById = new Dictionary<int, RegistryLanguage>();
      _tldLanguagesDataByName = new Dictionary<string, RegistryLanguage>();

      foreach (XElement itemElement in dataCacheElement.Elements("item"))
      {
        try
        {
          var languageId = 0;
          var languageName = string.Empty;
          var registryTag = string.Empty;

          var found = false;
          foreach (XAttribute itemAtt in itemElement.Attributes())
          {
            string name = itemAtt.Name.ToString();
            if (name == "languageId" || name == "languageName" || name == "registryTag")
            {
              found = true;

              if ((name == "languageId"))
              {
                if (!string.IsNullOrEmpty(itemAtt.Value))
                {
                  int langId;
                  if (Int32.TryParse(itemAtt.Value, out langId))
                  {
                    languageId = langId;
                  }
                }
              }
              if (name == "languageName")
              {
                languageName = itemAtt.Value;
              }
              if (name == "registryTag")
              {
                registryTag = itemAtt.Value;
              }
            }
          }
          if (found)
          {
            var regLangData = new RegistryLanguage(languageId, languageName, registryTag);
            _tldLanguageData.Add(regLangData);
            _tldLanguagesDataById.Add(languageId, regLangData);
            _tldLanguagesDataByName.Add(languageName.ToLowerInvariant(), regLangData);
          }
        }
        catch (Exception ex)
        {
          string message = ex.Message + ex.StackTrace;
          var aex = new AtlantisException("TLDLanguageResponseData.ctor", "0", message, itemElement.ToString(), null, null);
          Engine.Engine.LogAtlantisException(aex);
        }
      }
    }

    public string ToXML()
    {
      string result = "<exception/>";
      if (_xmlData != null)
      {
        result = _xmlData;
      }
      return result;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }

  public class RegistryLanguage
  {
    public RegistryLanguage(int languageId, string languageName, string registryTag)
    {
      LanguageId = languageId;
      LanguageName = languageName;
      RegistryTag = registryTag;
    }

    public int LanguageId { get; set; }
    public string LanguageName { get; set; }
    public string RegistryTag { get; set; }
  }
}
