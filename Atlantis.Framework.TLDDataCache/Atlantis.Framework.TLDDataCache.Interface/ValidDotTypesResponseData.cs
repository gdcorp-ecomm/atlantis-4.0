using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class ValidDotTypesResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    private readonly Dictionary<string, int> _validDotTypes;
    private readonly Dictionary<int, string> _validDotTypesById;

    public static ValidDotTypesResponseData FromException(RequestData requestData, Exception ex)
    {
      return new ValidDotTypesResponseData(requestData, ex);
    }

    private ValidDotTypesResponseData(RequestData requestData, Exception ex)
    {
      string message = ex.Message + ex.StackTrace;
      string inputData = requestData.ToXML();
      _exception = new AtlantisException(requestData, "ValidDotTypesResponseData.ctor", message, inputData);
    }

    public static ValidDotTypesResponseData FromDataCacheElement(XElement cacheDataElement)
    {
      return new ValidDotTypesResponseData(cacheDataElement);
    }

    private ValidDotTypesResponseData(XElement cacheDataElement)
    {
      _validDotTypes = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
      _validDotTypesById = new Dictionary<int, string>();

      foreach (XElement itemElement in cacheDataElement.Descendants("item"))
      {
        try
        {
          string tld = itemElement.Attribute("tld").Value;
          int tldid = Convert.ToInt32(itemElement.Attribute("tldid").Value);
          
          if (!string.IsNullOrEmpty(tld))
          {
            _validDotTypes.Add(tld, tldid);
            _validDotTypesById.Add(tldid, tld);
          }
        }
        catch (Exception ex)
        {
          string message = ex.Message + ex.StackTrace;
          var aex = new AtlantisException("ValidDotTypesResponseData.ctor", "0", message, itemElement.ToString(), null, null);
          Engine.Engine.LogAtlantisException(aex);
        }

      }
    }

    public bool TryGetTldId(string tld, out int tldId)
    {
      return _validDotTypes.TryGetValue(tld, out tldId);
    }

    public bool TryGetTldById(int tldId, out string tld)
    {
      return _validDotTypesById.TryGetValue(tldId, out tld);
    }

    public bool IsValidTld(string tld)
    {
      return _validDotTypes.ContainsKey(tld);
    }

    public string ToXML()
    {
      XElement element = new XElement("validdottypes");

      foreach (KeyValuePair<string, int> kvp in _validDotTypes)
      {
        XElement tld = new XElement("tld",
          new XAttribute("name", kvp.Key),
          new XAttribute("id", kvp.Value.ToString()));
        element.Add(tld);
      }

      return element.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
