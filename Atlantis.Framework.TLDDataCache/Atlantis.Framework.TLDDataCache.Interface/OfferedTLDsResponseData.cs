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
  
      return new OfferedTLDsResponseData(tlds);
    }

    private OfferedTLDsResponseData()
    {
      _offeredTLDsInOrder = new List<string>();
    }

    private OfferedTLDsResponseData(List<string> tlds)
    {
      _offeredTLDsInOrder = tlds;
    }

    public IEnumerable<string> OfferedTLDs
    {
      get { return _offeredTLDsInOrder; }
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
