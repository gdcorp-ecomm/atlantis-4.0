using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  public class ShopperWatchListResponseData : IResponseData
  {
    private readonly string _responseXml;
    private readonly AtlantisException _exception;
    private readonly bool _isSuccess;
    private IShopperWatchListResponse _shopperWatchListResponse;

    public ShopperWatchListResponseData(string responseJson)
    {
      _exception = null;
      _isSuccess = DeserializeJson(responseJson);
    }

    public ShopperWatchListResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
      _isSuccess = false;
    }

    public ShopperWatchListResponseData(string responseXml, RequestData requestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(requestData, "ShopperWatchListResponseData", ex.Message, requestData.ToXML());
      _isSuccess = false;
    }

    public IShopperWatchListResponse ShopperWatchListResponse
    {
      get { return _shopperWatchListResponse; }
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
        var ser = new DataContractJsonSerializer(typeof(ShopperWatchListJsonResponse));
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        var shopperWatchListJsonResponse = (ShopperWatchListJsonResponse)ser.ReadObject(stream);
        _shopperWatchListResponse = shopperWatchListJsonResponse.ShopperWatchListResponse;
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
