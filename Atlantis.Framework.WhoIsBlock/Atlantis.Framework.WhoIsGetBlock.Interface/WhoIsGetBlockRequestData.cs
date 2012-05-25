using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.WhoIsGetBlock.Interface
{
    public class WhoIsGetBlockRequestData : RequestData
    {
        private string ipValue;
        private int timeSpanValue;

        public string IP
        {
            get { return ipValue; }
            set { ipValue = value; }
        }

        public int TimeSpan
        {
            get { return timeSpanValue; }
            set { timeSpanValue = value; }
        }

        public WhoIsGetBlockRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pagecount,
                                        string ip, int timeSpan)
            : base(shopperId, sourceUrl, orderId, pathway, pagecount)
        {
            ipValue = ip;
            timeSpanValue = timeSpan;
        }

        public override string GetCacheMD5()
        {
            throw new Exception("WhoIsGetBlock is not a cacheable Request.");
        }
    }
}
