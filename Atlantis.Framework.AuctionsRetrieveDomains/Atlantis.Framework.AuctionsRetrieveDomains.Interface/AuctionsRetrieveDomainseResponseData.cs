using System;
using System.Data;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionsRetrieveDomains.Interface
{
  public class AuctionsRetrieveDomainsResponseData : IResponseData
  {
    private DataSet _ds = null;
    private AtlantisException _exAtlantis = null;
    
    private bool _success = false;
    public bool IsSuccess { get { return _success; } }

    public AuctionsRetrieveDomainsResponseData(DataSet ds)
    {
      _ds = ds;
      _success = true;
    }

    public AuctionsRetrieveDomainsResponseData(AtlantisException exAtlantis)
    {
      _exAtlantis = exAtlantis;
    }

    public AuctionsRetrieveDomainsResponseData(DataSet ds, RequestData oRequestData, Exception ex)
    {
      _ds = ds;
      _exAtlantis = new AtlantisException(oRequestData, "AuctionsRetrieveDomainsResponseData", ex.Message, string.Empty);
    }

    public DataSet AuctionsList
    {
      get
      {
        return _ds;
      }
    }    

    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exAtlantis;
    }

    #endregion
  }
}
