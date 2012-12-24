using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TLDMLByNameResponseData : IResponseData
  {
    private XElement _tldmlElement;
    private AtlantisException _exception;

    public TLDMLStatusType Status { get; private set; }
    public string StatusMessage { get; private set; }

    private TLDMLByNameResponseData(XElement tldmlElement)
    {
      _tldmlElement = tldmlElement;
    }

    private TLDMLByNameResponseData(RequestData requestData, Exception ex)
    {
      string message = ex.Message + ex.StackTrace;
      string inputData = requestData.ToXML();
      _exception = new AtlantisException(requestData, "TLDMLByIdResponseData.ctor", message, inputData);
    }

    public static TLDMLByNameResponseData FromXElement(XElement tldmlElement)
    {
      return new TLDMLByNameResponseData(tldmlElement);
    }

    public static TLDMLByNameResponseData FromException(RequestData requestData, Exception ex)
    {
      return new TLDMLByNameResponseData(requestData, ex);
    }

    public string ToXML()
    {
      string result = "<exception/>";
      if (_tldmlElement != null)
      {
        result = _tldmlElement.ToString();
      }
      return result;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
