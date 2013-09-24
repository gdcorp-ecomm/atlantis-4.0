using System;
using System.Web.Script.Serialization;
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
      if (!string.IsNullOrEmpty(responseHtml))
      {
        _exception = null;
        bool isError = false;
        try
        {
          var serializer = new JavaScriptSerializer();
          var data = serializer.Deserialize<TuiResponse>(responseHtml);

          isError = data.ResponseType.Equals("error");
        }
        catch (Exception)
        {
        }

        _responseHtml = responseHtml;

        if (!isError)
        {
          _isSuccess = true;            
        }
        else
        {
          _responseHtml = string.Empty;
          _isSuccess = false;  
        }
      }
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

    [Serializable]
    internal class TuiResponse
    {
      public string ResponseType; 
      public string Message;
    }
  }
}
