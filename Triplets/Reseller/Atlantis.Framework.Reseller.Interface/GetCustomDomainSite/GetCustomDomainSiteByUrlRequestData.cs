using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Reseller.Interface
{
    public class GetCustomDomainSiteByUrlRequestData : RequestData
    {
        public const string _DATA_CACHE_METHOD_NAME = "CustomDomainSiteGetbyPL_customDomainSite";

        public GetCustomDomainSiteByUrlRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string customDomainUrl)
            : base(shopperId, sourceURL, orderId, pathway, pageCount)
        {
            this.CustomDomainUrl = customDomainUrl ?? string.Empty;
        }

        public string CustomDomainUrl { get; private set; }

        public override string GetCacheMD5()
        {
            throw new Exception("This method not supported by this object.");
        }

        public override string ToXML()
        {
            XElement dataCacheRequest = new XElement(_DATA_CACHE_METHOD_NAME);
            XElement dataCacheRequestParam = new XElement("param");
            dataCacheRequestParam.Add(new XAttribute("name", "s_pl_customDomainSite"));
            dataCacheRequestParam.Add(new XAttribute("value", this.CustomDomainUrl));
            dataCacheRequest.Add(dataCacheRequestParam);

            return dataCacheRequest.ToString(SaveOptions.DisableFormatting);
        }
    }
}
