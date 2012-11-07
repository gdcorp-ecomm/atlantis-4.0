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
      set
      {
        _productOptions = value;
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

    public ProductUpgradePathResponseData(Dictionary<int, UpgradeProductInfo> products, List<ProductOptions> filters, int originalPfId)
    {
      _products = products;
      _productOptions = filters;
      SetupFilteredProducts(originalPfId);
    }

    public void SetupFilteredProducts(int originalPfid)
    {
      _filteredProducts.Clear();
      UpgradeProductInfo origproduct = _products[originalPfid];
      foreach (ProductOptions currentOption in _productOptions)
      {
        System.Diagnostics.Debug.WriteLine(currentOption.Duration + ":" + currentOption.DurationUnit);
        int monthlyUnit = currentOption.Duration / 12;
        if (!_filteredProducts.ContainsKey(originalPfid))
        {
          if (currentOption.DurationUnit == DurationUnit.Month && origproduct.RecurringMethod == UpgradeProductInfo.MONTHLY && currentOption.Duration >= origproduct.PeriodCount)
          {
            AddToFilteredProduct(origproduct);
          }
          else if (currentOption.DurationUnit == DurationUnit.Month && origproduct.RecurringMethod == UpgradeProductInfo.QUARTERLY && currentOption.Duration >= 3)
          {
            AddToFilteredProduct(origproduct);
          }
          else if (currentOption.DurationUnit == DurationUnit.Month && origproduct.RecurringMethod == UpgradeProductInfo.SEMIANUUAL && currentOption.Duration >= 6)
          {
            AddToFilteredProduct(origproduct);
          }
          else if (currentOption.DurationUnit == DurationUnit.Month && origproduct.RecurringMethod == UpgradeProductInfo.ANNUAL && monthlyUnit >= origproduct.PeriodCount)
          {
            AddToFilteredProduct(origproduct);
          }
          else if (currentOption.DurationUnit == DurationUnit.Year && origproduct.RecurringMethod == UpgradeProductInfo.ANNUAL && currentOption.Duration >= origproduct.PeriodCount)
          {
            AddToFilteredProduct(origproduct);
          }
        }
        foreach (KeyValuePair<int, UpgradeProductInfo> currentProduct in _products)
        {
          UpgradeProductInfo tempProduct = currentProduct.Value;
          System.Diagnostics.Debug.WriteLine(tempProduct.RecurringMethod + ":" + tempProduct.PeriodCount);
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
      if (!_filteredProducts.ContainsKey(originalPfid))
      {
        AddToFilteredProduct(origproduct);
      }
    }

    private void AddToFilteredProduct(UpgradeProductInfo currentProduct)
    {
      currentProduct.DisplayTerm = CreateDisplayTerm(currentProduct);
      _filteredProducts[currentProduct.ProductID] = currentProduct;
    }

    private void AddToFilteredProduct(ProductOptions currentOption, UpgradeProductInfo currentProduct)
    {
      currentProduct.DisplayTerm = CreateDisplayTerm(currentOption);
      _filteredProducts[currentProduct.ProductID] = currentProduct;
      System.Diagnostics.Debug.WriteLine("Add: " + currentProduct.RecurringMethod + ":" + currentProduct.PeriodCount);
    }

    private string CreateDisplayTerm(UpgradeProductInfo currentProduct)
    {
      string displayTerm = string.Empty;
      switch (currentProduct.RecurringMethod)
      {
        case UpgradeProductInfo.MONTHLY:
        case UpgradeProductInfo.QUARTERLY:
        case UpgradeProductInfo.SEMIANUUAL:
          if (currentProduct.PeriodCount == 1)
          {
            displayTerm = "1 Month";
          }
          else
          {
            displayTerm = currentProduct.PeriodCount + " Months";
          }
          break;
        case UpgradeProductInfo.ANNUAL:
          if (currentProduct.PeriodCount == 1)
          {
            displayTerm = "1 Year";
          }
          else
          {
            displayTerm = currentProduct.PeriodCount + " Years";
          }
          break;
      }
      return displayTerm;
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
