using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Tokens.Interface;
using System;
using System.Web;

namespace Atlantis.Framework.TH.Products
{
  internal class ProductCompareRenderContext
  {
    IProductProvider _products;
    ISiteContext _siteContext;
    ICurrencyProvider _currency;
    Lazy<bool> _maskPricesIfAllowed;

    internal ProductCompareRenderContext(IProviderContainer container)
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

      ProductCompareToken pcToken = token as ProductCompareToken;
      int value1 = 0;
      int value2 = 0;
      pcToken.TokenResult = string.Empty;

      if (pcToken != null)
      {
        if (DetermineValues(pcToken, out value1, out value2))
        {
          switch (pcToken.RenderType)
          {
            case "percent":
              result = RenderPercent(pcToken, value1, value2);
              break;
            case "times":
              result = RenderTimes(pcToken, value1, value2);
              break;
            case "addition":
              result = RenderAddition(pcToken, value1, value2);
              break;
            case "subtraction":
              result = RenderSubtraction(pcToken, value1, value2);
              break;
            case "division":
              result = RenderDivision(pcToken, value1, value2);
              break;
            case "multiplication":
              result = RenderMultiplication(pcToken, value1, value2);
              break;
            default:
              result = false;
              pcToken.TokenError = "Invalid element root: " + pcToken.RenderType;
              break;
          }
        }
      }
      else
      {
        result = false;
        pcToken.TokenError = "Cannot convert IToken to ProductCompareToken";
      }
      if (result)
      {
        HtmlFormatTokenResult(pcToken);
      }
      return result;
    }

    private bool DetermineValues(ProductCompareToken token, out int value1, out int value2)
    {
      value1 = 0;
      value2 = 0;
      bool result = false;
      if ((token.PrimaryProductId != 0) && (token.SecondaryProductId != 0 || token.SecondaryPrice != 0))
      {
        IProductView primaryProduct = _products.NewProductView(_products.GetProduct(token.PrimaryProductId));
        if (token.SecondaryProductId != 0)
        {
          IProductView secondaryProduct = _products.NewProductView(_products.GetProduct(token.SecondaryProductId));
          value1 = primaryProduct.MonthlyCurrentPrice.Price;
          value2 = secondaryProduct.MonthlyCurrentPrice.Price;
        }
        else
        {
          if (token.SecondaryPeriod == "yearly")
          {
            value1 = primaryProduct.YearlyCurrentPrice.Price;
          }
          else
          {
            value1 = primaryProduct.MonthlyCurrentPrice.Price;
          }
          value2 = token.SecondaryPrice;
        }
        result = true;
      }
      return result;
    }

    private void HtmlFormatTokenResult(ProductCompareToken token)
    {
      if (!string.IsNullOrEmpty(token.TokenResult) && !string.IsNullOrEmpty(token.Html))
      {
        if (token.Html.Contains("{0}"))
        {
          token.TokenResult = token.Html.Replace("{0}", token.TokenResult);
        }
      }
    }

    protected string GetPriceText(int price)
    {
      var ci = _currency.GetCurrencyInfo("USD");
      var cp = _currency.NewCurrencyPrice(price, ci, CurrencyPriceType.Transactional);

      PriceTextOptions options = PriceTextOptions.None;
      if (_maskPricesIfAllowed.Value)
      {
        options = PriceTextOptions.MaskPrices;
      }

      return _currency.PriceText(cp, options);
    }

    #region RenderMethods

    private bool RenderTimes(ProductCompareToken token, int value1, int value2)
    {
      bool result = false;
      double saving = (double)value2 / value1;
      if (saving >= (double)token.HideBelow)
      {
        token.TokenResult = Math.Round(saving, 1).ToString();
        result = true;
      }
      return result;
    }

    private bool RenderAddition(ProductCompareToken token, int value1, int value2)
    {
      bool result = false;
      int total = value1 + value2;
      if (total >= token.HideBelow)
      {
        token.TokenResult = GetPriceText(total);
        result = true;
      }
      return result;
    }

    private bool RenderSubtraction(ProductCompareToken token, int value1, int value2)
    {
      bool result = false;
      int total = value1 - value2;
      if (total >= token.HideBelow)
      {
        token.TokenResult = GetPriceText(total);
        result = true;
      }
      return result;
    }

    private bool RenderMultiplication(ProductCompareToken token, int value1, int value2)
    {
      bool result = false;
      int total = value1 * value2;
      if (total >= token.HideBelow)
      {
        token.TokenResult = GetPriceText(total);
        result = true;
      }
      return result;
    }

    private bool RenderDivision(ProductCompareToken token, int value1, int value2)
    {
      bool result = false;
      if (value2 != 0)
      {
        int total = value1 / value2;
        if (total >= (double)token.HideBelow)
        {
          token.TokenResult = GetPriceText(total);
          result = true;
        }
      }
      return result;
    }

    private bool RenderPercent(ProductCompareToken token, double value1, double value2)
    {
      bool result = false;
      string tokenResult = string.Empty;
      int percent;

      if ((token.PrimaryProductId != 0) && (token.SecondaryProductId != 0 || token.SecondaryPrice != 0))
      {
        IProductView primaryProduct = _products.NewProductView(_products.GetProduct(token.PrimaryProductId));
        if (token.SecondaryProductId != 0)
        {
          IProduct secondaryProduct = _products.GetProduct(token.SecondaryProductId);
          primaryProduct.CalculateSavings(secondaryProduct);
          if (primaryProduct.SavingsPercentage >= token.HideBelow)
          {
            tokenResult = primaryProduct.SavingsPercentage.ToString();
            result = true;
          }
        }
        else
        {
          percent = Convert.ToInt32((1 - (((double)value1) / (double)value2)) * 100);
          if (percent >= token.HideBelow)
          {
            tokenResult = percent.ToString();
            result = true;
          }
        }
      }

      token.TokenResult = tokenResult;
      return result;
    }
  }

    #endregion
}
