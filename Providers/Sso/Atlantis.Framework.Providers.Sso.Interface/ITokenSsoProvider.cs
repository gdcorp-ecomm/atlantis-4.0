using Atlantis.Framework.Sso.Interface.JsonHelperClasses;

namespace Atlantis.Framework.Providers.Sso.Interface
{
  public interface ITokenSsoProvider
  {
    int TokenTimeoutMins { get; }
    string CurrentLoggedInShopperId { get; }
    Token Token { get; }
    bool TokenHasData { get; }
    bool CurrentShopperIsTokenShopper { get; }
    bool IsTokenValid();
    void SetLoggedInShopper();
  }
}
