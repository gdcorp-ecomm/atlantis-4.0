using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;
using System.Linq;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TLDMLByNameResponseData : IResponseData
  {
    private XDocument _tldmlDocument;
    private AtlantisException _exception;

    private TLDMLRegistration _registration;

    private TLDMLByNameResponseData(XDocument tldmlDocument)
    {
      _tldmlDocument = tldmlDocument;
      _registration = new TLDMLRegistration(_tldmlDocument);
    }

    private TLDMLByNameResponseData(RequestData requestData, Exception ex)
    {
      string message = ex.Message + ex.StackTrace;
      string inputData = requestData.ToXML();
      _exception = new AtlantisException(requestData, "TLDMLByIdResponseData.ctor", message, inputData);
    }

    public static TLDMLByNameResponseData FromXDocument(XDocument tldmlDocument)
    {
      return new TLDMLByNameResponseData(tldmlDocument);
    }

    public static TLDMLByNameResponseData FromException(RequestData requestData, Exception ex)
    {
      return new TLDMLByNameResponseData(requestData, ex);
    }

    public TLDMLRegistration Registration
    {
      get { return _registration; }
    }

    public string ToXML()
    {
      string result = "<exception/>";
      if (_tldmlDocument != null)
      {
        result = _tldmlDocument.ToString();
      }
      return result;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
