using System;

namespace Atlantis.Framework.Reseller.Interface.CustomDomains
{
    public interface ICustomDomainSite
    {
        /// <summary>
        /// The PrivateLabelID (or PLID) of the reseller associated with the custom domain.
        /// </summary>
        int PrivateLabelId { get; }

        /// <summary>
        /// The parent company private label ID (defaults to Go Daddy)
        /// </summary>
        int ShopperPrivateLabelId { get; }

        /// <summary>
        /// The domain name used to implement this custom domain.
        /// </summary>
        string DomainNameUrl { get; }

        /// <summary>
        /// The type of custom domain reseller for the account. LegacySSL is reserved for internal reseller accounts
        /// as well as the top 200 reseller custom domain deployment that all use SSL certificates. Anything else
        /// moving forward should be using the "NEW" enum value.
        /// </summary>
        CustomDomainType Type { get; }

        /// <summary>
        /// The current setup state of the custom domain - primarily used by provisioning service to process accounts
        /// in various states.
        /// </summary>
        ApplicationState CurrentState { get;}

        /// <summary>
        /// The dns entry assigned to the custom domain account that the reseller should have their dns updated to 
        /// point to in order for the CDRWEB proxy servers to serve up the content.
        /// </summary>
        DnsEntry DnsHostEntry { get; }

        /// <summary>
        /// The resellers ProgID (program ID) associated with the account.
        /// </summary>
        string ProgId { get; }

        /// <summary>
        /// The ID of the shopper who owns this reseller account;
        /// </summary>
        string ShopperId { get; }

        /// <summary>
        /// Identifies that the domain name that will be used for the custom domain account is under GoDaddy's DNS
        /// management, allowing the provisioning process to automatically make updates accordingly during the setup
        /// process for the account.
        /// </summary>
        bool IsResellerDnsInternal { get; }
    }
}
