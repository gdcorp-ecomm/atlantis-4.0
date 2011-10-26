using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCSetNameservers.Interface
{
  public class DCCSetNameserversResponseData : IResponseData
  {
    private bool _isSuccess;
    public bool IsSuccess
    {
      get { return (_exception == null && _isSuccess); }
    }

    private string _validationMsg = string.Empty;
    public string ValidationMsg
    {
      get { return _validationMsg; }
    }

    public DCCSetNameserversResponseData(string responseXML)
    {
      _responseXml = responseXML;
      PopulateFromXML(responseXML);
    }

    public DCCSetNameserversResponseData(string validationXML, bool isSuccess)
    {
      _responseXml = validationXML;
      _isSuccess = isSuccess;
      _validationMsg = ParseValidationDesc(validationXML);
    }

    public DCCSetNameserversResponseData(string responseXML, AtlantisException exAtlantis)
    {
      _responseXml = responseXML;
      _exception = exAtlantis;
    }

    public DCCSetNameserversResponseData(string responseXML, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXML;
      _exception = new AtlantisException(oRequestData,
                                   "DCCSetNameserversResponseData",
                                   ex.Message,
                                   oRequestData.ToXML());
    }

    private static string ParseValidationDesc(string validationDoc)
    {
      XmlDocument oDoc = new XmlDocument();
      oDoc.LoadXml(validationDoc);

      XmlElement oEle = (XmlElement)oDoc.SelectSingleNode("/VALIDATION/ACTIONRESULTS/ACTIONRESULT");
      string sResult = oEle.Attributes["Description"].Value;
      return sResult;
    }

    private void PopulateFromXML(string resultXML)
    {
      if (resultXML.Contains("<success"))
      {
        _isSuccess = true;
      }
    }

    private AtlantisException _exception;
    public AtlantisException GetException()
    {
      return _exception;
    }

    private string _responseXml;
    public string ToXML()
    {
      return _responseXml;
    }
  }
}
