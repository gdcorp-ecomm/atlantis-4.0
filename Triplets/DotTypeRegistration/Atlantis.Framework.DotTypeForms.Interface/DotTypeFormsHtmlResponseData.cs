using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeForms.Interface
{
  public class DotTypeFormsHtmlResponseData : IResponseData
  {
    private readonly string _responseHtml;
    private readonly AtlantisException _exception;
    private readonly bool _isSuccess;

    public DotTypeFormsHtmlResponseData(string responseHtml)
    {
      _responseHtml = responseHtml;
      _exception = null;
      _isSuccess = true;
    }

    public DotTypeFormsHtmlResponseData(string responseHtml, AtlantisException exAtlantis)
    {
      _responseHtml = responseHtml;
      _exception = exAtlantis;
    }

    public DotTypeFormsHtmlResponseData(string responseHtml, RequestData requestData, Exception ex)
    {
      _responseHtml = responseHtml;
      _exception = new AtlantisException(requestData, "DotTypeFormsHtmlSchemaResponseData", ex.Message, requestData.ToXML());
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return _responseHtml;
    }
  }
}
