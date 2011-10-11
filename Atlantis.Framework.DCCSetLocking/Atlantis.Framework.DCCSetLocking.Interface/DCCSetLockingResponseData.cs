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
      _validationMsg = parseVerificationDesc(validationXml);
    }

    string parseVerificationDesc(string verificaitonDoc)
    {
      XmlDocument oDoc = new XmlDocument();
      oDoc.LoadXml(verificaitonDoc);

      XmlElement oEle = (XmlElement)oDoc.SelectSingleNode("/VERIFICATION/ACTIONRESULTS/ACTIONRESULT");
      return (oEle != null) ? oEle.Attributes["Description"].Value : "";
    }

    public DCCSetLockingResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(oRequestData,
                                   "DCCSetLockingResponseData",
                                   ex.Message,
                                   oRequestData.ToXML());
    }

    private void PopulateFromXml(string resultXml)
    {
      //ResultID52 == "Already in specified Status"
      if (resultXml.Contains("<success") )
      {
        _isSuccess = true;
      }
        /*
      else if (resultXML.Contains("<error"))
      {
        XmlDocument responseDoc = new XmlDocument();
        responseDoc.LoadXml(resultXML);
        responseDoc.Attributes["desc"].Value

      }
         * */
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
