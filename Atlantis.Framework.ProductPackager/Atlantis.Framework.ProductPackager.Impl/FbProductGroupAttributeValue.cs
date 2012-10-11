using System.Collections.Generic;
using Atlantis.Framework.ProductPackager.Impl.ProductPackage;
using Atlantis.Framework.ProductPackager.Interface;

namespace Atlantis.Framework.ProductPackager.Impl
{
  internal class FbProductGroupAttributeValue : IProductGroupAttributeValue
  {
    private readonly ProductGroupAttribValue _fbProductGroupAttributeValue;

    private int? _id;
    public int Id
    {
      get
      {
        if(!_id.HasValue)
        {
          _id = ParseHelper.ParseInt(_fbProductGroupAttributeValue.id, "ProductGroupAttribValue \"id\" is not an integer. Value: {0}");
        }
        return _id.Value;
      }
    }

    private string _name;
    public string Name
    {
      get
      {
        if (_name == null)
        {
          _name = _fbProductGroupAttributeValue.name;
        }
        return _name;
      }
    }

    private string _value;
    public string Value
    {
      get
      {
        if (_value == null)
        {
          _value = _fbProductGroupAttributeValue.val;
        }
        return _value;
      }
    }

    private bool? _isDefault;
    public bool IsDefault
    {
      get
      {
        if(!_isDefault.HasValue)
        {
          _isDefault = ParseHelper.ParseBool(_fbProductGroupAttributeValue.@default, "ProductGroupAttribValue \"default\" is not an integer. Value: {0}");
        }
        return _isDefault.Value;
      }
    }

    private IList<IProductData> _productData; 
    public IList<IProductData> ProductDataList
    {
      get
      {
        if(_productData == null)
        {
          _productData = PopulateProductDataList(_fbProductGroupAttributeValue);
        }
        return _productData;
      }
    }

    internal FbProductGroupAttributeValue(ProductGroupAttribValue fbProductGroupAttributeValue)
    {
      _fbProductGroupAttributeValue = fbProductGroupAttributeValue;
    }

    private static IList<IProductData> PopulateProductDataList(ProductGroupAttribValue fbProductGroupAttributeValue)
    {
      List<ProductGroupAttributeProducts> productGroupAttributeProductsList = new List<ProductGroupAttributeProducts>(fbProductGroupAttributeValue.Prods);
      productGroupAttributeProductsList.Sort(new DisplayOrderComparer());

      IList<IProductData> productDataList = new List<IProductData>(productGroupAttributeProductsList.Count);
      foreach (ProductGroupAttributeProducts fbProductGroupAttributeProduct in productGroupAttributeProductsList)
      {
        productDataList.Add(new FbProductData(fbProductGroupAttributeProduct));
      }

      return productDataList;
    }

    private class DisplayOrderComparer : IComparer<ProductGroupAttributeProducts>
    {
      public int Compare(ProductGroupAttributeProducts x, ProductGroupAttributeProducts y)
      {
        int xDisplayOrder;
        int.TryParse(x.disporder, out xDisplayOrder);

        int yDisplayOrder;
        int.TryParse(y.disporder, out yDisplayOrder);

        return xDisplayOrder.CompareTo(yDisplayOrder);
      }
    }
  }
}