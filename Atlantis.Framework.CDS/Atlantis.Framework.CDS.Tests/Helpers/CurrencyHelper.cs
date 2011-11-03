using System;
using System.Text.RegularExpressions;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Providers.Currency;
using Atlantis.Framework.DataCache;
using Atlantis.Framework.Interface;

namespace Helpers
{
  public static class CurrencyHelper
  {
    private static Regex _dollarFinder = null;
    private static Regex DollarFinder
    {
      get
      {
        if (_dollarFinder == null)
        {
          _dollarFinder = new Regex(@"\$(\d{1,3}(\,\d{3})*|(\d+))(\.\d{2})?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
        return _dollarFinder;
      }
    }

    public static string FixCurrencyInText(string text, bool maskPrices)
    {
      string result = text;

      try
      {
        ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
        ICurrencyInfo usdInfo = CurrencyData.GetCurrencyInfo("USD");
        if (!currency.SelectedDisplayCurrencyInfo.Equals(usdInfo))
        {
          MatchCollection matches = DollarFinder.Matches(text);
          foreach (Match match in matches)
          {
            string matchNumbers = match.Value.Substring(1);
            double amount;
            if (Double.TryParse(matchNumbers, out amount))
            {
              int price = (int)Math.Ceiling(amount * 100);
              ICurrencyPrice currencyPrice = new CurrencyPrice(price, usdInfo, CurrencyPriceType.Transactional);
              string priceText = currency.PriceText(currencyPrice, maskPrices);
              result = result.Replace(match.Value, priceText);
            }
          }
        }

      }
      catch
      { }

      return result;
    }

    public static ICurrencyPrice GetTransactionalPriceFromUSD(int usdAmount)
    {
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
      ICurrencyInfo usdInfo = CurrencyData.GetCurrencyInfo("USD");
      ICurrencyPrice usdPrice = new CurrencyPrice(usdAmount, usdInfo, CurrencyPriceType.Transactional);
      ICurrencyPrice result = CurrencyProvider.ConvertPrice(usdPrice, currency.SelectedTransactionalCurrencyInfo);
      return result;
    }

    public static ICurrencyPrice GetSelectedTransactionalPrice(int Amount)
    {
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
      ICurrencyInfo currencyInfo = currency.SelectedTransactionalCurrencyInfo;
      return new CurrencyPrice(Amount, currencyInfo, CurrencyPriceType.Transactional);
    }

    public static int GetUSDCurrentPriceForProduct(int unifiedProductId)
    {
      int USDCurrentPrice;
      bool promoEstimate;
      ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();

      DataCache.GetPromoPriceEx(siteContext.PrivateLabelId, unifiedProductId, shopperContext.ShopperPriceType, "USD", out USDCurrentPrice, out promoEstimate);
      return USDCurrentPrice;
    }

    public static int GetUSDListPriceForProduct(int unifiedProductId)
    {
      int USDListPrice;
      bool promoEstimate;
      ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();

      DataCache.GetListPriceEx(siteContext.PrivateLabelId, unifiedProductId, shopperContext.ShopperPriceType, "USD", out USDListPrice, out promoEstimate);
      return USDListPrice;
    }

    public static ICurrencyPrice ConvertTransactionalToUSD(ICurrencyPrice transactionalPrice)
    {
      ICurrencyPrice result = transactionalPrice;
      if (transactionalPrice != null && transactionalPrice.Type == CurrencyPriceType.Transactional)
      {
        ICurrencyInfo transactionalInfo = transactionalPrice.CurrencyInfo;
        ICurrencyInfo usdInfo = CurrencyData.GetCurrencyInfo("USD");
        if (usdInfo != null)
        {
          if ((!transactionalPrice.CurrencyInfo.Equals(usdInfo)) && (transactionalInfo.ExchangeRatePricing > 0))
          {
            double convertedDouble = Math.Ceiling(transactionalPrice.Price * transactionalInfo.ExchangeRatePricing);
            int convertedPrice = Int32.MaxValue;
            if (convertedDouble < Int32.MaxValue)
            {
              convertedPrice = Convert.ToInt32(convertedDouble);
            }
            result = new CurrencyPrice(convertedPrice, usdInfo, CurrencyPriceType.Converted);
          }
        }
      }
      return result;
    }

    public static string DropDecimalForWholeUSDDollars(string PriceText)
    {
      string dropDecimalPrice = PriceText;
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
      ICurrencyInfo usdInfo = CurrencyData.GetCurrencyInfo("USD");
      if (currency.SelectedDisplayCurrencyInfo.Equals(usdInfo))
      {
        if (PriceText.EndsWith(".00"))
        {
          dropDecimalPrice = PriceText.Replace(".00", String.Empty);
        }
      }
      return dropDecimalPrice;
    }

    public static string ConvertUSDPriceToCents(string PriceText)
    {
      string centPrice = PriceText;
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
      ICurrencyInfo usdInfo = CurrencyData.GetCurrencyInfo("USD");
      if (currency.SelectedDisplayCurrencyInfo.Equals(usdInfo))
      {
        if (PriceText.StartsWith("$0."))
        {
          centPrice = PriceText.Replace("$0.", String.Empty) + "&cent;";
        }
      }
      return centPrice;
    }
  }
}
