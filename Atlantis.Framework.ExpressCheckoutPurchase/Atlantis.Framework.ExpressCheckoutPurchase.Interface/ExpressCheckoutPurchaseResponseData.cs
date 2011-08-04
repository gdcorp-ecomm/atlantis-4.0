using System;
using System.Text;
using System.Xml.Linq;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ExpressCheckoutPurchase.Interface
{
  public class ExpressCheckoutPurchaseResponseData : IResponseData
  {
    #region Properties
    public struct ErrorStruct
    {
      public string ErrorCode { get; set; }
      public string Description { get; set; }
      public string ServerName { get; set; }
    }

    private readonly AtlantisException _exception;
    private readonly string _resultXML = string.Empty;

    public bool IsSuccess { get; private set; }
    public ErrorStruct Error { get; private set; }
    public string OrderXml { get; private set; }

    #endregion

    public ExpressCheckoutPurchaseResponseData(string xml, string orderXml)
    {
      _resultXML = xml;
      OrderXml = orderXml;
      IsSuccess = _resultXML.IndexOf("Success", StringComparison.OrdinalIgnoreCase) > -1;

      if (!IsSuccess)
      {
        XDocument xDoc = XDocument.Parse(xml);
        XElement root = xDoc.Element("Status");

        var err = new ErrorStruct
                    {
                      ErrorCode = root.Element("Error").Value,
                      Description = root.Element("Description").Value,
                      ServerName = root.Element("Server").Value
                    };
        Error = err;
      }
      else
      {
        int errorCount = 0;
        var sb = new StringBuilder();
        XDocument xDoc = XDocument.Parse(OrderXml);
        XElement root = xDoc.Element("ORDER");
        XElement basketErrors = root.Element("BASKETERRORS");
        XElement purchaseErrors = root.Element("PURCHASEERRORS");

        if (basketErrors.HasElements || purchaseErrors.HasElements)
        {
          IsSuccess = false;
          var err = new ErrorStruct {ErrorCode = "N/A", ServerName = "N/A"};

          if (basketErrors.HasElements)
          {
            sb.Append("BasketError: ");
            sb.Append(basketErrors.Element("ERROR").Attribute("description").Value);
            errorCount++;
          }
          if (purchaseErrors.HasElements)
          {
            if (errorCount > 0)
            {
              sb.Append(" | PurchaseError: ");
              sb.Append(purchaseErrors.Element("ERROR").Attribute("description").Value);
            }
            else
            {
              sb.Append("PurchaseError: ");
              sb.Append(purchaseErrors.Element("ERROR").Attribute("description").Value);
            }
          }
          err.Description = sb.ToString();
          Error = err;
        }
      }
    }

     public ExpressCheckoutPurchaseResponseData(AtlantisException atlantisException)
     {
       OrderXml = string.Empty;
       _exception = atlantisException;
     }

    public ExpressCheckoutPurchaseResponseData(RequestData requestData, Exception exception)
    {
      OrderXml = string.Empty;
      _exception = new AtlantisException(requestData,
                                         "ExpressCheckoutPurchaseResponseData",
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
