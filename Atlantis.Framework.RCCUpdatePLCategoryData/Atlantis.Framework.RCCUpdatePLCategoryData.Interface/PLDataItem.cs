using System;

namespace Atlantis.Framework.RCCUpdatePLCategoryData.Interface
{
    public class PLDataItem
    {
        private int _privateLabelId;
        private int _plDataCategoryId;
        private string _plData = string.Empty;
        private string _plResponse = null;

        public PLDataItem(int privateLabelId, int plDataCategoryId, string plData)
        {
            this._privateLabelId = privateLabelId;
            this._plDataCategoryId = plDataCategoryId;
            this._plData = plData;
        }

        public int PrivateLabelId
        {
            get { return this._privateLabelId; }
        }

        public int PlDataCategoryId
        {
            get { return this._plDataCategoryId; }
        }

        public string PlData
        {
            get { return this._plData; }
        }

        public string PlResponse
        {
            get { return this._plResponse; }
        }

        public void UpdateResponseStatus(string status)
        {
            this._plResponse = status;
        }
    }
}
