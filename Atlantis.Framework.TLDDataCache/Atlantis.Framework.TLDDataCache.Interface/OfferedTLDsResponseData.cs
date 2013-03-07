using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class OfferedTLDsResponseData : IResponseData
  {
    public static OfferedTLDsResponseData Empty {get; private set;}

    static OfferedTLDsResponseData()
    {
      Empty = new OfferedTLDsResponseData();
    }

    private AtlantisException _exception;
    private HashSet<string> _offeredTLDs;
    private List<string> _offeredTLDsInOrder;

    public static OfferedTLDsResponseData FromException(AtlantisException exception)
    {
      return new OfferedTLDsResponseData(exception);
    }

    private OfferedTLDsResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public static OfferedTLDsResponseData FromCacheXml(string cacheXml)
    {
      List<string> tlds = new List<string>();

      XElement tldList = XElement.Parse(cacheXml);
      foreach (var tldItem in tldList.Descendants("tld"))
      {
        string name = tldItem.Attribute("name").Value;
        string availCheckStatus = tldItem.Attribute("availcheckstatus").Value;
        if ("1".Equals(availCheckStatus))
        {
          tlds.Add(name);
        }
      }

      if (tlds.Count == 0)
      {
        return Empty;
      }
      else
      {
        return new OfferedTLDsResponseData(tlds);
      }
    }

    public static OfferedTLDsResponseData FromDataTable(DataTable dataCacheTable)
    {
      return new OfferedTLDsResponseData(dataCacheTable);
    }

    private OfferedTLDsResponseData()
    {
      _offeredTLDs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
      _offeredTLDsInOrder = new List<string>();
    }

    private OfferedTLDsResponseData(List<string> tlds)
    {
      _offeredTLDsInOrder = tlds;
      _offeredTLDs = new HashSet<string>(tlds, StringComparer.OrdinalIgnoreCase);
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
