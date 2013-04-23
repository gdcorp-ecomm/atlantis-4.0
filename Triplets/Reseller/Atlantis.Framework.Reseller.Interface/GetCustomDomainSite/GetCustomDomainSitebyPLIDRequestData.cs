using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Reseller.Interface
{
    public class GetCustomDomainSitebyPLIDRequestData : RequestData
    {
      public const string _DATA_CACHE_METHOD_NAME = "CustomDomainSiteGetByPrivateLabelID";


        public GetCustomDomainSitebyPLIDRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLabelId)
          : base(shopperId, sourceURL, orderId, pathway, pageCount)
        {
          this.PrivateLabelId = privateLabelId;
        }

        public int PrivateLabelId { get; private set; }

        public override string GetCacheMD5()
        {
            throw new Exception("This method not supported by this object.");
        }

        public override string ToXML()
        {
            XElement dataCacheRequest = new XElement(_DATA_CACHE_METHOD_NAME);

            XElement dataCacheRequestParam2 = new XElement("param");
            dataCacheRequestParam2.Add(new XAttribute("name", "n_privateLabelID"));
            dataCacheRequestParam2.Add(new XAttribute("value", this.PrivateLabelId));
            dataCacheRequest.Add(dataCacheRequestParam2);
          
          

            return dataCacheRequest.ToString(SaveOptions.DisableFormatting);
        }
    }
}
