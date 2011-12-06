using System;
using System.Data;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RenewalsGetServiceRecords.Interface
{
  public class RenewalsGetServiceRecordsResponseData : IResponseData
  {
    private DataSet _ds = null;
    private AtlantisException _atlException = null;

    public RenewalsGetServiceRecordsResponseData(DataSet ds)
    {
      _ds = ds;
      _isSuccess = true;
    }

    public RenewalsGetServiceRecordsResponseData(AtlantisException exAtlantis)
    {
      _atlException = exAtlantis;
    }

    public RenewalsGetServiceRecordsResponseData(DataSet ds, RequestData oRequestData, Exception ex)
    {
      _ds = ds;
      _atlException = new AtlantisException(oRequestData, "RenewalsGetServiceRecordsResponseData", ex.Message, string.Empty);
    }

    private bool _isSuccess = false;
    public bool IsSuccess
    {
      get
      {
        return _isSuccess;
      }
    }

    public DataSet RenewingServiceRecordsSet
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
