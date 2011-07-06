
namespace Atlantis.Framework.Providers.Interface.Links
{
  public interface ILinkProvider
  {
    string CssRoot { get; }
    string DefaultScheme { get; }
    string GetFullSecureUrl(string relativePath);
    string GetFullSecureUrl(string relativePath, QueryParamMode queryParamMode);
    string GetFullSecureUrl(string relativePath, QueryParamMode queryParamMode, params string[] additionalQueryParameters);
    string GetFullSecureUrl(string relativePath, QueryParamMode queryParamMode, System.Collections.Specialized.NameValueCollection queryMap);
    string GetFullSecureUrl(string relativePath, params string[] additionalQueryParameters);
    string GetFullUrl(string relativePath);
    string GetFullUrl(string relativePath, QueryParamMode queryParamMode);
    string GetFullUrl(string relativePath, QueryParamMode queryParamMode, params string[] additionalQueryParameters);
    string GetFullUrl(string relativePath, QueryParamMode queryParamMode, System.Collections.Specialized.NameValueCollection queryMap);
    string GetFullUrl(string relativePath, params string[] additionalQueryParameters);
    string GetRelativeUrl(string relativePath);
    string GetRelativeUrl(string relativePath, QueryParamMode queryParamMode);
    string GetRelativeUrl(string relativePath, QueryParamMode queryParamMode, params string[] additionalQueryParameters);
    string GetRelativeUrl(string relativePath, QueryParamMode queryParamMode, System.Collections.Specialized.NameValueCollection queryMap);
    string GetRelativeUrl(string relativePath, params string[] additionalQueryParameters);
    string GetRelativeUrl(string relativePath, System.Collections.Specialized.NameValueCollection queryMap);
    string GetUrl(string linkName, string relativePath);
    string GetUrl(string linkName, string relativePath, QueryParamMode queryParamMode, params string[] additionalQueryParameters);
    string GetUrl(string linkName, string relativePath, QueryParamMode queryParamMode, bool isSecure);
    string GetUrl(string linkName, string relativePath, QueryParamMode queryParamMode, bool isSecure, params string[] additionalQueryParameters);
    string GetUrl(string linkName, string relativePath, QueryParamMode queryParamMode, bool isSecure, System.Collections.Specialized.NameValueCollection queryMap);
    string GetUrl(string linkName, string relativePath, params string[] additionalQueryParameters);
    string GetUrl(string linkName, string relativePath, System.Collections.Specialized.NameValueCollection queryMap);
    string GetUrlArguments();
    string GetUrlArguments(QueryParamMode queryParamMode);
    string GetUrlArguments(QueryParamMode queryParamMode, params string[] queryParameters);
    string GetUrlArguments(params string[] queryParameters);
    string GetUrlArguments(System.Collections.Specialized.NameValueCollection queryMap);
    string GetUrlArguments(System.Collections.Specialized.NameValueCollection queryMap, QueryParamMode queryParamMode);
    string ImageRoot { get; }
    bool IsDebugInternal();
    string JavascriptRoot { get; }
    string LargeImagesRoot { get; }
    string PresentationCentralImagesRoot { get; }
    string this[string linkName, int contextId] { get; }
    string this[string linkName] { get; }
    string VideoMeRoot { get; }
    string VideoRoot { get; }
  }
}
