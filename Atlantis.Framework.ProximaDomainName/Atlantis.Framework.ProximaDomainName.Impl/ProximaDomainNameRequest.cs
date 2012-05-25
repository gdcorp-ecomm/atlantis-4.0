using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ProximaDomainName.Impl.ProximaWS;
using Atlantis.Framework.ProximaDomainName.Interface;

namespace Atlantis.Framework.ProximaDomainName.Impl
{
    public class ProximaDomainNameRequest : IRequest
    {
        public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
        {
            ProximaDomainNameResponseData oResponseData = null;

            try
            {
                ProximaDomainNameRequestData request = (ProximaDomainNameRequestData)oRequestData;
                WSCgdProximaService service = new WSCgdProximaService();
                service.Url = ((WsConfigElement)oConfig).WSURL;
                service.Timeout = request.Timeout;

                string response = service.GetProximaOptionsByDomainName(request.Domain);
                oResponseData = new ProximaDomainNameResponseData(response);
            }
            catch (Exception ex)
            {
                oResponseData = new ProximaDomainNameResponseData(oRequestData, ex);
            }

            return oResponseData;
        }
    }
}