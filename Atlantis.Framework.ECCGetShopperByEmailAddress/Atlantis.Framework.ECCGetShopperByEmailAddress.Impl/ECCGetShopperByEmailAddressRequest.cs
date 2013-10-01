using System;
using Atlantis.Framework.ECCGetShopperByEmailAddress.Interface;
using Atlantis.Framework.Interface;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;

namespace Atlantis.Framework.ECCGetShopperByEmailAddress.Impl
{

  public class ECCGetShopperByEmailAddressRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ECCGetShopperByEmailAddressResponseData response = null;

      try
      {
        var request = (ECCGetShopperByEmailAddressRequestData)requestData;

        string wsUrl = ((WsConfigElement)config).WSURL;
        int timeout = (int)request.RequestTimeout.TotalMilliseconds;
        string shopperId = string.Empty;
        string message = string.Empty;
        string resultCode = string.Empty;

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string jsonPostData = CreateEccPostString(request, serializer);

        HttpWebRequest eccWebRequest = WebRequest.Create(wsUrl) as HttpWebRequest;
        eccWebRequest.Method = "POST";
        eccWebRequest.ContentType = "application/x-www-form-urlencoded";
        eccWebRequest.Timeout = timeout;

        byte[] byteData = Encoding.UTF8.GetBytes(jsonPostData);
        eccWebRequest.ContentLength = byteData.Length;
        using (Stream eccPostStream = eccWebRequest.GetRequestStream())
        {
          eccPostStream.Write(byteData, 0, byteData.Length);
        }

        using (HttpWebResponse eccWebResponse = eccWebRequest.GetResponse() as HttpWebResponse)
        {
          StreamReader responseReader = new StreamReader(eccWebResponse.GetResponseStream());
          string eccWebResponseData = responseReader.ReadToEnd();

          eccWebResponseData = eccWebResponseData.Replace("[]", "\"\""); //need to replace the empty array with an empty string to fix deserialization
          eccWebResponseData = eccWebResponseData.Replace("[", string.Empty).Replace("]", string.Empty); //needed because this webservice doesn't pass back true json

          if (eccWebResponseData.ToLower().Contains("shopper_id"))
          {
            var shopperReturnData = serializer.Deserialize<ReturnDataWithShopperId>(eccWebResponseData);
            shopperId = shopperReturnData.response.Results.shopper_id;
            message = shopperReturnData.response.Message ?? string.Empty;
            resultCode = shopperReturnData.response.ResultCode ?? string.Empty;
          }
          else
          {
            var shopperReturnData = serializer.Deserialize<ReturnData>(eccWebResponseData);
            message = shopperReturnData.response.Message ?? string.Empty;
            resultCode = shopperReturnData.response.ResultCode ?? string.Empty;
          }
        }

        response = new ECCGetShopperByEmailAddressResponseData(shopperId, message, resultCode);
      }
      catch (AtlantisException ex)
      {
        response = new ECCGetShopperByEmailAddressResponseData(ex);
      }
      catch (Exception ex)
      {
        response = new ECCGetShopperByEmailAddressResponseData(requestData, ex);
      }

      return response;
    }

    public string CreateEccPostString(ECCGetShopperByEmailAddressRequestData request, JavaScriptSerializer serializer)
    {
      string jsonPostFormatString = "method=getShopperForEmailAddress&key=tH15!zt433Cc@P1*&params=[{0}]";

      Dictionary<string, string> jsonPostParams = new Dictionary<string, string>();
      Dictionary<string, string> jsonRequestParams = new Dictionary<string, string>();

      jsonRequestParams["emailaddress"] = request.EmailAddress;
      jsonPostParams["Id"] = request.AuthId;
      jsonPostParams["Token"] = request.Token;
      jsonPostParams["Parameters"] = "{{parameters}}";


      string serialReqParams = serializer.Serialize(jsonRequestParams);
      string serialPostParams = serializer.Serialize(jsonPostParams);
      serialPostParams = serialPostParams.Replace("\"{{parameters}}\"", serialReqParams);

      string jsonPostData = string.Format(jsonPostFormatString, serialPostParams);

      return jsonPostData;
    }
  }

  #region Simple Deserialization Classes
  public class ReturnData
  {
    public ResultData response { get; set; }
  }

  public class ReturnDataWithShopperId
  {
    public ResultDataWithShopperId response { get; set; }
  }

  public class ResultData
  {
    public string ResultCode { get; set; }
    public string Message { get; set; }
  }

  public class ResultDataWithShopperId : ResultData
  {
    public Results Results { get; set; }
  }

  public class Results
  {
    public string shopper_id { get; set; }
  }
  #endregion
}
