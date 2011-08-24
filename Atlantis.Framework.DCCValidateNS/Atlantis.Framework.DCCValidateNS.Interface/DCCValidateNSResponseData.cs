using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCValidateNS.Interface
{
  public class DCCValidateNSResponseData : IResponseData
  {
    public enum ValidationResults
    {
      INVALID_NAMESERVERS = 0,
      VALID_NAMESERVERS
    }


    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;

    private List<DCCValidateNameServerInfo> _nameServers = new List<DCCValidateNameServerInfo>();
    public int Results { get; set; }

    private int _callInternalResults = -1;
    private int _callInternalType = -1;
    private int _callErrorCode = -1;

    public List<DCCValidateNameServerInfo> NameServers
    {
      get
      {
        return _nameServers;
      }
    }

    public int CountOfValidNameServers
    {
      get
      {
        int validCount = 0;
        foreach (DCCValidateNameServerInfo current in _nameServers)
        {
          if (current.Valid)
          {
            validCount++;
          }
        }
        return validCount;
      }
    }

    public int ErrorCode
    {
      get
      {
        return _callErrorCode;
      }
    }
    public bool AllThirdPartyNameServers
    {
      get
      {
        return (_callInternalType == 0);
      }
    }
    
    public bool AllGodaddyNameServers
    {
      get
      {
        return (_callInternalType == 1);
      }
    }
    
    public bool MixedNameServers
    {
      get
      {
        return (_callInternalType == 2);
      }
    }

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public DCCValidateNSResponseData(List<DCCValidateNameServerInfo> nameServers,int result,int internalType,int errorcode)
    {
      _callInternalResults = result;
      _callInternalType = internalType;
      _callErrorCode = errorcode;
      _nameServers = nameServers;
      if (_callInternalResults == 1)
      {
        _success = true;
      }
    }

     public DCCValidateNSResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
       _success = false;
    }

     public DCCValidateNSResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "DCCValidateNSResponseData",
                                   exception.Message,
                                   requestData.ToXML());
      _success = false;
    }


    #region IResponseData Members

    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
