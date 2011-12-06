using System;
using System.Data;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ResellerCalculator.Interface
{
  public class ResellerCalculatorResponseData : IResponseData
  {
    private DataTable _ds = null;
    private AtlantisException _exAtlantis = null;

    private bool _success = false;
    public bool IsSuccess { get { return _success; } }

    public ResellerCalculatorResponseData(DataTable ds)
    {
      _ds = ds;
      _success = true;
    }

    public ResellerCalculatorResponseData(AtlantisException exAtlantis)
    {
      _exAtlantis = exAtlantis;
    }

    public ResellerCalculatorResponseData(DataTable ds, RequestData oRequestData, Exception ex)
    {
      _ds = ds;
      _exAtlantis = new AtlantisException(oRequestData, "ResellerCalculatorResponseData", ex.Message, string.Empty);
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
