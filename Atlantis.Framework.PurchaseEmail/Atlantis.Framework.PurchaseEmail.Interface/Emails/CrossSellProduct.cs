
namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal class CrossSellProduct
  {
    CrossSellConfigProductId _id;
    string _productUrl;
    string _productName;
    string _priceText;
    string _productDescription;
    string _savingsText;

    public CrossSellConfigProductId Id { get { return _id; } set { _id = value; } }
    public string ProductUrl { get { return _productUrl; } set { _productUrl = value; } }
    public string ProductName { get { return _productName; } set { _productName = value; } }
    public string PriceText { get { return _priceText; } set { _priceText = value; } }
    public string ProductDescription { get { return _productDescription; } set { _productDescription = value; } }
    public string SavingsText { get { return _savingsText; } set { _savingsText = value; } }

    public CrossSellProduct(CrossSellConfigProductId id,
    string productUrl,
    string productName,
    string priceText,
    string productDescription,
    string savingsText)
    {
      _id = id;
      _productUrl = productUrl;
      _productName = productName;
      _priceText = priceText;
      _productDescription = productDescription;
      _savingsText = savingsText;
    }
  }
}
