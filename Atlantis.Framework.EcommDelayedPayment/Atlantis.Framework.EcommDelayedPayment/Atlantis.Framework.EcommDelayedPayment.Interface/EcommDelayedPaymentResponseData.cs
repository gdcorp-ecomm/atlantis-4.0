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
    private Dictionary<string, string> _formValues = new Dictionary<string, string>();

    public string RedirectURL { get; set; }
    public string ErrorOccured { get; set; }
    public string InvoiceID { get; set; }
    public string RedirectAction { get; set; }

    public Dictionary<string, string> FormValues
    {
      get
      {
        return _formValues;
      }
    }

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
        _success = true;
        _resultXML = redirectXML;
        if (!string.IsNullOrEmpty(redirectXML))
        {
          XmlDocument redirectDoc = new XmlDocument();
          redirectDoc.LoadXml(redirectXML);
          XmlNode redirectURL = redirectDoc.SelectSingleNode("//Redirect/URL");
          XmlNode formNode = redirectDoc.SelectSingleNode("//Redirect/Form");
          XmlNode redirectTypeNod = redirectDoc.SelectSingleNode("//Redirect");
          PopulateFormValues(formNode);
          RedirectURL = redirectURL.InnerText;
          RedirectAction = "POST";
          foreach(XmlAttribute currentAttribute in redirectTypeNod.Attributes)
          {
            if (currentAttribute.Name=="action")
            {
              switch(currentAttribute.Value)
              {
                case "get":
                  RedirectAction="GET";
                  break;
              }
            }
          }
        }
        else if (!string.IsNullOrEmpty(errorXML))
        {
          XmlDocument errorsDoc = new XmlDocument();
          errorsDoc.LoadXml(errorXML);
          XmlNode errorNode = errorsDoc.SelectSingleNode("//ERRORS/ERROR");
          ErrorOccured = errorNode.InnerText;
          _success = false;
        }
        if (!string.IsNullOrEmpty(invoiceID))
        {
          InvoiceID = invoiceID;
        }
        else
        {
          _success = false;
        }
        
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

    private void PopulateFormValues(XmlNode formNode)
    {
      if (formNode != null)
      {
        foreach (XmlNode fieldValue in formNode.ChildNodes)
        {
          _formValues[fieldValue.Attributes["name"].Value]= fieldValue.InnerText;
        }
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
