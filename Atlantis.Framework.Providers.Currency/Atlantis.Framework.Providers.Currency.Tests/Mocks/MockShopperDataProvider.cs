using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Shopper.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Currency.Tests.Mocks
{
  public class MockShopperDataProvider : ProviderBase, IShopperDataProvider 
  {
    public static class MockProperties
    {
      public const string ShopperData = "MockShopperDataProvider.ShopperData";
    }

    Dictionary<string, string> _mockShopperData;

    public MockShopperDataProvider(IProviderContainer container)
      : base(container)
    {
      _mockShopperData = container.GetData(MockProperties.ShopperData, new Dictionary<string, string>());
    }

    public void RegisterNeededFields(params string[] fields)
    {
      return;
    }

    public void RegisterNeededFields(System.Collections.Generic.IEnumerable<string> fields)
    {
      return;
    }

    public bool TryGetField<T>(string fieldName, out T fieldValue)
    {
      fieldValue = default(T);
      bool result = false;

      string dataValue;
      if (_mockShopperData.TryGetValue(fieldName, out dataValue))
      {
        result = TryConvert(dataValue, out fieldValue);
      }

      return result;
    }

    private bool TryConvert<T>(string rawValue, out T convertedValue)
    {
      convertedValue = default(T);

      try
      {
        convertedValue = (T)Convert.ChangeType(rawValue, typeof(T));
        return true;
      }
      catch
      {
        return false;
      }
    }

    public bool IsShopperValid()
    {
      return true;
    }

    public bool TryCreateNewShopper()
    {
      return true;
    }

    public bool TryUpdateShopper(IDictionary<string, string> updates)
    {
      return false;
    }
  }
}
