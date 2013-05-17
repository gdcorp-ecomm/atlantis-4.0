using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeForms.Interface
{
  public class DotTypeFormsXmlSchemaResponseData : IResponseData
  {
    private readonly string _responseXml;
    private readonly AtlantisException _exception;
    private readonly bool _isSuccess;
    private readonly DotTypeFormsSchema _dotTypeFormsSchema;

    public DotTypeFormsXmlSchemaResponseData(string responseXml)
    {
      _responseXml = responseXml;
      _exception = null;
      _dotTypeFormsSchema = new DotTypeFormsSchema(responseXml);
      _isSuccess = _dotTypeFormsSchema.IsSuccess;
    }

    public DotTypeFormsXmlSchemaResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
    }

    public DotTypeFormsXmlSchemaResponseData(string responseXml, RequestData requestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(requestData, "DotTypeFormsXmlSchemaResponseData", ex.Message, requestData.ToXML());
    }

    public DotTypeFormsSchema DotTypeFormsSchema
    {
      get { return _dotTypeFormsSchema; }
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
      return _responseXml;
    }
  }
}
