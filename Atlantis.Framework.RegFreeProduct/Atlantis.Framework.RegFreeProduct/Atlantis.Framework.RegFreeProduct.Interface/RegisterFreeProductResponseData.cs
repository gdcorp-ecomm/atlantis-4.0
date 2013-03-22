
namespace Atlantis.Framework.RegFreeProduct.Interface
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

    public bool IsSuccess
    {
      get
      {
        if (!string.IsNullOrEmpty(this._resultXML))
        {
          XDocument xDoc = XDocument.Parse(this._resultXML);
          _success = !xDoc.Elements("error").Any() && xDoc.Elements("Status").Any() && string.Equals("success", xDoc.Element("Status").Value, StringComparison.OrdinalIgnoreCase);
        }
        return _success;
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
