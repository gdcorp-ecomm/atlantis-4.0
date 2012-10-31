using System.Collections.Generic;
using Atlantis.Framework.ProductPackager.Impl.ProductPackage;
using Atlantis.Framework.ProductPackager.Interface;

namespace Atlantis.Framework.ProductPackager.Impl
{
  internal class FbProductGroupPackageData : IProductGroupPackageData
  {
    private readonly ProductGroupProducts _fbProductGroupPackageData;

    private int? _productId;
    public int ProductId
    {
      get
      {
        if(!_productId.HasValue)
        {
          _productId = ParseHelper.ParseInt(_fbProductGroupPackageData.pfid, "ProductGroupPackageData \"pfid\" is not an integer. Value: {0}");
        }
        return _productId.Value;
      }
    }

    private int? _quantity;
    public int Quantity
    {
      get
      {
        if(!_quantity.HasValue)
        {
          _quantity = ParseHelper.ParseInt(_fbProductGroupPackageData.qty, "ProductGroupPackageData \"qty\" is not an integer. Value: {0}");
        }
        return _quantity.Value;
      }
    }

    private double? _duration;
    public double? Duration
    {
      get
      {
        if(!_duration.HasValue)
        {
          _duration = null; // TODO: Hard coded until the service returns this
        }
        return _duration;
      }
    }

    private IList<IProductPackageMapping> _productPackageMappings;
    public IList<IProductPackageMapping> ProductPackageMappings
    {
      get
      {
        if(_productPackageMappings == null)
        {
          _productPackageMappings = PopulateProductPackageMappings(_fbProductGroupPackageData);
        }
        return _productPackageMappings;
      }
    }

    internal FbProductGroupPackageData(ProductGroupProducts fbProductGroupPackageData)
    {
      _fbProductGroupPackageData = fbProductGroupPackageData;

      
    }

    private static IList<IProductPackageMapping> PopulateProductPackageMappings(ProductGroupProducts fbProductGroupPackageData)
    {
      IList<IProductPackageMapping>  productPackageMappings = new List<IProductPackageMapping>(fbProductGroupPackageData.Pkgs.Length);
      
      foreach (ProductGroupPackage fbProductPackageMapping in fbProductGroupPackageData.Pkgs)
      {
        productPackageMappings.Add(new FbProductPackageMapping(fbProductPackageMapping));
      }

      return productPackageMappings;
    }
  }
}
