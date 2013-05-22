using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  public class GeneralEoiJsonResponseData : IResponseData
  {
    private readonly string _responseXml;
    private readonly AtlantisException _exception;
    private readonly bool _isSuccess;
    private IDotTypeEoiResponse _dotTypeEoiResponse;

    public GeneralEoiJsonResponseData(string responseJson)
    {
      _exception = null;
      _isSuccess = DeserializeJson(responseJson);
    }

    public GeneralEoiJsonResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
      _isSuccess = false;
    }

    public GeneralEoiJsonResponseData(string responseXml, RequestData requestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(requestData, "DotTypeGetGeneralEoiJsonResponseData", ex.Message, requestData.ToXML());
      _isSuccess = false;
    }

    public IDotTypeEoiResponse DotTypeEoiResponse
    {
      get { return _dotTypeEoiResponse; }
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

    private bool DeserializeJson(string json)
    {
      bool success = true;
      try
      {
        var ser = new DataContractJsonSerializer(typeof(DotTypeJsonResponse));
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        var dotTypeJsonResponse = (DotTypeJsonResponse)ser.ReadObject(stream);
        _dotTypeEoiResponse = dotTypeJsonResponse.DotTypeEoiResponse;
        stream.Close();
      }
      catch (Exception e)
      {
        success = false;
      }
      return success;
    }
  }
}
