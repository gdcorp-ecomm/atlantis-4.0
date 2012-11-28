using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Currency;
using System.Web;
using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.TH.Currency
{
  /// <summary>
  /// This class gets a new instance per call to the Evaluate Tokens method.
  /// It is safe to use member variables on this class.
  /// </summary>
  internal class CurrencyPriceRenderContext
  {
    ICurrencyProvider _currency;
    ISiteContext _siteContext;
    Lazy<bool> _maskPricesIfAllowed;

    internal CurrencyPriceRenderContext(IProviderContainer container)
    {
      _siteContext = container.Resolve<ISiteContext>();
      _currency = container.Resolve<ICurrencyProvider>();
      _maskPricesIfAllowed = new Lazy<bool>(() => { return GetIsPriceMaskOn(); });
    }

    private bool GetIsPriceMaskOn()
    {
      bool result = false;
      if ((_siteContext.ContextId == 6) && (HttpContext.Current != null) && (HttpContext.Current.Request != null))
      {
        string appHeaderQuery = HttpContext.Current.Request.QueryString["app_hdr"];
        if (string.Equals(appHeaderQuery, "1387", StringComparison.Ordinal))
        {
          result = true;
        }
      }
      return result;
    }

    internal bool RenderToken(IToken token)
    {
      bool result = true;

      CurrencyPriceToken cpToken = token as CurrencyPriceToken;
      if (cpToken != null)
      {
        switch (cpToken.RenderType)
        {
          case "currencyprice":
            result = RenderSimplePrice(cpToken);
            break;
          case "template":
            break;
          default:
            result = false;
            cpToken.TokenError = "Invalid element root: " + cpToken.RenderType;
            cpToken.TokenResult = string.Empty;
            break;
        }
      }
      else
      {
        result = false;
        cpToken.TokenError = "Cannot convert IToken to CurrencyPriceToken";
        cpToken.TokenResult = string.Empty;
      }

      return result;
    }

    private ICurrencyPrice GetPrice(CurrencyPriceToken token)
    {
      ICurrencyPrice result = _currency.NewCurrencyPriceFromUSD(token.UsdAmount);

      if (token.CurrencyType != null)
      {
        ICurrencyInfo txCurrency = _currency.GetValidCurrencyInfo(token.CurrencyType);
        result = _currency.ConvertPrice(result, txCurrency);
      }

      return result;
    }

    private PriceFormatOptions GetPriceFormatOptions(CurrencyPriceToken token)
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

      if (string.Equals("parentheses", token.NegativeFormat, StringComparison.OrdinalIgnoreCase))
      {
        result |= PriceFormatOptions.NegativeParentheses;
      }

      return result;
    }


    private PriceTextOptions GetPriceTextOptions(CurrencyPriceToken token)
    {
      PriceTextOptions result = PriceTextOptions.None;

      if (_maskPricesIfAllowed.Value && token.AllowMask)
      {
        result |= PriceTextOptions.MaskPrices;
      }

      if (!string.Equals("notallowed", token.NegativeFormat, StringComparison.OrdinalIgnoreCase))
      {
        result |= PriceTextOptions.AllowNegativePrice;
      }

      return result;
    }

    private bool RenderSimplePrice(CurrencyPriceToken token)
    {
      bool result = false;
      string priceText = string.Empty;

      ICurrencyPrice price = GetPrice(token);

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
  }
}
