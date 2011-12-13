using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.EcommPaymentProfileUnique.Interface;

namespace Atlantis.Framework.EcommPaymentProfileUnique.Impl
{
    public class EcommPaymentProfileUniqueRequest : IRequest
    {
        public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
        {
            EcommPaymentProfileUniqueResponseData responseData = null;

            try
            {

                var taskRequest = (EcommPaymentProfileUniqueRequestData)requestData;

                if (!String.IsNullOrEmpty(taskRequest.ShopperID) &&
                    !String.IsNullOrEmpty(taskRequest.OrderID))
                {

                    var service = new PaymentProfileUnique.Service1()
                    {
                        Url = ((WsConfigElement)config).WSURL,
                        Timeout =
                            (int)
                            taskRequest.RequestTimeout.TotalMilliseconds
                    };
                    bool bUsesNonUniquePaymentType = false;
                    string sErrorXml;
                    X509Certificate2 cert = FindCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, config.GetConfigValue("CertificateName"));

                    cert.Verify();
                    service.ClientCertificates.Add(cert);

                    bool response = service.IsUniquePaymentProfile(requestData.ShopperID, requestData.OrderID,
                                                                     out bUsesNonUniquePaymentType, out sErrorXml);
                    responseData = new EcommPaymentProfileUniqueResponseData(response, bUsesNonUniquePaymentType, sErrorXml);
                }
                else
                {
                    throw new ArgumentException("ShopperID or OrderID are incorrect.");
                }


            }

            catch (AtlantisException exAtlantis)
            {
                responseData = new EcommPaymentProfileUniqueResponseData(exAtlantis);
            }

            catch (Exception ex)
            {
                responseData = new EcommPaymentProfileUniqueResponseData(requestData, ex);
            }

            return responseData;
        }
        private X509Certificate2 FindCertificate(StoreLocation location, StoreName name, X509FindType findType, string findValue)
        {
            X509Store store = new X509Store(name, location);

            try
            {
                // create and open store for read-only access
                store.Open(OpenFlags.ReadOnly);

                // search store
                X509Certificate2Collection col = store.Certificates.Find(findType, findValue, true);

                // return first certificate found
                return col[0];
            }
            // always close the store
            finally
            {
                store.Close();
            }
        }
    }
}
