using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCCreateDNS.Interface
{
  public class DCCCreateDNSResponseData  : IResponseData
  {
    private string _responseXml;
    private AtlantisException _exception;

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

    public DCCCreateDNSResponseData(bool result)
    {
      _isSuccess = result;
    }

    public DCCCreateDNSResponseData(List<string> errorList)
    {
      _errorList = errorList;
    }

    public DCCCreateDNSResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
    }

    public DCCCreateDNSResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(oRequestData,
                                   "DCCCreateDNSResponseData", 
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
