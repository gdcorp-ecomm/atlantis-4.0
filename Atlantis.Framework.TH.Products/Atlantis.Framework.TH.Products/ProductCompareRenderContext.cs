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

    internal ProductCompareRenderContext(IProviderContainer container)
    {
      _siteContext = container.Resolve<ISiteContext>();
      _products = container.Resolve<IProductProvider>();
    }

    internal bool RenderToken(IToken token)
    {
      bool result = true;

      ProductCompareToken pcToken = token as ProductCompareToken;
      if (pcToken != null)
      {
        switch (pcToken.RenderType)
        {
          case "percent":
            result = RenderPercent(pcToken);
            break;
          case "times":
            result = RenderTimes(pcToken);
            break;
          default:
            result = false;
            pcToken.TokenError = "Invalid element root: " + pcToken.RenderType;
            pcToken.TokenResult = string.Empty;
            break;
        }
      }
      else
      {
        result = false;
        pcToken.TokenError = "Cannot convert IToken to ProductCompareToken";
        pcToken.TokenResult = string.Empty;
      }
      return result;
    }

    private bool RenderTimes(ProductCompareToken token)
    {
      bool result = false;
      string tokenResult = string.Empty;
      double saving = -1;
      if ((token.PrimaryProductId != 0) && (token.SecondaryProductId != 0 || token.SecondaryPrice != 0))
      {
        IProductView primaryProduct = _products.NewProductView(_products.GetProduct(token.PrimaryProductId));
        if (token.SecondaryProductId != 0)
        {
          IProductView secondaryProduct = _products.NewProductView(_products.GetProduct(token.SecondaryProductId));
          saving = secondaryProduct.MonthlyCurrentPrice.Price / primaryProduct.MonthlyCurrentPrice.Price;
        }
        else
        {
          if (token.SecondaryPeriod == "yearly")
          {
            saving = (double)token.SecondaryPrice / primaryProduct.YearlyCurrentPrice.Price;
          }
          else
          {
            saving = (double)token.SecondaryPrice / primaryProduct.MonthlyCurrentPrice.Price;
          }
        }

        if (saving >= (double)token.HideBelow)
        {
          tokenResult = Math.Round(saving, 1).ToString();
          result = true;
        }
      }

      if (!string.IsNullOrEmpty(tokenResult) && !string.IsNullOrEmpty(token.Html))
      {
        if (token.Html.Contains("{0}"))
        {
          tokenResult = token.Html.Replace("{0}", tokenResult);
        }
      }

      token.TokenResult = tokenResult;
      return result;
    }

    private bool RenderPercent(ProductCompareToken token)
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
          if (token.SecondaryPeriod == "yearly")
          {
            percent = Convert.ToInt32((1 - (((double)primaryProduct.YearlyCurrentPrice.Price) / (double)token.SecondaryPrice)) * 100);
          }
          else
          {
            percent = Convert.ToInt32((1 - (((double)primaryProduct.MonthlyCurrentPrice.Price) / (double)token.SecondaryPrice)) * 100);
          }
          if (percent >= token.HideBelow)
          {
            tokenResult = percent.ToString();
            result = true;
          }
        }
      }

      if (!string.IsNullOrEmpty(tokenResult) && !string.IsNullOrEmpty(token.Html))
      {
        if (token.Html.Contains("{0}"))
        {
          tokenResult = token.Html.Replace("{0}", tokenResult);
        }
      }

      token.TokenResult = tokenResult;
      return result;
    }
  }
}
