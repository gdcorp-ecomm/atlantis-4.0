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
    public string OfferId { get; private set; }
    private Dictionary<string, string> _fastballProductData;
    private AtlantisException _exception = null;
    private const string _ITEMFORMAT = "<item key=\"{0}\"><![CDATA[{1}]]></item>";
    private const string _DATAFORMAT = "<data offerId=\"{0}\" status=\"{1}\">";
    public int Status { get; set; }

    public FastballProductResponseData()
    {
    }

    public FastballProductResponseData(IDictionary<string, string> productData, int status, string offerId = "")
    {
      OfferId = offerId;
      Status = status;

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
      StringBuilder xml = new StringBuilder(string.Empty);
      xml.AppendFormat(_DATAFORMAT, OfferId, Status);

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

      XElement data = doc.Root;
      XAttribute offerId = data.Attribute("offerId");
      if (offerId != null)
      {
        OfferId = offerId.Value;
      }

      XAttribute status = data.Attribute("status");
      if (status != null)
      {
        int statusInt;
        if (int.TryParse(status.Value, out statusInt))
        {
          if (statusInt == FastballProductStatus.Valid)
          {
            statusInt = FastballProductStatus.ValidCached;
          }
          else if (statusInt == FastballProductStatus.Failed)
          {
            statusInt = FastballProductStatus.FailedCached;
          }
          Status = statusInt;
        }
      }

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
