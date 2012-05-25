using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.WhoIsStatsLogger.Interface
{
    public class WhoIsStatsLoggerRequestData : RequestData
    {
        private int statsLogIDValue = 0;
        private string hostNameValue = string.Empty;
        private string clientPlatformValue = string.Empty;
        private string browserValue = string.Empty;
        private string domainNameValue = string.Empty;
        private string topLevelDomainValue = string.Empty;
        private int privateLabelIDValue = 0;
        private DateTime startTimeValue = DateTime.Now;
        private DateTime endTimeValue = DateTime.Now;
        private int elapsedTimeValue = 0;
        private int detailThresholdValue = 0;
        private bool isDetailAvailableValue = false;
        private bool isDetailForcedValue = false;
        private bool isBusinessRegistrationValue = false;
        private bool isInternalIPValue = false;
        private DateTime createDateValue = DateTime.Now;
        private string resultCodeValue = string.Empty;

        public int StatsLogId
        {
            get { return statsLogIDValue; }
            set { statsLogIDValue = value; }
        }

        public string HostName
        {
            get { return hostNameValue; }
            set { hostNameValue = value; }
        }

        public string ClientPlatform
        {
            get { return clientPlatformValue; }
            set { clientPlatformValue = value; }
        }

        public string Browser
        {
            get { return browserValue; }
            set { browserValue = value; }
        }

        public string DomainName
        {
            get { return domainNameValue; }
            set { domainNameValue = value; }
        }

        public string TopLevelDomain
        {
            get { return topLevelDomainValue; }
            set { topLevelDomainValue = value; }
        }

        public int PrivateLabelId
        {
            get { return privateLabelIDValue; }
            set { privateLabelIDValue = value; }
        }

        public DateTime StartTime
        {
            get { return startTimeValue; }
            set { startTimeValue = value; }
        }

        public DateTime EndTime
        {
            get { return endTimeValue; }
            set { endTimeValue = value; }
        }

        public int ElapsedTime
        {
            get { return elapsedTimeValue; }
            set { elapsedTimeValue = value; }
        }

        public int DetailThreshold
        {
            get { return detailThresholdValue; }
            set { detailThresholdValue = value; }
        }

        public bool IsDetailAvailable
        {
            get { return isDetailAvailableValue; }
            set { isDetailAvailableValue = value; }
        }

        public bool IsDetailForced
        {
            get { return isDetailForcedValue; }
            set { isDetailForcedValue = value; }
        }

        public bool IsBusinessRegistration
        {
            get { return isBusinessRegistrationValue; }
            set { isBusinessRegistrationValue = value; }
        }

        public bool IsInternalIp
        {
            get { return isInternalIPValue; }
            set { isInternalIPValue = value; }
        }

        public DateTime CreateDate
        {
            get { return createDateValue; }
            set { createDateValue = value; }
        }

        public string ResultCode
        {
            get { return resultCodeValue; }
            set { resultCodeValue = value; }
        }

        public WhoIsStatsLoggerRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
            string hostName, string clientPlatform, string browser, string domainName, string tld, int plID,
                DateTime startTime, DateTime endTime, int elapsedTime, byte detailThreshold, bool detailAvail,
                bool detailForced, bool bizReg, bool internalIP, string resultCode)
            : base(shopperId, sourceUrl, orderId, pathway, pageCount)
        {
            hostNameValue = hostName;
            clientPlatformValue = clientPlatform;
            browserValue = browser;
            domainNameValue = domainName;
            topLevelDomainValue = tld;
            privateLabelIDValue = plID;
            startTimeValue = startTime;
            endTimeValue = endTime;
            elapsedTimeValue = elapsedTime;
            detailThresholdValue = detailThreshold;
            isDetailAvailableValue = detailAvail;
            isDetailForcedValue = detailForced;
            IsBusinessRegistration = bizReg;
            isInternalIPValue = internalIP;
            resultCodeValue = resultCode;
        }

        public override string GetCacheMD5()
        {
            throw new Exception("WhoIsStatsLogger is not a cacheable Request.");
        }
    }
}
