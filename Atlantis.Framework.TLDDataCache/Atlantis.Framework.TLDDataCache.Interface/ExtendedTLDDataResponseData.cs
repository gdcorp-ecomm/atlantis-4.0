using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class ExtendedTLDDataResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    private readonly string _xmlData;

    private readonly Dictionary<string, Dictionary<string, string>> _tldData;

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
      _xmlData = dataCacheElement.ToString();
      _tldData = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

      foreach (XElement itemElement in dataCacheElement.Elements("item"))
      {
        try
        {
          var dictTldData = new Dictionary<string, string>();
          foreach (XAttribute itemAtt in itemElement.Attributes())
          {
            dictTldData.Add(itemAtt.Name.ToString(), itemAtt.Value);
          }

          _tldData.Add(dictTldData["tld"], dictTldData);
        }
        catch (Exception ex)
        {
          string message = ex.Message + ex.StackTrace;
          var aex = new AtlantisException("ExtendedTLDDataResponseData.ctor", "0", message, itemElement.ToString(), null, null);
          Engine.Engine.LogAtlantisException(aex);
        }
      }
    }

    public Dictionary<string, Dictionary<string, string>> TLDData
    {
      get
      {
        return _tldData;
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
