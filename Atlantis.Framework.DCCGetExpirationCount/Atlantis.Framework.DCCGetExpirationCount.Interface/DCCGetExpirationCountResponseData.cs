using System;
using Atlantis.Framework.Interface;
using System.Xml;
using System.Collections.Generic;

namespace Atlantis.Framework.DCCGetExpirationCount.Interface
{
  public class DCCGetExpirationCountResponseData : IResponseData
  {
    private AtlantisException _exception;
    private string _responseXml;
    
    private Dictionary<string, ExpirationDomainCountsResult> _shopperResultsDictionary =  new Dictionary<string,ExpirationDomainCountsResult>();
    private string _defaultShopperId = string.Empty;

    public DCCGetExpirationCountResponseData(string responseXml, RequestData requestData)
    {
      _responseXml = responseXml;
      _isSuccess = ParseResponse(requestData);
      _defaultShopperId = requestData.ShopperID;
    }

    private bool ParseResponse(RequestData requestData)
    {
      bool result = false;

      if (!string.IsNullOrEmpty(_responseXml))
      {
        try
        {
          XmlDocument resultDoc = new XmlDocument();
          resultDoc.LoadXml(_responseXml);

          XmlNode successNode = resultDoc.SelectSingleNode("//success");
          if (successNode != null)
          {
            string successValue = successNode.InnerText;
            if (successValue == "1")
            {
              XmlNodeList shopperNodes = resultDoc.SelectNodes("//shopper");
              foreach (XmlElement shopperElement in shopperNodes)
              {
                if (shopperElement != null)
                {
                  ExpirationDomainCountsResult resultItem = new ExpirationDomainCountsResult(shopperElement);
                  _shopperResultsDictionary[resultItem.ShopperId] = resultItem;
                }
              }
              result = true;
            }
            else // error
            {
              string message = null;
              string description = null;

              XmlNode errorMessageNode = resultDoc.SelectSingleNode("//error/message");
              if (errorMessageNode != null)
              {
                message = errorMessageNode.InnerText;
              }

              XmlNode errorDescriptionNode = resultDoc.SelectSingleNode("//error/description");
              if (errorDescriptionNode != null)
              {
                description = errorDescriptionNode.InnerText;
              }

              if (string.IsNullOrEmpty(message))
              {
                message = "No error message provided.";
              }

              if (string.IsNullOrEmpty(description))
              {
                description = "No error description provided.";
              }

              _exception = new AtlantisException(requestData, "DomainGetExpirationCounts.ParseResponse", message + Environment.NewLine + description, _responseXml);
            }
          }
          else
          {
            _exception = new AtlantisException(requestData, "DomainGetExpirationCounts.ParseResponse", "Xml result missing success node.", _responseXml);
          }

        }
        catch (Exception ex)
        {
          _exception = new AtlantisException(requestData, "DomainGetExpirationCounts.ParseResponse", ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
        }
      }

      return result;
    }

    private bool _isSuccess;
    public bool IsSuccess
    {
      get { return _isSuccess; }
    }

    public DCCGetExpirationCountResponseData(string responseXml, AtlantisException ex)
    {
      _responseXml = responseXml;
      _exception = ex;
      _isSuccess = false;
    }

    public DCCGetExpirationCountResponseData(string responseXml, RequestData requestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(requestData, "DomainGetExpirationCountResponseData", ex.Message + Environment.NewLine + ex.StackTrace, _responseXml);
      _isSuccess = false;
    }

    public ExpirationDomainCountsResult GetExpirationDomainCountResult()
    {
      ExpirationDomainCountsResult result = null;
      _shopperResultsDictionary.TryGetValue(_defaultShopperId, out result);
      return result;
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _responseXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
