using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCSetAutoRenew.Interface
{
  public class DCCSetAutoRenewResponseData : IResponseData
  {
    string _responseXml;
    AtlantisException _exception;
    bool _isSuccess = false;

    private string _validationMsg;
    public string ValidationMsg
    {
      get { return _validationMsg; }
    }

    public DCCSetAutoRenewResponseData(string responseXML)
    {
      _validationMsg = "";
      _responseXml = responseXML;
      PopulateFromXML(responseXML);
    }

    public DCCSetAutoRenewResponseData(string responseXML, AtlantisException exAtlantis)
    {
      _responseXml = responseXML;
      _exception = exAtlantis;
    }

    public DCCSetAutoRenewResponseData(string validationXML, bool isSuccess)
    {
      _responseXml = validationXML;
      _isSuccess = isSuccess;
      _validationMsg = parseVerificationDesc(validationXML);
    }


    public DCCSetAutoRenewResponseData(string responseXML, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXML;
      _exception = new AtlantisException(oRequestData,
                                   "DCCSetAutoRenewResponseData",
                                   ex.Message,
                                   oRequestData.ToXML());
    }

    string parseVerificationDesc(string verificaitonDoc)
    {
      XmlDocument oDoc = new XmlDocument();
      oDoc.LoadXml(verificaitonDoc);

      XmlElement oEle = (XmlElement)oDoc.SelectSingleNode("/VERIFICATION/ACTIONRESULTS/ACTIONRESULT");
      return (oEle != null) ? oEle.Attributes["Description"].Value : "";
    }

    void PopulateFromXML(string resultXML)
    {
      if (resultXML.Contains("<success"))
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

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return _responseXml;
    }

    #endregion
  }
}
