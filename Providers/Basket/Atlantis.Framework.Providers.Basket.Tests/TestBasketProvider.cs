using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Basket.Interface;
using System;

namespace Atlantis.Framework.Providers.Basket.Tests
{
  public class TestBasketProvider : BasketProvider
  {
    private Lazy<ISiteContext> _siteContext;

    public TestBasketProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => { return Container.Resolve<ISiteContext>(); });
    }

  }
}
