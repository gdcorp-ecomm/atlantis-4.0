using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class CustomTLDGroupResponseData : IResponseData
  {
    private static CustomTLDGroupResponseData _EMPTYGROUP;

    static CustomTLDGroupResponseData()
    {
      _EMPTYGROUP = new CustomTLDGroupResponseData();
    }

    public static CustomTLDGroupResponseData EmptyGroup
    {
      get { return _EMPTYGROUP; }
    }

    public static CustomTLDGroupResponseData FromDelimitedString(string value, char delimiter = '|')
    {
      CustomTLDGroupResponseData result = _EMPTYGROUP;

      if (!string.IsNullOrEmpty(value))
      {
        char[] delimiters = new char[1] { delimiter };
        string[] tlds = value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

        if (tlds.Length > 0)
        {
          result = new CustomTLDGroupResponseData(tlds);
        }
      }

      return result;
    }

    public static CustomTLDGroupResponseData FromException(AtlantisException exception)
    {
      return new CustomTLDGroupResponseData(exception);
    }

    private HashSet<string> _tlds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    private List<string> _tldsInOrder = new List<string>();
    private AtlantisException _exception = null;

    private CustomTLDGroupResponseData()
    {
    }

    private CustomTLDGroupResponseData(AtlantisException exception)
    {
      _exception = exception;
    }


    private CustomTLDGroupResponseData(IEnumerable<string> tlds)
    {
      foreach (string tld in tlds)
      {
        if (!_tlds.Contains(tld))
        {
          _tlds.Add(tld);
          _tldsInOrder.Add(tld);
        }
      }
    }

    public int Count
    {
      get { return _tlds.Count; }
    }

    public IEnumerable<string> TldsInOrder
    {
      get { return _tldsInOrder; }
    }

    public bool Contains(string tld)
    {
      return _tlds.Contains(tld);
    }

    public string ToXML()
    {
      XElement element = new XElement("CustomTLDGroupResponseData");
      foreach (string tld in _tldsInOrder)
      {
        XElement tldElement = new XElement("tld");
        tldElement.Add(new XAttribute("name", tld));
        element.Add(tldElement);
      }

      return element.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

  }
}
