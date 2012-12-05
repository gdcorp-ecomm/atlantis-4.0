using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Pricing;

namespace Atlantis.Framework.Providers.Currency.Tests.Mocks
{
  public class MockPriceProvider : ProviderBase, IPricingProvider
  {
    public const int USD_PFID58_PRICE = 327;
    public const int GBP_PFID58_PRICE = 313;
    public const int INR_PFID58_PRICE = 9957;
    public const int USD_PFID101_PRICE = 654;
    public const int GBP_PFID101_PRICE = 626;
    public const int INR_PFID101_PRICE = 19914;

    public MockPriceProvider(IProviderContainer container) : base(container)
    {
    }

    public bool DoesIscAffectPricing(string iscCode)
    {
      return iscCode == "valid";
    }

    public bool GetCurrentPrice(int unifiedProductId, int shopperPriceType, string currencyType, out int price, string isc = "", int catalogId = 0)
    {
      bool success = false;
      price = 0;
      if (shopperPriceType == 0)
      {
        switch (unifiedProductId)
        {
          case 58:
            switch (currencyType.ToLowerInvariant())
            {
              case "usd":
                price = USD_PFID58_PRICE;
                success = true;
                break;
              case "gbp":
                price = GBP_PFID58_PRICE;
                success = true;
                break;
              case "inr":
                price = INR_PFID58_PRICE;
                success = true;
                break;
            }
            break;

          case 101:
            {
              switch (currencyType.ToLowerInvariant())
              {
                case "usd":
                  price = USD_PFID101_PRICE;
                  success = true;
                  break;
                case "gbp":
                  price = GBP_PFID101_PRICE;
                  success = true;
                  break;
                case "inr":
                  price = INR_PFID101_PRICE;
                  success = true;
                  break;
              }
            }
            break;
        }
      }
      return success;
    }
  }
}
