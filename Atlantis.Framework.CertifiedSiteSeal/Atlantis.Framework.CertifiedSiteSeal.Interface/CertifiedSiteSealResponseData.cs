using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.CertifiedSiteSeal.Interface
{
  public class CertifiedSiteSealResponseData : IResponseData
  {
    private string _accreditationSeal = string.Empty;
    private AtlantisException _ex;
    public bool _isSuccess;

    public string AccreditationSeal
    {
      get
      {
        return this._accreditationSeal;
      }
    }

    public CertifiedSiteSealResponseData(string accreditationSeal)
    {
      if (!string.IsNullOrEmpty(accreditationSeal))
        this._accreditationSeal = accreditationSeal;
      this._isSuccess = true;
      this._ex = (AtlantisException)null;
    }

    public CertifiedSiteSealResponseData(RequestData oRequestData, Exception ex)
    {
      this._ex = new AtlantisException(oRequestData, "CertifiedDomainsResponseData", ((object)ex.Message).ToString(), oRequestData.ToXML());
    }

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return this._ex;
    }
  }
}
