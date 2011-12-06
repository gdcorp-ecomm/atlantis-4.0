using System;
using System.Data;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ResellerTopAcctByDate.Interface
{
  public class ResellerTopAcctByDateResponseData : IResponseData
  {
    private DataTable _ds = null;
    private AtlantisException _exAtlantis = null;

    private bool _success = false;
    public bool IsSuccess { get { return _success; } }

    public ResellerTopAcctByDateResponseData(DataTable ds)
    {
      _ds = ds;
      _success = true;
    }

    public ResellerTopAcctByDateResponseData(AtlantisException exAtlantis)
    {
      _exAtlantis = exAtlantis;
    }

    public ResellerTopAcctByDateResponseData(DataTable ds, RequestData oRequestData, Exception ex)
    {
      _ds = ds;
      _exAtlantis = new AtlantisException(oRequestData, "ResellerTopAcctByDateResponseData", ex.Message, string.Empty);
    }

    public DataTable ResellerList
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
