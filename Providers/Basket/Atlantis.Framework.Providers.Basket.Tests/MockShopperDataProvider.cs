using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Atlantis.Framework.Interface;
using System;
using Atlantis.Framework.Providers.Shopper.Interface;

namespace Atlantis.Framework.Providers.Basket.Tests
{
  [ExcludeFromCodeCoverage]
  public class MockShopperDataProvider : ProviderBase, IShopperDataProvider
  {
    public const string MockCreatedShopperId = "832652";
    public const string MockBadShopperId = "444444";

    private readonly Lazy<IShopperContext> _shopperContext;
    private readonly Lazy<bool> _causeErrors; 

    public MockShopperDataProvider(IProviderContainer container)
      : base(container)
    {
      _shopperContext = new Lazy<IShopperContext>(() => Container.Resolve<IShopperContext>());
      _causeErrors = new Lazy<bool>(LoadCauseErrors);
    }

    private bool LoadCauseErrors()
    {
      return Container.GetData("MockShopperDataProvider.CauseErrors", false);
    }

    public bool IsShopperValid()
    {
      if (_causeErrors.Value)
      {
        throw new Exception("IsShopperValid Mock Error.");
      }

      return !string.IsNullOrEmpty(_shopperContext.Value.ShopperId) && (_shopperContext.Value.ShopperId != MockBadShopperId);      
    }

    public void RegisterNeededFields(IEnumerable<string> fields)
    {
    }

    public void RegisterNeededFields(params string[] fields)
    {
    }

    public bool TryCreateNewShopper()
    {
      if (_causeErrors.Value)
      {
        throw new Exception("TryCreateNewShopper Mock Error.");
      }

      _shopperContext.Value.SetNewShopper(MockCreatedShopperId);
      return true;
    }

    public bool TryGetField<T>(string fieldName, out T fieldValue)
    {
      fieldValue = default(T);
      return false;
    }

    public bool TryUpdateShopper(IDictionary<string, string> updates)
    {
      return false;
    }

    public ShopperUpdateResultType UpdateShopperInfo(IDictionary<string, string> updates)
    {
      throw new NotImplementedException();
    }
  }
}
