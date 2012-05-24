using System;
using Atlantis.Framework.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.BizRegDomain.Interface
{
    public class BizRegDomainRequestData : RequestData
    {
        private string domainValue;
        private int timeoutValue;

        public BizRegDomainRequestData(
            string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
            string domain, int timeout)
            : base(shopperId, sourceUrl, orderId, pathway, pageCount)
        {
            domainValue = domain;
            timeoutValue = timeout;
        }

        public string DomainName
        {
            get { return domainValue; }
        }

        public int Timeout
        {
            get { return timeoutValue; }
        }

        public override string GetCacheMD5()
        {
            throw new Exception("BizRegDomain is not a cacheable request.");
        }
    }
}
