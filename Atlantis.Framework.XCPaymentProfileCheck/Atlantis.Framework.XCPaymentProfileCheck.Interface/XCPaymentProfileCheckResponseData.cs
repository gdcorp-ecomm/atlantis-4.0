using System;
using System.Xml.Linq;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.XCPaymentProfileCheck.Interface
{
  public class XCPaymentProfileCheckResponseData : IResponseData
  {
    public struct ErrorStruct
    {
      public string ErrorCode {get; set;}
      public string Description { get; set; }
      public string ServerName { get; set; }
    }

    private readonly AtlantisException _exception;
    private readonly string _resultXML = string.Empty;

    public bool IsSuccess { get; private set; }
    public bool HasInstantPurchasePayment { get; private set; }
    public ErrorStruct Error { get; private set; }

    public XCPaymentProfileCheckResponseData(string xml, bool hasInstantPurchasePayment)
    {
      _resultXML = xml;
      HasInstantPurchasePayment = hasInstantPurchasePayment;

      var xDoc = XDocument.Parse(xml);
      XElement root = xDoc.Element("Status");

      if (root != null && root.HasElements)
      {
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
        IsSuccess = true;
      }
    }

     public XCPaymentProfileCheckResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public XCPaymentProfileCheckResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                         "XCPaymentProfileCheckResponseData",
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
