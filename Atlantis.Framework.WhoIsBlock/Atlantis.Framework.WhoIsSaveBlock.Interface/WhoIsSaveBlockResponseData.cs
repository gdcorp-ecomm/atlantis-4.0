using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.WhoIsSaveBlock.Interface
{
    public class WhoIsSaveBlockResponseData : IResponseData
    {
        private AtlantisException _exAtlantis;

        public bool IsSuccess { get; set; }

        public WhoIsSaveBlockResponseData()
        {
            IsSuccess = true;
        }

        public WhoIsSaveBlockResponseData(AtlantisException exAtlantis)
        {
            IsSuccess = false;
            _exAtlantis = exAtlantis;
        }

        public WhoIsSaveBlockResponseData(RequestData oRequestData, Exception ex)
        {
            IsSuccess = false;
            _exAtlantis = new AtlantisException(oRequestData, "WhoIsSaveBlockResponseData", ex.Message, string.Empty);
        }

        public string ToXML()
        {
            return string.Empty;
        }

        public AtlantisException GetException()
        {
            return _exAtlantis;
        }

    }
}
