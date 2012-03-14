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

    public List<string> Nameservers { get; set; }

    private Dictionary<string, string[]> _nameserversByTld = new Dictionary<string, string[]>();
    public Dictionary<string, string[]> NameserversByTld
    {
      get { return _nameserversByTld; }
    }

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public GetPremiumDNSDefaultNameServersResponseData(IEnumerable<string> defaultNameserver, 
      Dictionary<string, string[]> nameserversByTld)
    {
      try
      {
        Nameservers = defaultNameserver.ToList();
        _nameserversByTld = nameserversByTld;
        _success = true;
      } 
      catch
      {
        _success = false;
      }
    }

    public GetPremiumDNSDefaultNameServersResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public GetPremiumDNSDefaultNameServersResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,"GetPremiumDNSDefaultNameServersResponseData", exception.Message,requestData.ToXML());
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
