using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetShopper.Interface
{
  public class GetShopperResponseData : IResponseData
  {
    string _responseXml = null;
    AtlantisException _exception;

    public GetShopperResponseData(string responseXml)
    {
      _responseXml = responseXml;
      _exception = null;
    }

    public GetShopperResponseData(string responseXml, AtlantisException ex)
    {
      _responseXml = responseXml;
      _exception = ex;
    }

    public GetShopperResponseData(string responseXml, RequestData requestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(requestData,
                                   "GetShopperResponseData", 
                                   ex.Message.ToString(), 
                                   requestData.ToXML());
    }

    XmlDocument _responseDoc = null;    
    XmlDocument ShopperXmlDoc
    {
      get
      {
        if (_responseDoc == null)
        {
          _responseDoc = new XmlDocument();
          _responseDoc.LoadXml(_responseXml);
        }
        return _responseDoc;
      }
    }

    public string GetField(string fieldName)
    {
      string result = string.Empty;
      string sXPath = String.Format("/Shopper/Fields/Field[@Name='{0}']", fieldName);
      XmlNode xnField = ShopperXmlDoc.SelectSingleNode(sXPath);

      if (xnField != null)
        result = xnField.InnerText;

      return result;
    }

    public long GetInterestPref(long lCommTypeID, long lInterestTypeID)
    {
      long result = 0;
      string sXPath = String.Format("/Shopper/Preferences/Interest[@CommTypeID='{0}' and @InterestTypeID='{1}']",
                                    lCommTypeID,
                                    lInterestTypeID);
      XmlElement xlInterest = ShopperXmlDoc.SelectSingleNode(sXPath) as XmlElement;

      if (xlInterest != null)
        result = XmlConvert.ToInt32(xlInterest.GetAttribute("OptIn"));

      return result;
    }

    public long GetCommunicationPref(long lCommTypeID)
    {
      long result = 0;
      string sXPath = String.Format("/Shopper/Preferences/Communication[@CommTypeID='{0}']",
                                    lCommTypeID);
      XmlElement xlComm = ShopperXmlDoc.SelectSingleNode(sXPath) as XmlElement;

      if (xlComm != null)
        result = XmlConvert.ToInt32(xlComm.GetAttribute("OptIn"));

      return result;
    }

    public bool IsSuccess
    {
      get { return _responseXml.IndexOf("success", StringComparison.OrdinalIgnoreCase) > -1; }
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
