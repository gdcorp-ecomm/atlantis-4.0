using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCDeleteDNS.Interface
{
  public class DCCDeleteDNSResponseData : IResponseData
  {
    string _responseXml;
    AtlantisException _exception;

    private bool _isSuccess;
    public bool IsSuccess
    {
      get { return (_exception == null && _isSuccess); }
    }

    private List<string> _errorList = new List<string>();
    public List<string> ErrorList
    {
      get { return _errorList; }
    }

    public DCCDeleteDNSResponseData(bool result)
    {
      _isSuccess = result;
    }

    public DCCDeleteDNSResponseData(List<string> errorList)
    {
      _errorList = errorList;
    }

    public DCCDeleteDNSResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
    }

    public DCCDeleteDNSResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(oRequestData,
                                   "DCCDeleteDNSResponseData", 
                                   ex.Message, 
                                   oRequestData.ToXML());
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
