using System;
using System.Net;
using System.Web;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using Atlantis.Framework.Engine;
using Atlantis.Framework.Interface;
using Atlantis.Framework.RCCUpdatePLCategoryData.Interface;
using Atlantis.Framework.RCCUpdatePLCategoryData.Impl.gdmasterapi;

namespace Atlantis.Framework.RCCUpdatePLCategoryData.Impl
{
    public class RCCUpdatePLCategoryData : IRequest
    {
        public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
        {
            RCCUpdatePLCategoryDataResponseData response = null;
            RCCUpdatePLCategoryDataRequestData plDataUpdate = null;
            List<PLDataItem> exceptionList = new List<PLDataItem>();

            try
            {
                plDataUpdate = requestData as RCCUpdatePLCategoryDataRequestData;

                using (gdmasterapi.Service service = new Service())
                {
                    string wsUrl = ((WsConfigElement)config).WSURL;
                    service.Url = wsUrl;
                    service.Timeout = (int)Math.Truncate(plDataUpdate.RequestTimeout.TotalMilliseconds);
                    X509Certificate2 cert = ((WsConfigElement)config).GetClientCertificate();
                    service.Credentials = CredentialCache.DefaultCredentials;
                    service.PreAuthenticate = true;

                    if (cert == null)
                        throw new AtlantisException(plDataUpdate, "RCCUpdatePLCategoryData::RequestHandler", "Invalid or missing certificate passed to triplet", null);

                    service.ClientCertificates.Add(cert);

                    for (int i = 0; i < plDataUpdate.PlDataItems.Count; i++)
                    {
                        PLDataItem item = plDataUpdate.PlDataItems[i];

                        if (item.PrivateLabelId == 0)
                        {
                            exceptionList.Add(item);
                            exceptionList[i].UpdateResponseStatus("No Private Label Id supplied to handler.");
                            plDataUpdate.RemovePlDataUpdateItem(item);
                        }
                        else if (item.PlDataCategoryId == 0)
                        {
                            exceptionList.Add(item);
                            exceptionList[i].UpdateResponseStatus("No PL Data Category Id supplied to handler.");
                            plDataUpdate.RemovePlDataUpdateItem(item);
                        }
                        else
                        {
                            string result = service.UpdatePLData(item.PrivateLabelId, item.PlDataCategoryId, item.PlData);

                            XmlDocument resultDoc = new XmlDocument();
                            resultDoc.LoadXml(result);
                            XmlNode rootNode = resultDoc.DocumentElement;
                            XmlNode exceptionNode = rootNode.SelectSingleNode("Error");

                            if (exceptionNode != null)
                            {
                                XmlNode exceptionDescNode = rootNode.SelectSingleNode("Description");

                                if (exceptionDescNode != null)
                                {
                                    if (exceptionDescNode.InnerText.ToLowerInvariant().Contains("fk_pl_data_pl_entity"))
                                    {
                                        item.UpdateResponseStatus("The specified Private Label Id [" + item.PrivateLabelId.ToString() + "] does not exist");
                                        exceptionList.Add(item);
                                    }
                                    else if (exceptionDescNode.InnerText.ToLowerInvariant().Contains("fk_pl_data_pl_datacategory"))
                                    {
                                        item.UpdateResponseStatus("The specified PL Data Category Id [" + item.PlDataCategoryId.ToString() + "] does not exist");
                                        exceptionList.Add(item);
                                    }
                                    else
                                    {
                                        item.UpdateResponseStatus("The following error was returned from the service: " + exceptionDescNode.InnerText);
                                        exceptionList.Add(item);
                                    }
                                }
                            }
                        }
                    }

                    if (exceptionList.Count > 0)
                    {
                        if (plDataUpdate.PlDataItems.Count == exceptionList.Count)
                            response = new RCCUpdatePLCategoryDataResponseData(plDataUpdate, new AtlantisException(plDataUpdate, "RCCUpdatePLCategoryData::RequestHandler", exceptionList[0].PlResponse, null));
                        else
                            response = new RCCUpdatePLCategoryDataResponseData(plDataUpdate, exceptionList);
                    }
                    else
                    {
                        response = new RCCUpdatePLCategoryDataResponseData(plDataUpdate, "<Status>Success</Status>");
                    }
                }                
            }
            catch (System.Net.WebException exception)
            {
                response = new RCCUpdatePLCategoryDataResponseData(plDataUpdate, exception.Status);
            }
            catch (AtlantisException exception)
            {
                response = new RCCUpdatePLCategoryDataResponseData(plDataUpdate, exception);
            }
            catch (Exception exception)
            {
                response = new RCCUpdatePLCategoryDataResponseData(plDataUpdate, new AtlantisException(plDataUpdate, "RCCUpdatePLCategoryData::RequestHandler", exception.Message, exception.StackTrace));
            }

            return response;
        }
    }
}
