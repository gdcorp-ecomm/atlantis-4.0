using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductUpgradePath.Interface
{
  public class ProductUpgradePathResponseData : IResponseData
  {
    
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;

    private List<ProductOptions> _productOptions = new List<ProductOptions>();
    public List<ProductOptions> ProductOptions
    {
      get
      {
        return _productOptions;
      }
    }

    private Dictionary<int, UpgradeProductInfo> _products = new Dictionary<int, UpgradeProductInfo>();
    public Dictionary<int, UpgradeProductInfo> Products
    {
      get
      {
        return _products;
      }
    }

    private Dictionary<int, UpgradeProductInfo> _filteredProducts = new Dictionary<int, UpgradeProductInfo>();
    public Dictionary<int, UpgradeProductInfo> FilteredProducts
    {
      get
      {
        if (_filteredProducts.Count == 0)
        {
          SetupFilteredProducts();
        }
        return _filteredProducts;
      }
    }

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public ProductUpgradePathResponseData(Dictionary<int, UpgradeProductInfo> products, List<ProductOptions> filters)
    {
      _products = products;
      _productOptions = filters;
      SetupFilteredProducts();
    }

    private void SetupFilteredProducts()
    {
      _filteredProducts.Clear();
      foreach (ProductOptions currentOption in _productOptions)
      {
        System.Diagnostics.Debug.WriteLine(currentOption.Duration + ":" + currentOption.DurationUnit);
        foreach (KeyValuePair<int, UpgradeProductInfo> currentProduct in _products)
        {
          UpgradeProductInfo tempProduct=currentProduct.Value;
          System.Diagnostics.Debug.WriteLine(tempProduct.RecurringMethod+":"+tempProduct.PeriodCount);
          int monthlyUnit = currentOption.Duration / 12;
          if (currentOption.DurationUnit == DurationUnit.Month && tempProduct.RecurringMethod == UpgradeProductInfo.MONTHLY && currentOption.Duration == tempProduct.PeriodCount)
          {
            AddToFilteredProduct(currentOption, tempProduct);
            break;
          }
          else if (currentOption.DurationUnit == DurationUnit.Month && tempProduct.RecurringMethod == UpgradeProductInfo.QUARTERLY && currentOption.Duration == 3)
          {
            AddToFilteredProduct(currentOption, tempProduct);
            break;
          }
          else if (currentOption.DurationUnit == DurationUnit.Month && tempProduct.RecurringMethod == UpgradeProductInfo.SEMIANUUAL && currentOption.Duration == 6)
          {
            AddToFilteredProduct(currentOption, tempProduct);
            break;
          }
          else if (currentOption.DurationUnit == DurationUnit.Month && tempProduct.RecurringMethod == UpgradeProductInfo.ANNUAL && monthlyUnit == tempProduct.PeriodCount)
          {
            AddToFilteredProduct(currentOption, tempProduct);
            break;
          }
          else if (currentOption.DurationUnit == DurationUnit.Year && tempProduct.RecurringMethod == UpgradeProductInfo.ANNUAL && currentOption.Duration == tempProduct.PeriodCount)
          {
            AddToFilteredProduct(currentOption, tempProduct);
            break;
          }
        }
      }
    }

    private void AddToFilteredProduct(ProductOptions currentOption, UpgradeProductInfo currentProduct)
    {
      currentProduct.DisplayTerm = CreateDisplayTerm(currentOption);
      _filteredProducts[currentProduct.ProductID] = currentProduct;
      System.Diagnostics.Debug.WriteLine("Add: "+currentProduct.RecurringMethod + ":" + currentProduct.PeriodCount);
    }

    private string CreateDisplayTerm(ProductOptions currentOption)
    {
      string displayTerm = string.Empty;
      switch (currentOption.DurationUnit)
      {
        case DurationUnit.Month:
          if (currentOption.Duration == 1)
          {
            displayTerm = "1 Month";
          }
          else
          {
            displayTerm = currentOption.Duration + " Months";
          }
          break;
        case DurationUnit.Year:
          if (currentOption.Duration == 1)
          {
            displayTerm = "1 Year";
          }
          else
          {
            displayTerm = currentOption.Duration + " Years";
          }
          break;
      }
      return displayTerm;
    }

    public ProductUpgradePathResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public ProductUpgradePathResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "ProductUpgradePathResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
