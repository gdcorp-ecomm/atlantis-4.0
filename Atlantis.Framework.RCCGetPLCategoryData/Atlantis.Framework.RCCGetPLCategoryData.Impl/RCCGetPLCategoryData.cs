using System;
using System.Xml.Linq;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Engine;
using Atlantis.Framework.RCCGetPLCategoryData.Interface;
using Atlantis.Framework.RCCGetPLCategoryData.Impl.gdMasterAPI;

namespace Atlantis.Framework.RCCGetPLCategoryData.Impl
{
    public class RCCGetPLCategoryData : IRequest
    {
        public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
        {
            RCCGetPLCategoryDataRequestData request = null;
            RCCGetPLCategoryDataResponseData responseData = null;
            Service webService = null;
            List<PLCategoryDataItemResponse> responseList = null;
            XElement responseXml = null;

            try
            {
                request = requestData as RCCGetPLCategoryDataRequestData;

                if (request == null || request.PLCategoryDataRequests == null || request.PLCategoryDataRequests.Count == 0)
                    throw new ArgumentException("Supplied value was either null or empty.", "requestData");

                responseXml = new XElement("response");
                using (webService = new Service())
                {
                    webService.Url = ((WsConfigElement)config).WSURL;
                    webService.Timeout = 30000;
                    webService.ClientCertificates.Add(((WsConfigElement)config).GetClientCertificate());

                    responseList = new List<PLCategoryDataItemResponse>();
                    foreach (PLCategoryDataItemRequest requestItem in request.PLCategoryDataRequests)
                    {
                        try
                        {
                            XElement item = new XElement("plCategoryDataItemResponse");
                            item.Add(new XAttribute("privateLabelId", requestItem.PrivateLabelId));
                            item.Add(new XAttribute("plCategoryId", requestItem.PLCategoryId));
                            string wsResponse = webService.GetPLDataByCategory(requestItem.PrivateLabelId, requestItem.PLCategoryId);
                            item.Add(new XAttribute("plData", wsResponse));
                            responseXml.Add(item);
                            responseList.Add(new PLCategoryDataItemResponse(requestItem, wsResponse, true));
                        }
                        catch
                        {
                            responseList.Add(new PLCategoryDataItemResponse(requestItem, string.Empty, false));
                        }
                    }
                }

                responseData = new RCCGetPLCategoryDataResponseData(request, responseXml.ToString(), responseList);
            }
            catch (System.Net.WebException exception)
            {
                responseData = new RCCGetPLCategoryDataResponseData(requestData, exception.Status);
            }
            catch (AtlantisException exception)
            {
                responseData = new RCCGetPLCategoryDataResponseData(requestData, responseXml.ToString(), exception);
            }
            catch (Exception exception)
            {
                responseData = new RCCGetPLCategoryDataResponseData(requestData, responseXml.ToString(), exception);
            }

            return responseData;
        }
    }
}
