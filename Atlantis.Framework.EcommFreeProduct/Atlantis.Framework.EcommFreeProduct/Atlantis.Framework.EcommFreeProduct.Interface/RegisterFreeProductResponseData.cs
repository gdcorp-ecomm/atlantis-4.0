
using System.Collections.Generic;

namespace Atlantis.Framework.EcommFreeProduct.Interface
{
  using System;
  using System.Linq;
  using System.Xml.Linq;
  using Atlantis.Framework.Interface;

  public class RegisterFreeProductResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;
    private XDocument xDoc = null;

    public RegisterFreeProductResponseData(string xml)
    {
      this._resultXML = xml;
    }

    public RegisterFreeProductResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public RegisterFreeProductResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData, "RegFreeProductResponseData", exception.Message, requestData.ToXML());
    }

    private XDocument Document
    {
      get
      {
        if (xDoc == null)
        {
          xDoc = XDocument.Parse(this._resultXML);
        }
        return xDoc;
      }
    }

    public bool IsSuccess
    {
      get
      {
        if (!string.IsNullOrEmpty(this._resultXML))
        {
          _success = !Document.Elements("error").Any() && Document.Descendants("Status").Any() && string.Equals("success", Document.Descendants("Status").First().Value, StringComparison.OrdinalIgnoreCase);            
        }
        return _success;
      }
    }

    public IEnumerable<XNode> Items
    {
      get
      {
        if (!string.IsNullOrEmpty(this._resultXML))
        {
          return Document.Descendants("ITEM");
        }
        return null;
      }
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return _resultXML;
    }

  }
}
