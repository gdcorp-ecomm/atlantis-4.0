using Atlantis.Framework.Interface;
using Atlantis.Framework.Language.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using System;
using System.Web;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace Atlantis.Framework.Providers.Language.Handlers
{
  internal class CDSPhraseHandler : ILanguagePhraseHandler
  {
    const string CLIENT_SPOOF_PARAM_NAME = "version";
    const string SERVICE_SPOOF_PARAM_NAME = "docid";
    const string CDSDictionaryUrlFormat = "localization/{0}/{1}";
    const string DebugInfoKeyFormat = "CDS Lang. Dictionary - {0}";

    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<ILocalizationProvider> _localization;
    private readonly Lazy<IDebugContext> _debugContext;
    private const string _defaultLanguage = "en";

    public CDSPhraseHandler(IProviderContainer container)
    {
      _siteContext = new Lazy<ISiteContext>(container.Resolve<ISiteContext>);
      _localization = new Lazy<ILocalizationProvider>(container.Resolve<ILocalizationProvider>);
      if (_siteContext.Value.IsRequestInternal)
      {
        _debugContext = new Lazy<IDebugContext>(container.Resolve<IDebugContext>);
      }
    }

    public string GetLanguagePhrase(string dictionaryName, string phraseKey)
    {
      dictionaryName = dictionaryName.Substring(4);
      string phraseText;
      var fullLanguage = _localization.Value.FullLanguage;
      var fullLanguageDictionary = GetLanguageResponse(dictionaryName, fullLanguage);
      if (!TryGetPhraseText(fullLanguageDictionary, phraseKey, fullLanguage, out phraseText) && fullLanguage != _defaultLanguage)
      {
        var shortLanguage = _localization.Value.ShortLanguage;
        var shortLanguageDictionary = GetLanguageResponse(dictionaryName, shortLanguage);
        if (!TryGetPhraseText(shortLanguageDictionary, phraseKey, shortLanguage, out phraseText) && shortLanguage != _defaultLanguage)
        {
          var defaultLanguageDictionary = GetLanguageResponse(dictionaryName, _defaultLanguage);
          TryGetPhraseText(defaultLanguageDictionary, phraseKey, _defaultLanguage, out phraseText);
        }
      }
      return phraseText;
      
    }

    private CDSLanguageResponseData GetLanguageResponse(string dictionaryName, string language)
    {
      CDSLanguageResponseData response;
      bool byPassDataCache;
      string dictionaryUrl = string.Format(CDSDictionaryUrlFormat, dictionaryName, language);
      string spoofParam = GetSpoofParamIfAny(dictionaryUrl, out byPassDataCache);
      var request = new CDSLanguageRequestData(dictionaryName, language, spoofParam);
      if (byPassDataCache)
      {
        response = (CDSLanguageResponseData)Engine.Engine.ProcessRequest(request, LanguageProviderEngineRequests.CDSLanguagePhrase);
      }
      else
      {
        response = (CDSLanguageResponseData)DataCache.DataCache.GetProcessRequest(request, LanguageProviderEngineRequests.CDSLanguagePhrase);
      }
      LogCDSDebugInfo(dictionaryUrl, response.VersionId);
      return response;
    }

    private string GetSpoofParamIfAny(string dictionaryUrl, out bool byPassDataCache)
    {
      byPassDataCache = false;
      string spoofParam = string.Empty;

      if (_siteContext.Value.IsRequestInternal && HttpContext.Current != null)
      {
        string list = HttpContext.Current.Request.Params[CLIENT_SPOOF_PARAM_NAME];
        string versionId = GetSpoofedVersionId(dictionaryUrl, list);
        if (IsValidContentId(versionId))
        {
          byPassDataCache = true;
          var queryParams = new NameValueCollection();
          queryParams.Add(SERVICE_SPOOF_PARAM_NAME, versionId);
          string appendChar = spoofParam.Contains("?") ? "&" : "?";
          spoofParam += string.Concat(appendChar, ToQueryString(queryParams));
        }
      }

      return spoofParam;
    }

    private string GetSpoofedVersionId(string key, string list)
    {
      string value = null;

      if (!string.IsNullOrEmpty(list) && list.Contains(key))
      {
        string[] pairs = list.Split(new char[] { ',' });

        if (pairs.Length > 0)
        {
          //Gets the value in the first pair where the key matches
          //Example query string: version=localization/sales/integrationtests/hosting/web-hosting/en|df98ad79fa7d9f79ad8a7df
          value = (from pair in pairs
                   let pairArray = pair.Split(new char[] { '|' })
                   where pairArray.Length == 2
                   && pairArray[0] == key
                   select pairArray[1]).FirstOrDefault();
        }
      }
      return value;
    }

    private bool IsValidContentId(string text)
    {
      bool result = false;
      if (text != null)
      {
        string pattern = @"^[0-9a-fA-F]{24}$";
        result = Regex.IsMatch(text, pattern);
      }
      return result;
    }

    private string ToQueryString(NameValueCollection nvc)
    {
      return string.Join("&", nvc.AllKeys.SelectMany(key => nvc.GetValues(key).Select(value => string.Format("{0}={1}", key, value))).ToArray());
    }

    private void LogCDSDebugInfo(string name, string value)
    {
      if (_siteContext.Value.IsRequestInternal)
      {
        try
        {
          if (_debugContext.Value != null)
          {
            var debugData = _debugContext.Value.GetDebugTrackingData();
            if (debugData == null || !(from kvp in debugData where kvp.Value == value select kvp).Any())
            {
              _debugContext.Value.LogDebugTrackingData(string.Format(DebugInfoKeyFormat, name), value);
            }

          }
        }
        catch { }
      }
    }

    private bool TryGetPhraseText(CDSLanguageResponseData response, string phraseKey, string language, out string phraseText)
    {
      var phrase = response.Phrases.FindPhrase(phraseKey, _siteContext.Value.ContextId, _localization.Value.CountrySite, language);
      phraseText = string.Empty;
      var exists = false;
      if (phrase != null)
      {
        exists = true;
        if (phrase.PhraseText != null)
        {
          phraseText = phrase.PhraseText;
        }
      }
      return exists;
    }
  }
}
