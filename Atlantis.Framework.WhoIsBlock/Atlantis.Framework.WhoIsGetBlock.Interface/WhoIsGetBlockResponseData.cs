using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.WhoIsGetBlock.Interface
{
    public class WhoIsGetBlockResponseData : IResponseData
    {
        private AtlantisException _exAtlantis = null;
        private int blockCountValue;

        public bool IsSuccess { get; set; }

        public int BlockCount
        {
            get { return blockCountValue; }
        }

        public WhoIsGetBlockResponseData(int blockCount)
        {
            IsSuccess = true;
            blockCountValue = blockCount;
        }

        public WhoIsGetBlockResponseData(AtlantisException exAtlantis)
        {
            IsSuccess = false;
            _exAtlantis = exAtlantis;
        }

        public WhoIsGetBlockResponseData(RequestData oRequestData, Exception ex)
        {
            IsSuccess = false;
            _exAtlantis = new AtlantisException(oRequestData, "WhoIsGetBlockResponseData", ex.Message, string.Empty);
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
