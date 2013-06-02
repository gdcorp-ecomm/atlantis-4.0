using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPricing.Interface
{
  public class ProductIsOnSaleResponseData : IResponseData
  {
    private static ProductIsOnSaleResponseData _isOnSaleResponse;
    private static ProductIsOnSaleResponseData _notOnSaleResponse;

    public bool IsOnSale { get; private set; }

    #region Constructors
    static ProductIsOnSaleResponseData()
    {
      _isOnSaleResponse = new ProductIsOnSaleResponseData(true);
      _notOnSaleResponse = new ProductIsOnSaleResponseData(false);
    }

    private ProductIsOnSaleResponseData(bool isOnSale)
    {
      IsOnSale = isOnSale;
    }

    public static ProductIsOnSaleResponseData OnSale
    {
      get { return _isOnSaleResponse; }
    }

    public static ProductIsOnSaleResponseData NotOnSale
    {
      get { return _notOnSaleResponse; }
    }

    #endregion

    #region IResponseData Members

    public string ToXML()
    {
      var element = new XElement(this.GetType().Name);
      element.Add(
        new XAttribute("isOnSale", IsOnSale.ToString()));

      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return null;
    }

    #endregion
  }
}
