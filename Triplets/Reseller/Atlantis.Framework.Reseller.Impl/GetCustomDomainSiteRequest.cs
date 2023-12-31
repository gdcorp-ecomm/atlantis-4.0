﻿using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Reseller.Interface;
using Atlantis.Framework.Reseller.Interface.CustomDomains;

namespace Atlantis.Framework.Reseller.Impl
{
  public class GetCustomDomainSiteRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        using (GdDataCacheOutOfProcess dataCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          string dataCacheResponse = dataCache.GetCacheData(requestData.ToXML());
          XElement response = XElement.Parse(dataCacheResponse);

          if (Convert.ToInt32(response.Attribute("count").Value) == 0)
            result = new GetCustomDomainSiteResponseData(dataCacheResponse);
          else
          {
            XElement item = (XElement)response.FirstNode;
            result = new GetCustomDomainSiteResponseData(
                dataCacheResponse,
                Convert.ToInt32(item.Attribute("privateLabelID").Value),
                Convert.ToInt32(item.Attribute("shopperPrivateLabelId").Value),
                item.Attribute("pl_customDomainSite").Value,
                (Convert.ToInt32(item.Attribute("isLegacy").Value) == 0) ? CustomDomainType.New : CustomDomainType.LegacySSL,
                CustomDomainConfiguration.GetApplicationStateById(Convert.ToInt32(item.Attribute("pl_customDomainSetupStateID").Value)),
                CustomDomainConfiguration.GetDnsHostEntryByFqdn(item.Attribute("customDomainSiteDNSURL").Value.ToLowerInvariant()),
                item.Attribute("prog_id").Value,
                item.Attribute("shopper_id").Value,
                (Convert.ToInt32(item.Attribute("isInternalDNS").Value) == 0) ? false : true);
          }

        }

      }
      catch (AtlantisException exception)
      {
        result = new GetCustomDomainSiteResponseData(exception);
      }
      catch (Exception exception)
      {
        result = new GetCustomDomainSiteResponseData(new AtlantisException(requestData, "GetCustomDomainSiteByUrl::RequestHandler()", exception.Message, exception.StackTrace));
      }

      return result;
    }
  }
}
