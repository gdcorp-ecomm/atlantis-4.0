using System;
using System.Data;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.WhoIsStatsDetail.Interface
{
  public class WhoIsStatsDetailResponseData : IResponseData
  {
    private AtlantisException _exAtlantis = null;

    public bool IsSuccess { get; set; }

    public WhoIsStatsDetailResponseData(DataTable ds)
    {
      IsSuccess = true;
    }

    public WhoIsStatsDetailResponseData(AtlantisException exAtlantis)
    {
      IsSuccess = false;
      _exAtlantis = exAtlantis;
    }

    public WhoIsStatsDetailResponseData(DataTable ds, RequestData oRequestData, Exception ex)
    {
      IsSuccess = false;
      _exAtlantis = new AtlantisException(oRequestData, "WhoIsStatsDetailResponseData", ex.Message, string.Empty);
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

