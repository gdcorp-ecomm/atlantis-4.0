using System.Collections.Generic;
using Atlantis.Framework.ProductPackager.Impl.ProductPackage;
using Atlantis.Framework.ProductPackager.Interface;

namespace Atlantis.Framework.ProductPackager.Impl
{
  internal class FbProductGroupAttribute : IProductGroupAttribute
  {
    private readonly ProductGroupAttribute _fbProductGroupAttribute;

    private string _name;
    public string Name
    {
      get
      {
        if(_name == null)
        {
          _name = _fbProductGroupAttribute.name;
        }
        return _name;
      }
    }

    private IList<IProductGroupAttributeValue> _productGroupAttributeValues;
    public IList<IProductGroupAttributeValue> ProductGroupAttributeValues
    {
      get
      {
        if(_productGroupAttributeValues == null)
        {
          _productGroupAttributeValues = PopulateProductGroupAttributeValues(_fbProductGroupAttribute);
        }
        return _productGroupAttributeValues;
      }
    }

    internal FbProductGroupAttribute(ProductGroupAttribute fbProductGroupAttribute)
    {
      _fbProductGroupAttribute = fbProductGroupAttribute;
    }

    private static IList<IProductGroupAttributeValue> PopulateProductGroupAttributeValues(ProductGroupAttribute fbProductGroupAttribute)
    {
      List<ProductGroupAttribValue> attribValsList = new List<ProductGroupAttribValue>(fbProductGroupAttribute.AttribVals);
      attribValsList.Sort(new DisplayOrderComparer());

      IList<IProductGroupAttributeValue> productGroupAttributeValues = new List<IProductGroupAttributeValue>(attribValsList.Count);
      foreach (ProductGroupAttribValue fbProductGroupAttributeValue in attribValsList)
      {
        productGroupAttributeValues.Add(new FbProductGroupAttributeValue(fbProductGroupAttributeValue));
      }

      return productGroupAttributeValues;
    }

    private class DisplayOrderComparer : IComparer<ProductGroupAttribValue>
    {
      public int Compare(ProductGroupAttribValue x, ProductGroupAttribValue y)
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
