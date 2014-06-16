using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.Basket.Interface
{
  /// <summary>
  /// The BasketDeleteResponseData class stores response status.
  /// </summary>
  public class BasketDeleteResponseData : ResponseData
  {
    /// <summary>
    /// Gets response status.
    /// </summary>
    public BasketResponseStatus Status { get; private set; }

    /// <summary>
    /// Initializes a new instance of the BasketDeleteResponseData class using
    /// response xml specified.
    /// </summary>
    /// <param name="status">A BasketResponseStatus that represents the status 
    /// of the response</param>
    private BasketDeleteResponseData(BasketResponseStatus status)
    {
      Status = status;
    }

    /// <summary>
    /// Creates a new instance of the BasketDeleteResponseData class using
    /// the response xml string specified.
    /// </summary>
    /// <param name="responseXml">A string that has the xml representation of the response</param>
    /// <returns></returns>
    public static BasketDeleteResponseData FromData(string responseXml)
    {
      var responseElement = XElement.Parse(responseXml);
      var status = BasketResponseStatus.FromResponseElement(responseElement);
      return new BasketDeleteResponseData(status);
    }
  }
}
