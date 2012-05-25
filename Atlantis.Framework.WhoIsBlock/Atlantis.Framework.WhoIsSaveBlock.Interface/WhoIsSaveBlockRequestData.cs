using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.WhoIsSaveBlock.Interface
{
    public class WhoIsSaveBlockRequestData : RequestData
    {
        private string ipValue;
        private string pageValue;

        public string IP
        {
            get { return ipValue; }
        }

        public string Page
        {
            get { return pageValue; }
        }

        public WhoIsSaveBlockRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pagecount,
                                         string ip, string page)
            : base(shopperId, sourceUrl, orderId, pathway, pagecount)
        {
            ipValue = ip;
            pageValue = page;
        }

        public override string GetCacheMD5()
        {
            throw new Exception("WhoIsSaveBlock is not a cacheable Request.");
        }

    }
}
