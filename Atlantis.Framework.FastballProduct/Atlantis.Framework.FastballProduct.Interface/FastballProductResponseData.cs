using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.FastballProduct.Interface
{
  public class FastballProductResponseData : IResponseData, ISessionSerializableResponse
  {
    private Dictionary<string, string> _fastballProductData;
    private AtlantisException _exception = null;
    private const string _ITEMFORMAT = "<item key=\"{0}\"><![CDATA[{1}]]></item>";

    public FastballProductResponseData(IDictionary<string, string> productData)
    {
      if (productData == null)
      {
        _fastballProductData = new Dictionary<string, string>();
      }
      else
      {
        _fastballProductData = new Dictionary<string, string>(productData);
      }
    }

    public FastballProductResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, "FastballProductResponseData.ctor", ex.Message, ex.StackTrace, ex);
    }

    public bool TryGetFastballValue(string key, out string value)
    {
      bool result = false;
      value = null;

      if (_fastballProductData != null)
      {
        result = _fastballProductData.TryGetValue(key, out value);
      }

      return result;
    }

    public string ToXML()
    {
      StringBuilder xml = new StringBuilder("<data>");

      if (_fastballProductData != null)
      {
        foreach (string key in _fastballProductData.Keys)
        {
          if (key != null)
          {
            xml.AppendFormat(_ITEMFORMAT, key, _fastballProductData[key] ?? string.Empty);
          }
        }
      }

      xml.Append("</data>");
      return xml.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string SerializeSessionData()
    {
      return ToXML();
    }

    public void DeserializeSessionData(string sessionData)
    {
      _fastballProductData = new Dictionary<string, string>();

      XDocument doc = XDocument.Parse(sessionData);
      IEnumerable<XElement> items = doc.Descendants("item");

      foreach (XElement item in items)
      {
        XAttribute key = item.Attribute("key");
        if (key != null)
        {
          _fastballProductData[key.Value] = item.Value;
        }
      }
    }
  }
}
