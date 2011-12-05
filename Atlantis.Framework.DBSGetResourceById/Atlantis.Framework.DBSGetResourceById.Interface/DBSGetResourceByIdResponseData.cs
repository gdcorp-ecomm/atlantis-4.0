using System;
using System.Data;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DBSGetResourceById.Interface
{
  public class DBSGetResourceByIdResponseData : IResponseData
  {
    private DataTable _ds = null;
    private AtlantisException _exAtlantis = null;
    
    private bool _success = false;
    public bool IsSuccess { get { return _success; } }

    public DBSGetResourceByIdResponseData(DataTable  ds)
    {
      _ds = ds;
      _success = true;
    }

    public DBSGetResourceByIdResponseData(AtlantisException exAtlantis)
    {
      _exAtlantis = exAtlantis;
    }

    public DBSGetResourceByIdResponseData(DataTable  ds, RequestData oRequestData, Exception ex)
    {
      _ds = ds;
      _exAtlantis = new AtlantisException(oRequestData, "DBSGetResourceByIdResponseData", ex.Message, string.Empty);
    }

    public DataTable ResultTable
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
