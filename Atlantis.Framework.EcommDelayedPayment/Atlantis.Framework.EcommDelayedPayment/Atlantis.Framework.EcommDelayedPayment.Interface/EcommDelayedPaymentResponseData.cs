using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommDelayedPayment.Interface
{
  public class EcommDelayedPaymentResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;

    public string RedirectURL { get; set; }
    public string ErrorOccured { get; set; }
    public string InvoiceID { get; set; }

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public EcommDelayedPaymentResponseData(RequestData requestData,string redirectXML,string errorXML,string invoiceID)
    {
      try
      {
        ErrorOccured = string.Empty;
        RedirectURL = string.Empty;
        InvoiceID = string.Empty;
        if (!string.IsNullOrEmpty(redirectXML))
        {
          XmlDocument redirectDoc = new XmlDocument();
          redirectDoc.LoadXml(redirectXML);
          XmlNode redirectURL = redirectDoc.SelectSingleNode("//Redirect/URL");
          RedirectURL = redirectURL.InnerText;
        }
        else if (!string.IsNullOrEmpty(errorXML))
        {
          XmlDocument errorsDoc = new XmlDocument();
          errorsDoc.LoadXml(errorXML);
          XmlNode errorNode = errorsDoc.SelectSingleNode("//ERRORS/ERROR");
          ErrorOccured = errorNode.InnerText;
        }
        if (!string.IsNullOrEmpty(invoiceID))
        {
          InvoiceID = invoiceID;
        }
        _success = true;
      }
      catch (System.Exception ex)
      {
        this._exception = new AtlantisException(requestData,
                                    "EcommDelayedPaymentResponseData",
                                    ex.Message,
                                    requestData.ToXML());
        _success = false;
      }
    }

     public EcommDelayedPaymentResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public EcommDelayedPaymentResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "EcommDelayedPaymentResponseData",
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
