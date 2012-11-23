
namespace Atlantis.Framework.ProductPackagerAddToCartHandler
{
  internal class AddOnSelection
  {
    private readonly string[] _addOnValues;

    internal AddOnSelection(string inputValue)
    {
      _addOnValues = inputValue.Split(new[] {'|'});
    }

    private int? _productId;
    internal int ProductId
    {
      get
      {
        if (!_productId.HasValue)
        {
          _productId = 0;
          int parsedProductId;
          if (_addOnValues.Length == 2 && int.TryParse(_addOnValues[0], out parsedProductId))
          {
            _productId = parsedProductId;
          }
        }
        return _productId.Value;
      }
    }

    private int? _quantity;
    internal int Quantity
    {
      get
      {
        if (!_quantity.HasValue)
        {
          _quantity = 1;
          int parsedQuantity;
          if (_addOnValues.Length == 2 && int.TryParse(_addOnValues[1], out parsedQuantity))
          {
            _quantity = parsedQuantity;
          }
        }

        return _quantity.Value;
      }
    }

    internal bool IsValid
    {
      get { return _addOnValues.Length == 2 && ProductId > 0; }
    }
  }
}
