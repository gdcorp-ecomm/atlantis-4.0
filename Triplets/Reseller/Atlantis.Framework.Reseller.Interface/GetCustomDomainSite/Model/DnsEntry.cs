using System;

namespace Atlantis.Framework.Reseller.Interface.CustomDomains
{
    public struct DnsEntry
    {
        public DnsEntry(string fqdn, DnsRecordType recordType, bool isAssignable)
        {
            FQDN = fqdn;
            RecordType = recordType;
            IsAssignable = isAssignable;
        }

        /// <summary>
        /// The fully-qualified-domain-name of the DNS record URL that is (1) either used and available to assign to a new 
        /// custom domain account under the "NEW" CustomDomainType paradigm or (2) is in use for the custom domain
        /// account under the "LegacySSL" CustomDomainType paradigm. NOTE: Under the "NEW" paradigm, there is a 1-to-many
        /// relationship between the FQDN and the custom domain reseller site, while under the "LegacySSL" paradigm,
        /// there is a 1-to-1 relationship (external VIP for each account) due to SSL requirements.
        /// </summary>
        public string FQDN;

        /// <summary>
        /// The type of DNS entry created in the reseller's DNS management interface. **NOTE: "ARecord"s were only used
        /// for the "LegacySSL" paradigm to allow the internal resellers and top 200 resellers to point their root site
        /// at the custom domain proxy servers. The "CName" entries will be used for all custom domain sites created
        /// moving forward.
        /// </summary>
        public DnsRecordType RecordType;

        /// <summary>
        /// Identifies whether the FQDN is assignable out to resellers or not. **NOTE: All FQDNs assigned out under the "LegacySSL"
        /// paradigm will always reports as "Not Assignable".
        /// </summary>
        public bool IsAssignable;
    }
}
