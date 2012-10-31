using Atlantis.Framework.ProductPackager.Impl.ProductPackage;
using Atlantis.Framework.ProductPackager.Interface;

namespace Atlantis.Framework.ProductPackager.Impl
{
  internal class FbProductData : IProductData
  {
    private readonly ProductGroupAttributeProducts _fbProductGroupAttributeProduct;

    private int? _productId;
    public int ProductId
    {
      get
      {
        if(!_productId.HasValue)
        {
          _productId = ParseHelper.ParseInt(_fbProductGroupAttributeProduct.pfid, "ProductGroupAttributeProducts \"pfid\" is not an integer. Value: {0}");
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
          _quantity = ParseHelper.ParseInt(_fbProductGroupAttributeProduct.qty, "ProductGroupAttributeProducts \"qty\" is not an integer. Value: {0}");
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

    private bool? _isDefault;
    public bool IsDefault
    {
      get
      {
        if(!_isDefault.HasValue)
        {
          _isDefault = ParseHelper.ParseBool(_fbProductGroupAttributeProduct.@default, "ProductGroupAttributeProducts \"default\" is not an bool. Value: {0}");
        }
        return _isDefault.Value;
      }
    }

    internal FbProductData(ProductGroupAttributeProducts fbProductGroupAttributeProduct)
    {
      _fbProductGroupAttributeProduct = fbProductGroupAttributeProduct;
    }
  }
}