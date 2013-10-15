using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Providers.Basket.Tests
{
  public class TestBasketProviderOverrides : BasketProvider
  {
    public const string TESTCICODE = "9911";
    public const string TESTDISCOUNTCODE = "99";

    public TestBasketProviderOverrides(IProviderContainer container)
      : base(container)
    {
    }

    protected override void SetInitialAddItemProperties(Interface.IBasketAddItem basketAddItem)
    {
      basketAddItem.CiCode = TESTCICODE;
    }

    protected override void SetInitialAddRequestProperties(Interface.IBasketAddRequest basketAddRequest)
    {
      basketAddRequest.OrderDiscountCode = TESTDISCOUNTCODE;
    }
  }
}
