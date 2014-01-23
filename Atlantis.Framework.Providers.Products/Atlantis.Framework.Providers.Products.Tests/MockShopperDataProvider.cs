using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Shopper.Interface;

namespace Atlantis.Framework.Providers.Products.Tests
{
  public class MockShopperDataProvider : ProviderBase, IShopperDataProvider
  {
    public MockShopperDataProvider(IProviderContainer container) : base(container)
    {
    }

    public bool IsShopperValid()
    {
      return true;
    }

    public void RegisterNeededFields(IEnumerable<string> fields)
    {
      throw new NotImplementedException();
    }

    public void RegisterNeededFields(params string[] fields)
    {
      throw new NotImplementedException();
    }

    public bool TryCreateNewShopper()
    {
      throw new NotImplementedException();
    }

    public bool TryGetField<T>(string fieldName, out T fieldValue)
    {
      fieldValue = default(T);

      var shopperId = Container.GetData("MockShopperDataProvider.ShopperId", "0");

      switch (fieldName)
      {
        case "country":
          try
          {
            string shopperCountry = shopperId.Equals("zzShopper123") ? "zz" : "us";
            fieldValue = (T)Convert.ChangeType(shopperCountry, typeof(T));
            return true;
          }
          catch
          {
            return false;
          }
      }

      return false;
    }

    public bool TryUpdateShopper(IDictionary<string, string> updates)
    {
      throw new NotImplementedException();
    }

    public ShopperUpdateResultType UpdateShopperInfo(IDictionary<string, string> updates)
    {
      throw new NotImplementedException();
    }
  }
}