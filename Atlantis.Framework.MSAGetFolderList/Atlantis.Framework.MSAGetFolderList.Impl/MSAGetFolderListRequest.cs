using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MSA.Interface;
using Atlantis.Framework.MSAGetFolderList.Interface;

namespace Atlantis.Framework.MSAGetFolderList.Impl
{
    public class MSAGetFolderListRequest : IRequest
    {
      private const string BodyString = "method=getFolderList&params={\"extended_info\":\"true\"}";

      public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
      {

        MSAGetFolderListRequestData getFolderListRequestData = (MSAGetFolderListRequestData)requestData;
        MSAGetFolderListResponseData getFolderListResponseData;
        GetFolderListResponse getFolderListResponse = new GetFolderListResponse();
        try
        {
        
          string webServiceUrl = getFolderListRequestData.BaseUrl.Contains("80") ? "http://" : "https://";

          webServiceUrl += getFolderListRequestData.BaseUrl;
          webServiceUrl += config.GetConfigValue("ServicePath");


          HttpWebResponse webResponse = MailApiUtil.sendMailAPIRequest(webServiceUrl, BodyString,
                                                                             getFolderListRequestData.Hash,
                                                                             getFolderListRequestData.ApiKey);

          Stream responseStream = webResponse.GetResponseStream();
          StreamReader responseReader = new StreamReader(responseStream, Encoding.UTF8);
          string jsonResponse = responseReader.ReadToEnd();

          MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse));

          DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GetFolderListResponse));

          getFolderListResponse = (GetFolderListResponse)serializer.ReadObject(stream);
          responseStream.Close();
          responseReader.Close();
          stream.Close();
          getFolderListResponseData = new MSAGetFolderListResponseData(getFolderListResponse);

        }
        catch (Exception ex)
        {
          AtlantisException aex = new AtlantisException(requestData, "MSALoginUserRequest.RequestHandler", ex.Message,
                                                        ex.StackTrace, ex);
          getFolderListResponseData = new MSAGetFolderListResponseData(aex);
        }

        return getFolderListResponseData;
      }
    }
}
