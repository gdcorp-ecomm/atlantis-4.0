
using System;
using System.Collections.Specialized;

namespace Atlantis.Framework.Providers.Interface.Links
{
  public interface ILinkProvider
  {
    string CssRoot { get; }
    string DefaultScheme { get; }

    [Obsolete("Use GetUrl w/LinkProviderOptions.ProtocolHttps")]
    string GetFullSecureUrl(string relativePath);
    [Obsolete("Use GetUrl w/LinkProviderOptions.ProtocolHttps|LinkProviderOptions.QueryStringXYZs")]
    string GetFullSecureUrl(string relativePath, QueryParamMode queryParamMode);
    [Obsolete("Use GetUrl w/LinkProviderOptions.ProtocolHttps|LinkProviderOptions.QueryStringXYZs")]
    string GetFullSecureUrl(string relativePath, QueryParamMode queryParamMode, params string[] additionalQueryParameters);
    [Obsolete("Use GetUrl w/LinkProviderOptions.ProtocolHttps|LinkProviderOptions.QueryStringXYZs")]
    string GetFullSecureUrl(string relativePath, QueryParamMode queryParamMode, NameValueCollection queryMap);
    [Obsolete("Use GetUrl w/LinkProviderOptions.ProtocolHttps")]
    string GetFullSecureUrl(string relativePath, params string[] additionalQueryParameters);
    [Obsolete("Use GetUrl w/LinkProviderOptions.DefaultOptions")]
    string GetFullUrl(string relativePath);
    [Obsolete("Use GetUrl w/LinkProviderOptions.QueryStringXYZs")]
    string GetFullUrl(string relativePath, QueryParamMode queryParamMode);
    [Obsolete("Use GetUrl w/LinkProviderOptions.QueryStringXYZs")]
    string GetFullUrl(string relativePath, QueryParamMode queryParamMode, params string[] additionalQueryParameters);
    [Obsolete("Use GetUrl w/LinkProviderOptions.QueryStringXYZs")]
    string GetFullUrl(string relativePath, QueryParamMode queryParamMode, NameValueCollection queryMap);
    [Obsolete("Use GetUrl w/LinkProviderOptions.DefaultOptions")]
    string GetFullUrl(string relativePath, params string[] additionalQueryParameters);

    //[Obsolete("This method will rebind to GetUrl w/optional parameters")]
    string GetRelativeUrl(string relativePath);
    [Obsolete("Use GetRelativeUrl w/LinkProviderOptions.QueryStringXYZs")]
    string GetRelativeUrl(string relativePath, QueryParamMode queryParamMode);
    [Obsolete("Use GetRelativeUrl w/LinkProviderOptions.QueryStringXYZs")]
    string GetRelativeUrl(string relativePath, QueryParamMode queryParamMode, params string[] additionalQueryParameters);
    [Obsolete("Use GetRelativeUrl w/LinkProviderOptions.QueryStringXYZs")]
    string GetRelativeUrl(string relativePath, QueryParamMode queryParamMode, NameValueCollection queryMap);

    string GetRelativeUrl(string relativePath, params string[] additionalQueryParameters);

    //[Obsolete("This method will rebind to GetUrl w/optional parameters")]
    string GetRelativeUrl(string relativePath, NameValueCollection queryMap);

    //[Obsolete("This method will rebind to GetUrl w/optional parameters")]
    string GetUrl(string linkName, string relativePath);
    [Obsolete("Use GetUrl w/LinkProviderOptions.QueryStringXYZs")]
    string GetUrl(string linkName, string relativePath, QueryParamMode queryParamMode, params string[] additionalQueryParameters);
    [Obsolete("Use GetUrl w/LinkProviderOptions.QueryStringXYZs|LinkProviderOptions.ProtocolHttp/Https")]
    string GetUrl(string linkName, string relativePath, QueryParamMode queryParamMode, bool isSecure);
    [Obsolete("Use GetUrl w/LinkProviderOptions.QueryStringXYZs|LinkProviderOptions.ProtocolHttp/Https")]
    string GetUrl(string linkName, string relativePath, QueryParamMode queryParamMode, bool isSecure, params string[] additionalQueryParameters);
    [Obsolete("Use GetUrl w/LinkProviderOptions.QueryStringXYZs|LinkProviderOptions.ProtocolHttp/Https")]
    string GetUrl(string linkName, string relativePath, QueryParamMode queryParamMode, bool isSecure, NameValueCollection queryMap);

    string GetUrl(string linkName, string relativePath, params string[] additionalQueryParameters);

    //[Obsolete("This method will rebind to GetUrl w/optional parameters")]
    string GetUrl(string linkName, string relativePath, NameValueCollection queryMap);

    //[Obsolete("This method will rebind to GetUrlArguments w/optional parameters")]
    string GetUrlArguments();
    [Obsolete("Use GetUrlArguments w/LinkProviderOptions.QueryStringXYZs")]
    string GetUrlArguments(QueryParamMode queryParamMode);
    [Obsolete("Use GetUrlArguments w/LinkProviderOptions.QueryStringXYZs")]
    string GetUrlArguments(QueryParamMode queryParamMode, params string[] queryParameters);
    
    string GetUrlArguments(params string[] queryParameters);

    //[Obsolete("This method will rebind to GetUrlArguments w/optional parameters")]
    string GetUrlArguments(NameValueCollection queryMap);
    [Obsolete("Use GetUrlArguments w/LinkProviderOptions.QueryStringXYZs")]
    string GetUrlArguments(NameValueCollection queryMap, QueryParamMode queryParamMode);

    string ImageRoot { get; }
    bool IsDebugInternal();
    string JavascriptRoot { get; }
    string LargeImagesRoot { get; }
    string PresentationCentralImagesRoot { get; }
    string this[string linkName, int contextId] { get; }
    string this[string linkName] { get; }
    string VideoMeRoot { get; }
    string VideoRoot { get; }

//    string GetRelativeUrl(string relativePath, NameValueCollection queryMap = null, LinkProviderOptions options = LinkProviderOptions.DefaultOptions);
//    string GetRelativeUrl(string relativePath, LinkProviderOptions options, params string[] additionalQueryParameters);

    string GetUrl(string linkName, string relativePath, NameValueCollection queryMap = null, LinkProviderOptions options = LinkProviderOptions.DefaultOptions, string countrySiteId = null, string marketId = null);
    string GetUrl(string linkName, string relativePath, LinkProviderOptions options, string countrySiteId, string marketId, params string[] additionalQueryParameters);

    string GetUrlArguments(NameValueCollection queryMap = null, LinkProviderOptions options = LinkProviderOptions.DefaultOptions);
    string GetUrlArguments(LinkProviderOptions options, params string[] additionalQueryParameters);
  }
}
