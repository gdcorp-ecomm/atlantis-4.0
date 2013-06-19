using System.Linq;
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

      var overrideTlds = TLDsHelper.OverrideTlds();
      if (overrideTlds.Count > 0)
      {
        foreach (var item in overrideTlds)
        {
          if (!tlds.Contains(item, StringComparer.OrdinalIgnoreCase))
          {
            tlds.Add(item);
          }
        }
      }

      if (tlds.Count == 0)
      {
        return Empty;
      }
  
      return new OfferedTLDsResponseData(tlds);
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
      var rootElement = new XElement("OfferedTLDsData");

      if (_offeredTLDsInOrder != null)
      {
        foreach (var tld in _offeredTLDsInOrder)
        {
          var tldElement = new XElement("tld");
          tldElement.Add(new XAttribute("name", tld));

          rootElement.Add(tldElement);
        }
      }
      return rootElement.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
