using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.DotTypeValidation.Interface
{
  public class DotTypeValidationResponseData : IResponseData
  {
    private readonly string _responseXml;
    private readonly AtlantisException _exception;
    private bool _isSuccess;
    private bool _hasErrors;
    private Dictionary<string, string> _validationErrors;
    private string _token;

    public DotTypeValidationResponseData(string responseXml)
    {
      _responseXml = responseXml;
      _exception = null;
      BuildFromXml();
    }

    public DotTypeValidationResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
    }

    public DotTypeValidationResponseData(string responseXml, RequestData requestData, Exception ex)
    {
      _responseXml = responseXml;
      var message = string.Format("{0} | {1}", ex.Message, ex.StackTrace);
      _exception = new AtlantisException("DotTypeFormsValidationResponseData", 0, message, requestData.ToXML());
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
    }

    public bool HasErrors
    {
      get { return _hasErrors; }
    }

    public Dictionary<string, string> ValidationErrors
    {
      get { return _validationErrors; }
    }

    public string Token
    {
      get { return _token; }
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return _responseXml;
    }

    void BuildFromXml()
    {
      try
      {
        var rootElement = XElement.Parse(_responseXml);

        if (!rootElement.Name.LocalName.Equals("error"))
        {
          if (rootElement.Name.LocalName.Equals("errors"))
          {
            _hasErrors = true;
            _validationErrors = new Dictionary<string, string>();

            foreach (var errorElement in rootElement.Descendants("error"))
            {
              var key = errorElement.Attribute("key").Value;
              var errorMessage = errorElement.Value;
              _validationErrors.Add(key, errorMessage);
            }

            _token = string.Empty;
          }
          else if (rootElement.Name.ToString().Equals("success"))
          {
            _hasErrors = false;
            _token = rootElement.Value;

            _isSuccess = true;
          }
        }
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("DotTypeFormsValidationResponseData.BuildFromXml", 0, ex.Message + ex.StackTrace, _responseXml);
        Engine.Engine.LogAtlantisException(exception);
      }
    }
  }
}
