using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCSetLocking.Interface
{
  public class DCCSetLockingResponseData: IResponseData
  {
    private string _responseXml;
    private AtlantisException _exception;
    private bool _isSuccess;

    private string _validationMsg;
    public string ValidationMsg
    {
      get { return _validationMsg; }
    }

    public DCCSetLockingResponseData(string responseXml)
    {
      _responseXml = responseXml;
      PopulateFromXml(responseXml);      
    }

    public DCCSetLockingResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;      
    }

    public DCCSetLockingResponseData(string validationXml, bool isSuccess)
    {
      _responseXml = validationXml;
      _isSuccess = isSuccess;
      _validationMsg = ParseVerificationDesc(validationXml);
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

    public DCCSetLockingResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(oRequestData,
                                   "DCCSetLockingResponseData",
                                   ex.Message,
                                   ex.StackTrace);
    }

    private void PopulateFromXml(string resultXml)
    {
      if (resultXml.Contains("<success") )
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
