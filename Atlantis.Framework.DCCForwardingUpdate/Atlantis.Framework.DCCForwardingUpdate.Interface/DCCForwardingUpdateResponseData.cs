using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCForwardingUpdate.Interface
{
  public class DCCForwardingUpdateResponseData : IResponseData
  {
    AtlantisException _exception;
    
    private bool _isSuccess;
    public bool IsSuccess
    {
      get { return (_exception == null && _isSuccess); }
    }

    public string ResponseXml { get; private set; }

    public string ValidationMsg { get; private set;}

    public DCCForwardingUpdateResponseData(string responseXML)
    {
      ResponseXml = responseXML;
      PopulateFromXML(responseXML);
    }

    public DCCForwardingUpdateResponseData(string validationXML, bool isSuccess)
    {
      ResponseXml = validationXML;
      _isSuccess = isSuccess;
      ValidationMsg = ParseValidationDesc(validationXML);
    }

    public DCCForwardingUpdateResponseData(string responseXML, AtlantisException exAtlantis)
    {
      ResponseXml = responseXML;
      _exception = exAtlantis;
    }

    public DCCForwardingUpdateResponseData(string responseXML, RequestData oRequestData, Exception ex)
    {
      ResponseXml = responseXML;
      _exception = new AtlantisException(oRequestData,
                                   "DCCForwardingUpdateResponseData",
                                   ex.Message,
                                   oRequestData.ToXML());
    }

    public DCCForwardingUpdateResponseData(string responseXML, RequestData oRequestData)
    {
      ValidationMsg = parseVerificationDesc(responseXML);
      ResponseXml = responseXML;      
    }

    void PopulateFromXML(string resultXML)
    {
      if (resultXML.Contains("<success"))
      {
        _isSuccess = true;
      }
    }

    string parseVerificationDesc(string inXML)
    {
      string sResult = "";
      XmlDocument oDoc = new XmlDocument();
      oDoc.LoadXml(inXML);

      XmlElement oEle = (XmlElement)oDoc.SelectSingleNode("/VERIFICATION/ACTIONRESULTS/ACTIONRESULT");
      sResult = oEle.Attributes["Description"].Value;
      return sResult;
    }

    private static string ParseValidationDesc(string validationDoc)
    {
      string sResult;
      XmlDocument oDoc = new XmlDocument();
      oDoc.LoadXml(validationDoc);

      XmlElement oEle = (XmlElement)oDoc.SelectSingleNode("/VALIDATION/ACTIONRESULTS/ACTIONRESULT");
      sResult = oEle.Attributes["Description"].Value;
      return sResult;
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return ResponseXml;
    }

    #endregion
  }
}
