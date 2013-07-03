using System;
using System.Xml.Linq;
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
        bool isXmlResponse = false;
        XElement formsElement = null;
        try
        {
          formsElement = XElement.Parse(responseHtml);
          isXmlResponse = true;
        }
        catch (Exception)
        {
        }

        if (isXmlResponse)
        { 
          var error = formsElement.Name.LocalName;
          if (!error.Equals("error"))
          {
            _isSuccess = true;
          }
          _responseHtml = responseHtml;
          _exception = null;
        }
        else
        {
          _responseHtml = responseHtml;
          _exception = null;
          _isSuccess = true;
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
  }
}
