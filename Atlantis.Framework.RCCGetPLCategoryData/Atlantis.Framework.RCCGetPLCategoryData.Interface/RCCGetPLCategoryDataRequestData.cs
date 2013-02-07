using System;
using System.Xml.Linq;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RCCGetPLCategoryData.Interface
{
    // ATLANTIS CONFIG ITEM 649 //
    public class RCCGetPLCategoryDataRequestData : RequestData
    {
        private List<PLCategoryDataItemRequest> _requests;

        public RCCGetPLCategoryDataRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pagecount)
            : base(shopperId, sourceUrl, orderId, pathway, pagecount) 
        {
            this._requests = new List<PLCategoryDataItemRequest>();
        }

        public RCCGetPLCategoryDataRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pagecount, int privateLabelId, int plCategoryId)
            : base(shopperId, sourceUrl, orderId, pathway, pagecount)
        {
            this._requests = new List<PLCategoryDataItemRequest>();
            this._requests.Add( new PLCategoryDataItemRequest(privateLabelId, plCategoryId));
        }

        public RCCGetPLCategoryDataRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pagecount, List<PLCategoryDataItemRequest> requestList)
            : base(shopperId, sourceUrl, orderId, pathway, pagecount)
        {
            this._requests = new List<PLCategoryDataItemRequest>();
            this._requests = requestList;
        }

        public RCCGetPLCategoryDataRequestData(int privateLabelId, int plCategoryId)
            : base(string.Empty, string.Empty, string.Empty, string.Empty, 0)
        {
            this._requests = new List<PLCategoryDataItemRequest>();
            this._requests.Add(new PLCategoryDataItemRequest(privateLabelId, plCategoryId));
        }

        public RCCGetPLCategoryDataRequestData(List<PLCategoryDataItemRequest> requestList)
            : base(string.Empty, string.Empty, string.Empty, string.Empty, 0)
        {
            this._requests = new List<PLCategoryDataItemRequest>();
            this._requests = requestList;
        }

        public List<PLCategoryDataItemRequest> PLCategoryDataRequests
        {
            get { return this._requests; }
        }

        public void AddItem(PLCategoryDataItemRequest item)
        {
            this._requests.Add(item);
        }

        public void RemoveItem(PLCategoryDataItemRequest itemToRemove)
        {
            this._requests.Remove(itemToRemove);
        }

        public override string GetCacheMD5()
        {
            throw new Exception("This triplet does not support feature.");
        }

        public override string ToXML()
        {
            XElement rootElement = new XElement("request");
            foreach (PLCategoryDataItemRequest itemRequest in this._requests)
            {
                XElement item = new XElement("plCategoryDataItemRequest");
                item.Add(new XAttribute("privateLabelId", itemRequest.PrivateLabelId));
                item.Add(new XAttribute("plCategoryId", itemRequest.PLCategoryId));

                rootElement.Add(item);
            }

            return rootElement.ToString();
        }
    }
}
