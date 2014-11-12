using Atlantis.Framework.Interface;
using Atlantis.Framework.Language.Interface;
using Atlantis.Framework.Providers.Language.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using System;
using System.Web;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Language.Handlers
{
  internal class CDSPhraseHandler : ILanguagePhraseHandler
  {
    const string DebugInfoLogTrackingKey = "Providers.Language.CDSPhraseHandler.DebugInfoLogTracking";
    const string CLIENT_SPOOF_PARAM_NAME = "version";
    const string SERVICE_SPOOF_PARAM_NAME = "docid";
    const string CDSDictionaryUrlFormat = "localization/{0}/{1}";
    const string DebugInfoKeyFormat = "{0}. CDS Language Dictionary";
    internal const string SITE_ADMIN_URL_KEY = "SITEADMINURL";
    internal const string CDSM_CONTENT_RELATIVE_PATH = "contentmanagement/content/index/{0}/{1}";
    const string HyperLinkFormat = "<a href='{0}' target='_blank'>{1}/{2}</a>";
 
    private readonly IProviderContainer _container;
    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<ILocalizationProvider> _localization;
    private readonly Lazy<IDebugContext> _debugContext;
    private const string _defaultLanguage = "en";

    private readonly string _fullLanguage;
    private readonly string _shortLanguage;

    public CDSPhraseHandler(IProviderContainer container, string fullLanguage, string shortLanguage)
    {
      _container = container;
      _siteContext = new Lazy<ISiteContext>(container.Resolve<ISiteContext>);
      _localization = new Lazy<ILocalizationProvider>(container.Resolve<ILocalizationProvider>);

      _fullLanguage = fullLanguage;
      _shortLanguage = shortLanguage;

      if (_siteContext.Value.IsRequestInternal)
      {
        _debugContext = new Lazy<IDebugContext>(container.Resolve<IDebugContext>);
      }
    }

    public bool TryGetLanguagePhrase(string dictionaryName, string phraseKey, out string phrase)
    {
      return TryGetLanguagePhrase(dictionaryName, phraseKey, true, out phrase);
    }

    public bool TryGetLanguagePhrase(string dictionaryName, string phraseKey, bool doGlobalFallback, out string phrase)
    {
      bool retVal;

      dictionaryName = dictionaryName.Substring(4);
      var fullLanguage = _fullLanguage;
      var fullLanguageDictionary = GetLanguageResponse(dictionaryName, fullLanguage);

      if (!(retVal = TryGetPhraseText(fullLanguageDictionary, phraseKey, fullLanguage, out phrase)) && fullLanguage != _defaultLanguage)
      {
        var shortLanguage = _shortLanguage;
        var shortLanguageDictionary = GetLanguageResponse(dictionaryName, shortLanguage);
        if (!(retVal = TryGetPhraseText(shortLanguageDictionary, phraseKey, shortLanguage, out phrase)) && doGlobalFallback && shortLanguage != _defaultLanguage)
        {
          var defaultLanguageDictionary = GetLanguageResponse(dictionaryName, _defaultLanguage);
          retVal = TryGetPhraseText(defaultLanguageDictionary, phraseKey, _defaultLanguage, out phrase);
        }
      }

      return retVal;
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
      if (response != null && !string.IsNullOrEmpty(response.VersionId) && !string.IsNullOrEmpty(response.DocumentId))
      {
        LogCDSDebugInfo(response.VersionId, response.DocumentId);
      }
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

    private void LogCDSDebugInfo(string versionId, string documentId)
    {
      if (_siteContext.Value.IsRequestInternal)
      {
        try
        {
          if (_debugContext.Value != null)
          {
            HashSet<string> loggedVersionIds = _container.GetData<HashSet<string>>(DebugInfoLogTrackingKey, null);
            if (loggedVersionIds == null)
            {
              loggedVersionIds = new HashSet<string>();
            }
            if (!loggedVersionIds.Contains(versionId))
            {
              loggedVersionIds.Add(versionId);
              string debugInfoKey = string.Format(DebugInfoKeyFormat, loggedVersionIds.Count);
              string hyperlink = GetDebugInfoHyperLink(versionId, documentId);
              _debugContext.Value.LogDebugTrackingData(debugInfoKey, hyperlink);
              _container.SetData<HashSet<string>>(DebugInfoLogTrackingKey, loggedVersionIds);
            }
          }
        }
        catch
        { 
          //intentionally left blank
        }
      }
    }

    private string GetDebugInfoHyperLink(string versionId, string documentId)
    {
      string hyperlink = null;

      var contentRelativeUrl = string.Format(CDSM_CONTENT_RELATIVE_PATH, documentId, versionId);
      hyperlink = string.Format(HyperLinkFormat, contentRelativeUrl, documentId, versionId);

      return hyperlink;
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
