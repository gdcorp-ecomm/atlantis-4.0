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
    private List<string> _offeredTLDsInOrder;

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
      _offeredTLDsInOrder = new List<string>();

      if (dataCacheTable != null && dataCacheTable.Rows.Count > 0)
      {
        DataColumn columnName = dataCacheTable.Columns["name"];
        DataColumn columnAvailCheckStatus = dataCacheTable.Columns["availcheckstatus"];

        foreach (DataRow row in dataCacheTable.Rows)
        {
          bool availCheckStatus = (Convert.ToByte(row[columnAvailCheckStatus]) == 1);
          if (availCheckStatus)
          {
            string tld = row[columnName].ToString().ToUpperInvariant();
            _offeredTLDs.Add(tld);
            _offeredTLDsInOrder.Add(tld);
          }
        }

        AddOverrideTlds();
      }
    }

    private void AddOverrideTlds()
    {
      var overrideTlds = TLDsHelper.OverrideTlds;
      foreach (var overrideTld in overrideTlds)
      {
        if (!_offeredTLDs.Contains(overrideTld))
        {
          var tld = overrideTld.ToUpperInvariant();
          _offeredTLDs.Add(tld);
          _offeredTLDsInOrder.Add(tld);
        }
      }
    }

    public IEnumerable<string> OfferedTLDs
    {
      get { return _offeredTLDsInOrder; }
    }

    public bool IsTLDOffered(string tld)
    {
      return _offeredTLDs.Contains(tld);
    }

    public Dictionary<string, int> GetSortOrder()
    {
      Dictionary<string, int> result = new Dictionary<string, int>(_offeredTLDsInOrder.Count, StringComparer.OrdinalIgnoreCase);
      int sortOrder = 0;
      foreach (string tld in _offeredTLDsInOrder)
      {
        result[tld] = (++sortOrder);
      }
      return result;
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
