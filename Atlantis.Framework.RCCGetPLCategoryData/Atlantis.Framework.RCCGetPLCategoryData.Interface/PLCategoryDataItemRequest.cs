using System;

namespace Atlantis.Framework.RCCGetPLCategoryData.Interface
{
    public class PLCategoryDataItemRequest
    {
        public PLCategoryDataItemRequest() { }

        public PLCategoryDataItemRequest(int privateLabelId, int plCategoryId)
        {
            this._privateLabelId = privateLabelId;
            this._plCategoryId = plCategoryId;
        }

        private int _privateLabelId;
        public int PrivateLabelId
        {
            get { return this._privateLabelId; }
            set { this._privateLabelId = value; }
        }

        private int _plCategoryId;
        public int PLCategoryId
        {
            get { return this._plCategoryId; }
            set { this._plCategoryId = value; }
        }
    }
}
