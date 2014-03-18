using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Domains.Interface
{
    public static class DomainCheckResultCodes
    {
        public const int Unknown = 0;
        public const int Available = 1000;
        public const int Unavailable = 1001;
        public const int Invalid = 1002;
        public const int RRP = 1003;
        public const int EPP = 1004;
        public const int InvalidSldTld = 2000;
        public const int BannedWords = 2001;
        public const int BlockedDomainName = 2002;
    }
}
