using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCSetAutoRenew.Interface
{
  public class DCCSetAutoRenewResponseData : IResponseData
  {
    string _responseXml;
    AtlantisException _exception;
    bool _isSuccess;

    private string _validationMsg;
    public string ValidationMsg
    {
      get { return _validationMsg; }
    }

    public DCCSetAutoRenewResponseData(string responseXml)
    {
      _validationMsg = "";
      _responseXml = responseXml;
      PopulateFromXML(responseXml);
    }

    public DCCSetAutoRenewResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
    }

    public DCCSetAutoRenewResponseData(string validationXml, bool isSuccess)
    {
      _responseXml = validationXml;
      _isSuccess = isSuccess;
      _validationMsg = ParseVerificationDesc(validationXml);
    }


    public DCCSetAutoRenewResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(oRequestData,
                                   "DCCSetAutoRenewResponseData",
                                   ex.Message,
                                   ex.StackTrace);
    }

    string ParseVerificationDesc(string verificaitonDoc)
    {
      XmlDocument oDoc = new XmlDocument();
      oDoc.LoadXml(verificaitonDoc);

      XmlElement oEle = (XmlElement)oDoc.SelectSingleNode("/VERIFICATION/ACTIONRESULTS/ACTIONRESULT");
      string sResult = string.Empty;
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
