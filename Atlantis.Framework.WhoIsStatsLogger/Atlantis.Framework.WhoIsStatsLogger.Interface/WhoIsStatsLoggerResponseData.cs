using System;
using System.Data;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.WhoIsStatsLogger.Interface
{
  public class WhoIsStatsLoggerResponseData : IResponseData
  {
    private AtlantisException _exAtlantis = null;

    public bool IsSuccess { get; set; }
    public int StatusLoggerID { get; set; }

    public WhoIsStatsLoggerResponseData(int newID)
    {
      IsSuccess = true;
      StatusLoggerID = newID;
    }

    public WhoIsStatsLoggerResponseData(AtlantisException exAtlantis)
    {
      IsSuccess = false;
      _exAtlantis = exAtlantis;
    }

    public WhoIsStatsLoggerResponseData(RequestData oRequestData, Exception ex)
    {
      IsSuccess = false;
      _exAtlantis = new AtlantisException(oRequestData, "WhoIsStatsLoggerResponseData", ex.Message, string.Empty);
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
