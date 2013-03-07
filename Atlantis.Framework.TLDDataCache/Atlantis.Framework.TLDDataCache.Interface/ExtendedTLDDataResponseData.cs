using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class ExtendedTLDDataResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    private readonly Dictionary<string, string> _tldData;

    public static ExtendedTLDDataResponseData FromException(RequestData requestData, Exception ex)
    {
      return new ExtendedTLDDataResponseData(requestData, ex);
    }

    private ExtendedTLDDataResponseData(RequestData requestData, Exception ex)
    {
      string message = ex.Message + ex.StackTrace;
      string inputData = requestData.ToXML();
      _exception = new AtlantisException(requestData, "ExtendedTLDDataResponseData.ctor", message, inputData);
    }

    public static ExtendedTLDDataResponseData FromDataCacheElement(XElement dataCacheElement)
    {
      return new ExtendedTLDDataResponseData(dataCacheElement);
    }

    private ExtendedTLDDataResponseData(XElement dataCacheElement)
    {
      _tldData = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

      XElement itemElement = dataCacheElement.Descendants("data").FirstOrDefault();
      if (itemElement != null)
      {
        foreach (XAttribute itemAtt in itemElement.Attributes())
        {
          _tldData[itemAtt.Name.ToString()] = itemAtt.Value;
        }
      }
    }

    public bool TryGetValue(string extendedDataKey, out string value)
    {
      return _tldData.TryGetValue(extendedDataKey, out value);
    }

    public string ToXML()
    {
      XElement element = new XElement("ExtendedTLDData");

      if (_tldData != null)
      {
        foreach (KeyValuePair<string, string> kvp in _tldData)
        {
          element.Add(new XAttribute(kvp.Key, kvp.Value));
        }
      }
      return element.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
