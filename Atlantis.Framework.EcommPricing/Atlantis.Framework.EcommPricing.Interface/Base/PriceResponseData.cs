using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPricing.Interface.Base
{
  public abstract class PriceResponseData : IResponseData
  {
    public bool IsPriceFound { get; protected set; }
    public int Price { get; protected set; }
    public bool IsEstimate { get; protected set; }

    #region Constructors
    protected PriceResponseData()
    {
      IsPriceFound = false;
      Price = 0;
      IsEstimate = false;
    }

    protected PriceResponseData(bool isPriceFound, int price, bool isEstimate)
    {
      IsPriceFound = isPriceFound;
      Price = price;
      IsEstimate = isEstimate;
    }
    #endregion

    #region IResponseData Members

    public string ToXML()
    {
      var element = new XElement(this.GetType().Name);
      element.Add(
        new XAttribute("priceFound", IsPriceFound.ToString()),
        new XAttribute("price", Price.ToString()),
        new XAttribute("isEstimate", IsEstimate.ToString()));

      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return null;
    }

    #endregion
  }
}
