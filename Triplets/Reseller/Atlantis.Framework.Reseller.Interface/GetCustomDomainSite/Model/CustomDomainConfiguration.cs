using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Reseller.Interface.CustomDomains
{
    public enum CustomDomainType
    {
        LegacySSL = 1,
        New = 2
    }

    public enum DnsRecordType
    {
        ARecord = 1,
        CName = 2
    }

    public enum CustomDomainSiteRetrieveState
    {
        Success = 0,
        FailedLookup = 1,
        ExceptionOccurred = 2
    }

    public static class CustomDomainConfiguration
    {
        private static Dictionary<int, ApplicationState> _appStateList = null;
        private static Dictionary<string, DnsEntry> _dnsHostEntryList = null;

        public static class ApplicationStateIds
        {
            public static readonly int NewRequest = 0;
            public static readonly int PendingInternalDns = 1;
            public static readonly int PendingVerification = 2;
            public static readonly int Setup = 3;
            public static readonly int PendingDelete = 4;
            public static readonly int Deleted = 5;
            public static readonly int InternalException = 6;
            public static readonly int ExternalException = 7;
        }

        public static ApplicationState GetApplicationStateById(int appStateId)
        { 
            if (_appStateList == null)
            {
                _appStateList = new Dictionary<int,ApplicationState>();
                _appStateList.Add(0, new ApplicationState(0, 1, "New Request"));
                _appStateList.Add(1, new ApplicationState(1, 2, "Pending Internal Dns"));
                _appStateList.Add(2, new ApplicationState(2, 3, "Pending Verification"));
                _appStateList.Add(3, new ApplicationState(3, 4, "Setup"));
                _appStateList.Add(4, new ApplicationState(4, 5, "Pending Delete"));
                _appStateList.Add(5, new ApplicationState(5, 6, "Deleted"));
                _appStateList.Add(6, new ApplicationState(6, 7, "Internal Exception"));
                _appStateList.Add(7, new ApplicationState(7, 8, "External Exception"));
            }

            try
            {
                return (_appStateList.ContainsKey(appStateId)) ? _appStateList[appStateId] : _appStateList[ApplicationStateIds.InternalException];
            }
            catch
            {
                return _appStateList[ApplicationStateIds.InternalException];
            }
        }

        public static DnsEntry GetDnsHostEntryByFqdn(string fqdn)
        {
            if (_dnsHostEntryList == null)
            {
                _dnsHostEntryList = new Dictionary<string, DnsEntry>();
#if DEBUG
                _dnsHostEntryList.Add("cdrapplication.dev.int.securepaynet.net", new DnsEntry("cdrapplication.dev.int.securepaynet.net", DnsRecordType.CName, true));
#else
                _dnsHostEntryList.Add("cdrapplication.securepaynet.net", new DnsEntry("cdrapplication.securepaynet.net", DnsRecordType.CName, true));
#endif
           }

            if (!_dnsHostEntryList.ContainsKey(fqdn.Trim().ToLowerInvariant()))
                _dnsHostEntryList.Add(fqdn.Trim().ToLowerInvariant(), new DnsEntry(fqdn.Trim().ToLowerInvariant(), DnsRecordType.CName, false));

            return _dnsHostEntryList[fqdn.Trim().ToLowerInvariant()];
        }

    }
}
