using System.Collections.Specialized;

namespace Atlantis.Framework.Providers.Sso.Interface
{
  public interface ISsoProvider
  {
    string ServiceProviderGroupName { get; }
    string ServerOrClusterName { get; }
    string SpKey { get; }
    bool ParseArtifact(string artifact, out string shopperId);
    bool ParseArtifact(string artifact, out string shopperId, out int failureCount);
    string GetUrl(SsoUrlType ssoUrlType);
    string GetUrl(SsoUrlType ssoUrlType, NameValueCollection additionalParams);
  }

}
