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

    private ICurrencyPrice GetProductPrice(IProduct product, string renderType, string currencyTypeOverride)
    {
      ICurrencyPrice result;

      if ("list".Equals(renderType))
      {
        result = product.ListPrice;
      }
      else
      {
        result = product.CurrentPrice;
      }

      return result;
    }

    private ICurrencyPrice GetViewPrice(IProductView productView, string renderType, RecurringPaymentUnitType periodType)
    {
      ICurrencyPrice result;

      if ("list".Equals(renderType))
      {
        if (periodType == RecurringPaymentUnitType.Annual)
        {
          result = productView.YearlyListPrice;
        }
        else
        {
          result = productView.MonthlyListPrice;
        }
      }
      else
      {
        if (periodType == RecurringPaymentUnitType.Annual)
        {
          result = productView.YearlyCurrentPrice;
        }
        else
        {
          result = productView.MonthlyCurrentPrice;
        }
      }

      return result;
    }

    private bool RenderSimplePrice(ProductPriceToken token)
    {
      bool result = false;
      string priceText = string.Empty;

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
        }

        ICurrencyPrice price;

        if (periodType == RecurringPaymentUnitType.Unknown)
        {
          price = GetProductPrice(product, token.RenderType, token.CurrencyType);
        }
        else
        {
          IProductView productView = _products.NewProductView(product);
          price = GetViewPrice(productView, token.RenderType, periodType);
        }

        if (token.CurrencyType == null)
        {
          bool maskPrices = _maskPricesIfAllowed.Value && token.AllowMask;
          priceText = _currency.PriceText(price, maskPrices, token.DropDecimal, token.DropSymbol);
        }
        else
        {
          priceText = _currency.PriceFormat(price, token.DropDecimal, token.DropSymbol);
        }
        
        result = true;
      }

      token.TokenResult = priceText;
      return result;
    }


  }
}
