using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCSetContacts.Interface
{
  public class DCCSetContactsResponseData : IResponseData
  {
    string _responseXml;
    string _validationMsg = "";
    AtlantisException _exception;
    bool _isSuccess;

    public DCCSetContactsResponseData(string responseXml)
    {
      _responseXml = responseXml;
      PopulateFromXML(responseXml);
    }

    public DCCSetContactsResponseData(string validationXml, bool isSuccess)
    {
      _responseXml = validationXml;
      _isSuccess = isSuccess;
      _validationMsg = ParseValidationDesc(validationXml);
    }

    public DCCSetContactsResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
    }

    public DCCSetContactsResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(oRequestData,
                                         "DCCSetContactsResponseData",
                                         ex.Message,
                                         ex.StackTrace);
    }


    public DCCSetContactsResponseData(string responseXml, RequestData oRequestData)
    {
      _validationMsg = ParseVerificationDesc(responseXml);
      _responseXml = responseXml;      
    }


    private string ParseVerificationDesc(string xml)
    {
      string sResult = string.Empty;
      XmlDocument oDoc = new XmlDocument();
      oDoc.LoadXml(xml);

      XmlElement oEle = (XmlElement)oDoc.SelectSingleNode("/VERIFICATION/ACTIONRESULTS/ACTIONRESULT");
      if (oEle != null && oEle.Attributes["Description"] != null)
      {
        sResult = oEle.Attributes["Description"].Value;
      }
      return sResult;
    }

    string ParseValidationDesc(string validationDoc)
    {
      string sResult = string.Empty;
      XmlDocument oDoc = new XmlDocument();
      oDoc.LoadXml(validationDoc);

      XmlElement oEle = (XmlElement)oDoc.SelectSingleNode("/VALIDATION/ACTIONRESULTS/ACTIONRESULT");
      if (oEle != null && oEle.Attributes["Description"] != null)
      {
        sResult = oEle.Attributes["Description"].Value;
      }
      return sResult;
    }

    void PopulateFromXML(string resultXML)
    {
      if (resultXML.Contains("<success"))
      {
        _isSuccess = true;
      }
    }

    public bool IsSuccess
    {
      get { return (_exception == null && _isSuccess); }
    }

    public string ValidationMsg
    {
      get { return _validationMsg; }
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return _responseXml;
    }
  }
}
