using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Sso;
using System;
using System.ComponentModel;

namespace Atlantis.Framework.Providers.Tests
{
  public class TokenSsoProvider : TokenSsoProviderBase
  {
    private Lazy<IShopperContext> _shopperContext;

    public TokenSsoProvider(IProviderContainer container)
      : base(container)
    {
      _shopperContext = new Lazy<IShopperContext>(() => Container.Resolve<IShopperContext>());
    }

    private string _currentLoggedInShopperId;
    public override string CurrentLoggedInShopperId
    {
      get
      {

        return "867900";
      }

    }
  }
}
