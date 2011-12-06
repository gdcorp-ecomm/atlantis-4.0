using System;
using System.Data;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RenewalsMyRenewingDomains.Interface
{
  public class RenewalsMyRenewingDomainsResponseData : IResponseData
  {
    private DataSet _ds = null;
    private AtlantisException _atlException = null;

    public RenewalsMyRenewingDomainsResponseData(DataSet ds)
    {
      _ds = ds;
      _isSuccess = true;
    }

    public RenewalsMyRenewingDomainsResponseData(AtlantisException exAtlantis)
    {
      _atlException = exAtlantis;
    }

    public RenewalsMyRenewingDomainsResponseData(DataSet ds, RequestData oRequestData, Exception ex)
    {
      _ds = ds;
      _atlException = new AtlantisException(oRequestData, "RenewalsMyRenewingDomainsResponseData", ex.Message, string.Empty);
    }

    private bool _isSuccess = false;
    public bool IsSuccess
    {
      get
      {
        return _isSuccess;
      }
    }

    public DataSet RenewingDomainListSet
    {
      get
      {
        return _ds;
      }
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _atlException;
    }

    public string ToXML()
    {
      return string.Empty;
    }

    #endregion
  }
}
