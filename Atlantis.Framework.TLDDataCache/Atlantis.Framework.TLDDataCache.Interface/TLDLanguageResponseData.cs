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
      _tldLanguagesDataByName.TryGetValue(languageName, out result);
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
      _tldLanguagesDataByName = new Dictionary<string, RegistryLanguage>(StringComparer.OrdinalIgnoreCase);

      foreach (XElement itemElement in dataCacheElement.Elements("item"))
      {
        int languageId;

        var idAtt = itemElement.Attribute("languageId");
        var nameAtt = itemElement.Attribute("languageName");
        var tagAtt = itemElement.Attribute("registryTag");

        if (string.IsNullOrEmpty(idAtt.Value) || string.IsNullOrEmpty(nameAtt.Value) || string.IsNullOrEmpty(tagAtt.Value) || !Int32.TryParse(idAtt.Value, out languageId))
        {
          const string message = "Xml with invalid langId, langName or registryTag";
          var aex = new AtlantisException("TLDLanguageResponseData.ctor", "0", message, itemElement.ToString(), null, null);
          Engine.Engine.LogAtlantisException(aex);

          continue;
        }

        languageId = Int32.Parse(idAtt.Value);
        string languageName = nameAtt.Value;
        string registryTag = tagAtt.Value;

        var regLangData = new RegistryLanguage(languageId, languageName, registryTag);
        _tldLanguageData.Add(regLangData);
        _tldLanguagesDataById.Add(languageId, regLangData);
        _tldLanguagesDataByName.Add(languageName, regLangData);
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
}
