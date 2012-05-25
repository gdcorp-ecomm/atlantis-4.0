using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProximaDomainName.Interface
{
    public class ProximaDomainNameRequestData : RequestData
    {
        private string _domain;
        private int _timeout;

        public string Domain
        {
            get { return _domain; }
        }
        public int Timeout
        {
            get { return _timeout; }
        }

        public ProximaDomainNameRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
            string domain, int timeout)
            : base(shopperId, sourceUrl, orderId, pathway, pageCount)
        {
            _domain = domain;
            _timeout = timeout;
        }

        public override string GetCacheMD5()
        {
            throw new Exception("ProximaDomainName is not a cacheable request.");
        }
    }
}