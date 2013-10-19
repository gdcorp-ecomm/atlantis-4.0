using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Basket.Interface;
using System;

namespace Atlantis.Framework.Providers.Basket.Tests
{
  public class TestBasketProvider : BasketProvider
  {
    public TestBasketProvider(IProviderContainer container)
      : base(container)
    {
    }
  }
}
