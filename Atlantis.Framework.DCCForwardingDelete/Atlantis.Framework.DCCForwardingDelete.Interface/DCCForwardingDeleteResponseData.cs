using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCForwardingDelete.Interface
{
  public class DCCForwardingDeleteResponseData : IResponseData
  {
    string _responseXml;
    AtlantisException _exception;
    bool _isSuccess = false;
    public string ValidationMsg { get; private set;}

    public DCCForwardingDeleteResponseData(string responseXML)
    {
      _responseXml = responseXML;
      PopulateFromXML(responseXML);
    }

    public DCCForwardingDeleteResponseData(string responseXML, AtlantisException exAtlantis)
    {
      _responseXml = responseXML;
      _exception = exAtlantis;
    }

    public DCCForwardingDeleteResponseData(string responseXML, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXML;
      _exception = new AtlantisException(oRequestData,
                                   "DCCForwardingDeleteResponseData",
                                   ex.Message,
                                   oRequestData.ToXML());
    }

     public DCCForwardingDeleteResponseData(string responseXML, RequestData oRequestData)
    {
      ValidationMsg = parseVerificationDesc(responseXML);
      _responseXml = responseXML;      
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
