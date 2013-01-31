using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using Atlantis.Framework.Interface;
using System;
using Atlantis.Framework.MobilePushEmailGetSub.Interface;
using Atlantis.Framework.MobilePushEmailGetSubscriptions.Interface.Objects;

namespace Atlantis.Framework.MobilePushEmailGetSubscriptions.Impl
{
    public class MobilePushEmailGetSubscriptionsRequest : IRequest
    {
        private const string EccGetSubscriptionById = "{0}?action=getSubscriptions";
        private const string EccGetSubscriptionByEmail = "{0}?action=getSubscriptions&login={1}";
        private const string NonExistantLogin = "LOGIN DOES NOT EXIST";

        public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
        {
            IResponseData result = null;

            MobilePushEmailGetSubscriptionsRequestData mobPushGetSubsRequest = (MobilePushEmailGetSubscriptionsRequestData) requestData;
            WsConfigElement wsConfig = (WsConfigElement) config;

            try
            {
                // Email goes in query string
                // SubscriptionID goes in HTTP header ("X-Subscription-Id")
                // TODO: Determine in testing if we have to have the Email, if we use the SubscriptionID
                if (string.IsNullOrEmpty(mobPushGetSubsRequest.Email) && string.IsNullOrEmpty(mobPushGetSubsRequest.SubscriptionId))
                {
                    throw new Exception("Either Email or SubscriptionID must be present in parameters");
                }
                HttpWebRequest webRequest;
                string subscriptionUrl;
                ICollection<KeyValuePair<string, string>> requestHeaders = new Collection<KeyValuePair<string, string>>();
                if (string.IsNullOrEmpty(mobPushGetSubsRequest.Email))
                {
                    subscriptionUrl = string.Format(EccGetSubscriptionById, wsConfig.WSURL);
                    requestHeaders.Add(new KeyValuePair<string, string>("X-Subscription-Id", mobPushGetSubsRequest.SubscriptionId));
                }
                else
                {
                    subscriptionUrl = string.Format(EccGetSubscriptionByEmail, wsConfig.WSURL, mobPushGetSubsRequest.Email);
                }
                webRequest = WebRequest.Create(subscriptionUrl) as HttpWebRequest;
                if (webRequest != null)
                {
                    webRequest.Timeout = (int) requestData.RequestTimeout.TotalMilliseconds;
                    webRequest.Method = "POST";
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                    webRequest.ContentLength = 0;

                    foreach (KeyValuePair<string, string> reqHeader in requestHeaders)
                    {
                        webRequest.Headers.Add(reqHeader.Key, reqHeader.Value);
                    }
                    using (HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse)
                    {
                        if (response != null && webRequest.HaveResponse)
                        {
                            Stream responseStream = response.GetResponseStream();
                            if (responseStream != null)
                            {
                                StreamReader responseReader = new StreamReader(responseStream, Encoding.UTF8);
                                string jsonResponse = responseReader.ReadToEnd();
                                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse));

                                try
                                {
                                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(PushEmailSubscription[]));
                                    PushEmailSubscription[] subscriptions = (PushEmailSubscription[])serializer.ReadObject(stream);
                                    result =    new MobilePushEmailGetSubscriptionsResponseData(subscriptions);                             }
                                catch (Exception ex)
                                {
                                    if (jsonResponse.ToUpper().Contains(NonExistantLogin))
                                    {
                                        result = new MobilePushEmailGetSubscriptionsResponseData((PushEmailSubscription[])null);
                                        ((MobilePushEmailGetSubscriptionsResponseData) result).LoginExists = false;
                                    }
                                    else
                                    {
                                        result = new MobilePushEmailGetSubscriptionsResponseData(requestData, ex);
                                    }
                                }
                                responseStream.Close();
                                responseReader.Close();
                                stream.Close();
                            }
                            else
                            {
                                AtlantisException exception = new AtlantisException(requestData,
                                                                                    MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                                                                    "No responseStream, called " +
                                                                                    subscriptionUrl,string.Empty);
                                result = new MobilePushEmailGetSubscriptionsResponseData(exception);

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                result = new MobilePushEmailGetSubscriptionsResponseData(requestData, ex);
            }



            return result;
        }
    }
}
