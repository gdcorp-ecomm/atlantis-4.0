using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Tokens.Interface;
using System;
using System.Web;

namespace Atlantis.Framework.TH.Products
{
  /// <summary>
  /// This class gets a new instance per call to the Evaluate Tokens method.
  /// It is safe to use member variables on this class.
  /// </summary>
  internal class ProductPriceRenderContext
  {
    IProductProvider _products;
    ICurrencyProvider _currency;
    ISiteContext _siteContext;
    Lazy<bool> _maskPricesIfAllowed;

    internal ProductPriceRenderContext(IProviderContainer container)
    {
      _siteContext = container.Resolve<ISiteContext>();
      _products = container.Resolve<IProductProvider>();
      _currency = container.Resolve<ICurrencyProvider>();
      _maskPricesIfAllowed = new Lazy<bool>(() => { return GetIsPriceMaskOn(); });
    }

    private bool GetIsPriceMaskOn()
    {
      bool result = false;
      if ((_siteContext.ContextId == 6) && (HttpContext.Current != null) && (HttpContext.Current.Request != null))
      {
        string appHeaderQuery = HttpContext.Current.Request.QueryString["app_hdr"];
        if ("1387".Equals(appHeaderQuery))
        {
          result = true;
        }
      }
      return result;
    }

    internal bool RenderToken(IToken token)
    {
      bool result = true;

      ProductPriceToken ppToken = token as ProductPriceToken;
      if (ppToken != null)
      {
        switch (ppToken.RenderType)
        {
          case "current":
          case "list":
            result = RenderSimplePrice(ppToken);
            break;
          case "template":
            result = RenderTemplatePrice(ppToken);
            break;
          default:
            result = false;
            ppToken.TokenError = "Invalid element root: " + ppToken.RenderType;
            ppToken.TokenResult = string.Empty;
            break;
        }
      }
      else
      {
        result = false;
        ppToken.TokenError = "Cannot convert IToken to ProductPriceToken";
        ppToken.TokenResult = string.Empty;
      }

      return result;
    }

    private ICurrencyPrice GetPrice(ProductPriceToken token, bool useListPrice)
    {
      ICurrencyPrice result = null;

      if (token.ProductId != 0)
      {
        IProduct product = _products.GetProduct(token.ProductId);

        RecurringPaymentUnitType periodType = RecurringPaymentUnitType.Unknown;
        switch (token.Period.ToLowerInvariant())
        {
          case "monthly":
            periodType = RecurringPaymentUnitType.Monthly;
            break;
          case "yearly":
            periodType = RecurringPaymentUnitType.Annual;
            break;
          case "semiannual":
            periodType = RecurringPaymentUnitType.SemiAnnual;
            break;
          case "quarterly":
            periodType = RecurringPaymentUnitType.Quarterly;
            break;
        }

        ICurrencyInfo txCurrency = null;
        if (token.CurrencyType != null)
        {
          txCurrency = _currency.GetValidCurrencyInfo(token.CurrencyType);
        }

        if (useListPrice)
        {
          result = product.GetListPrice(periodType, token.PriceType, txCurrency);
        }
        else
        {
          result = product.GetCurrentPrice(periodType, token.PriceType, txCurrency, token.ISC);
        }
      }

      return result;
    }

    private PriceFormatOptions GetPriceFormatOptions(ProductPriceToken token)
    {
      PriceFormatOptions result = PriceFormatOptions.None;

      if (token.DropDecimal)
      {
        result |= PriceFormatOptions.DropDecimal;
      }

      if (token.DropSymbol)
      {
        result |= PriceFormatOptions.DropSymbol;
      }
      else if (!token.HtmlSymbol)
      {
        result |= PriceFormatOptions.AsciiSymbol;
      }

      if ("parentheses".Equals(token.NegativeFormat, StringComparison.OrdinalIgnoreCase))
      {
        result |= PriceFormatOptions.NegativeParentheses;
      }

      return result;
    }


    private PriceTextOptions GetPriceTextOptions(ProductPriceToken token)
    {
      PriceTextOptions result = PriceTextOptions.None;

      if (_maskPricesIfAllowed.Value && token.AllowMask)
      {
        result |= PriceTextOptions.MaskPrices;
      }

      if (!"notallowed".Equals(token.NegativeFormat, StringComparison.OrdinalIgnoreCase))
      {
        result |= PriceTextOptions.AllowNegativePrice;
      }

      return result;
    }

    private bool RenderSimplePrice(ProductPriceToken token)
    {
      bool result = false;
      string priceText = string.Empty;
      bool useListPrice = "list".Equals(token.RenderType, StringComparison.OrdinalIgnoreCase);

      ICurrencyPrice price = GetPrice(token, useListPrice);

      if (price != null)
      {
        PriceFormatOptions formatOptions = GetPriceFormatOptions(token);

        if (token.CurrencyType != null)
        {
          priceText = _currency.PriceFormat(price, formatOptions);
        }
        else
        {
          PriceTextOptions textOptions = GetPriceTextOptions(token);
          priceText = _currency.PriceText(price, textOptions, formatOptions);
        }

        result = true;
      }

      token.TokenResult = priceText;
      return result;
    }

    private bool RenderTemplatePrice(ProductPriceToken token)
    {
      bool result = false;
      string priceText = string.Empty;

      ICurrencyPrice listPrice = GetPrice(token, true);
      ICurrencyPrice currentPrice = GetPrice(token, false);

      if ((listPrice != null) && (currentPrice != null))
      {
        PriceFormatOptions formatOptions = GetPriceFormatOptions(token);
        string listText;
        string currentText;

        if (token.CurrencyType != null)
        {
          listText = _currency.PriceFormat(listPrice, formatOptions);
          currentText = _currency.PriceFormat(currentPrice, formatOptions);
        }
        else
        {
          PriceTextOptions textOptions = GetPriceTextOptions(token);
          listText = _currency.PriceText(listPrice, textOptions, formatOptions);
          currentText = _currency.PriceText(currentPrice, textOptions, formatOptions);
        }

        string formatTemplate = token.NoStrikeTemplate;
        if (listPrice.Price > currentPrice.Price)
        {
          formatTemplate = token.StrikeTemplate;
        }

        priceText = string.Format(formatTemplate, currentText, listText);
        result = true;
      }

      token.TokenResult = priceText;
      return result;
    }

  }
}
