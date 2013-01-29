using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TLDMLByNameResponseData : IResponseData
  {
    private XDocument _tldmlDocument;
    private AtlantisException _exception;

    private ITLDProduct _product;

    private TLDMLByNameResponseData(XDocument tldmlDocument)
    {
      _tldmlDocument = tldmlDocument;
      _product = new TLDMLProduct(_tldmlDocument) as ITLDProduct;
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

    public ITLDProduct Product
    {
      get { return _product; }
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
