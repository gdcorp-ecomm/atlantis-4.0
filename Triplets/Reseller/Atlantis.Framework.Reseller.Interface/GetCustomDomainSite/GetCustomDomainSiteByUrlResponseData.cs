using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Reseller.Interface.CustomDomains;

namespace Atlantis.Framework.Reseller.Interface
{
    public class GetCustomDomainSiteByUrlResponseData : IResponseData, ICustomDomainSite
    {
        private AtlantisException _exception = null;
        private string _xmlResponse = string.Empty;
        private CustomDomainSiteRetrieveState _retrievalState = CustomDomainSiteRetrieveState.Success;

        public GetCustomDomainSiteByUrlResponseData(string xmlResponse)
        {
            this._xmlResponse = xmlResponse;
            this._retrievalState = CustomDomainSiteRetrieveState.FailedLookup;
        }

        public GetCustomDomainSiteByUrlResponseData(AtlantisException exception)
        {
            this._exception = exception;
            this._retrievalState = CustomDomainSiteRetrieveState.ExceptionOccurred;
        }

        public GetCustomDomainSiteByUrlResponseData(string xmlResponse, AtlantisException exception)
        {
            this._xmlResponse = xmlResponse;
            this._exception = exception;
            this._retrievalState = CustomDomainSiteRetrieveState.ExceptionOccurred;
        }

        public GetCustomDomainSiteByUrlResponseData(string xmlResponse, int privateLabelId, int shopperPrivateLabelId, string domainNameUrl, CustomDomainType customDomainType, ApplicationState appState, DnsEntry dnsHostEntry, string progId, string shopperId, bool isResellerDnsInternal)
        {
            try
            {
                this._xmlResponse = xmlResponse;
                this.PrivateLabelId = privateLabelId;
                this.ShopperPrivateLabelId = shopperPrivateLabelId;
                this.DomainNameUrl = domainNameUrl;
                this.Type = customDomainType;
                this.CurrentState = appState;
                this.DnsHostEntry = dnsHostEntry;
                this.ShopperId = shopperId;
                this.ProgId = progId;
                this.IsResellerDnsInternal = isResellerDnsInternal;
                this._retrievalState = CustomDomainSiteRetrieveState.Success;
            }
            catch
            {
                this._retrievalState = CustomDomainSiteRetrieveState.ExceptionOccurred;
            }
        }

        public string ToXML()
        {
            return this._xmlResponse;
        }

        public AtlantisException GetException()
        {
            return this._exception;
        }

        public CustomDomainSiteRetrieveState ResponseState
        {
            get { return this._retrievalState; }
        }

        public int PrivateLabelId { get; private set; }

        public int ShopperPrivateLabelId { get; private set; }

        public string DomainNameUrl { get; private set; }

        public CustomDomainType Type { get; private set; }

        public ApplicationState CurrentState { get; private set; }

        public DnsEntry DnsHostEntry { get; private set; }

        public string ProgId { get; private set; }

        public string ShopperId { get; private set; }

        public bool IsResellerDnsInternal { get; private set; }
    }
}
