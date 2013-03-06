using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class ValidDotTypesResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    private readonly string _xmlData;

    private readonly HashSet<string> _validDotTypes;

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

    public static ValidDotTypesResponseData FromDataCacheElement(XElement dataCacheElement)
    {
      return new ValidDotTypesResponseData(dataCacheElement);
    }

    private ValidDotTypesResponseData(XElement dataCacheElement)
    {
      _xmlData = dataCacheElement.ToString();
      _validDotTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

      foreach (XElement itemElement in dataCacheElement.Elements("item"))
      {
        try
        {
          string tld = itemElement.Attribute("tld").Value;
          
          if (!string.IsNullOrEmpty(tld))
          {
            _validDotTypes.Add(tld);
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

    public IEnumerable<string> ValidDotTypes
    {
      get
      {
        return _validDotTypes;
      }
    }
    
    public string ToXML()
    {
      string result = "<exception/>";
      if (_xmlData != null)
      {
        result = _xmlData;
      }
      return result;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
