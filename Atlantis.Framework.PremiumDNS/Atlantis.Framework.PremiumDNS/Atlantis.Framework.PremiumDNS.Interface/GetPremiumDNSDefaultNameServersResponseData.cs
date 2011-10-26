using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PremiumDNS.Interface
{
  public class GetPremiumDNSDefaultNameServersResponseData : IResponseData 
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public GetPremiumDNSDefaultNameServersResponseData(IEnumerable<string> results)
    {
      try
      {
        Nameservers = results.ToList();
        _success = true;
      } catch
      {
        _success = false;
      }
    }

    public List<string> Nameservers { get; set; }

    public GetPremiumDNSDefaultNameServersResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
      _success = false;
    }

    public GetPremiumDNSDefaultNameServersResponseData(RequestData requestData, Exception exception)
    {
      _success = false;
      this._exception = new AtlantisException(requestData,
                                   "GetPremiumDNSDefaultNameServersResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    #region Implementation of IResponseData

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
