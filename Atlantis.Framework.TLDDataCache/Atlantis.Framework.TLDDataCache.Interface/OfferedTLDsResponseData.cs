using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Data;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class OfferedTLDsResponseData : IResponseData
  {
    private AtlantisException _exception;

    private HashSet<string> _offeredTLDs;
    private Dictionary<string, int> _tldOrder;

    public static OfferedTLDsResponseData FromException(RequestData requestData, Exception ex)
    {
      return new OfferedTLDsResponseData(requestData, ex);
    }

    private OfferedTLDsResponseData(RequestData requestData, Exception ex)
    {
      string message = ex.Message + ex.StackTrace;
      string inputData = requestData.ToXML();
      _exception = new AtlantisException(requestData, "OfferedTLDsResponseData.ctor", message, inputData);
    }

    public static OfferedTLDsResponseData FromDataTable(DataTable dataCacheTable)
    {
      return new OfferedTLDsResponseData(dataCacheTable);
    }

    private OfferedTLDsResponseData(DataTable dataCacheTable)
    {
      _offeredTLDs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
      _tldOrder = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

      DataColumn columnName = dataCacheTable.Columns["name"];
      DataColumn columnAvailCheckStatus = dataCacheTable.Columns["availcheckstatus"];

      int sortOrder = 0;

      foreach (DataRow row in dataCacheTable.Rows)
      {
        bool availCheckStatus = (Convert.ToByte(row[columnAvailCheckStatus]) == 1);
        if (availCheckStatus)
        {
          string tld = row[columnName].ToString().ToUpperInvariant();
          _offeredTLDs.Add(tld);

          _tldOrder[tld] = (++sortOrder);
        }
      }
    }

    public IEnumerable<string> OfferedTLDs
    {
      get { return _offeredTLDs; }
    }

    public bool IsTLDOffered(string tld)
    {
      return _offeredTLDs.Contains(tld);
    }

    public Dictionary<string, int> GetSortOrder()
    {
      return new Dictionary<string, int>(_tldOrder, StringComparer.OrdinalIgnoreCase);
    }

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
