using System;

namespace Atlantis.Framework.RCCGetPLCategoryData.Interface
{
    public class PLCategoryDataItemResponse
    {
        public PLCategoryDataItemResponse() { }

        public PLCategoryDataItemResponse(PLCategoryDataItemRequest request)
        {
            this._request = request;
        }

        public PLCategoryDataItemResponse(PLCategoryDataItemRequest request, string plCategoryData)
        {
            this._request = request;
            this._plCategoryData = plCategoryData;
            this._isSuccess = true;
        }

        public PLCategoryDataItemResponse(PLCategoryDataItemRequest request, string plCategoryData, bool isSuccess)
        {
            this._request = request;
            this._plCategoryData = plCategoryData;
            this._isSuccess = isSuccess;
        }

        private PLCategoryDataItemRequest _request = null;
        public PLCategoryDataItemRequest Request
        {
            get { return this._request; }
            set { this._request = value; }
        }

        private string _plCategoryData = null;
        public string PLCategoryData
        {
            get { return this._plCategoryData; }
            set { this._plCategoryData = value; }
        }

        private bool _isSuccess = true;
        public bool IsSuccess
        {
            get { return this._isSuccess; }
            set { this._isSuccess = value; }
        }
    }
}
