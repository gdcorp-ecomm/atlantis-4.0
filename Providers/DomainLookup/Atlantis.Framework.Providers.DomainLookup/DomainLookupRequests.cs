using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.DomainLookup
{
    public static class DomainLookupRequests
    {
        static DomainLookupRequests()
        {
            DomainLookupRequestType = 728;
        }

        public static int DomainLookupRequestType { get; private set; }
    }
}
