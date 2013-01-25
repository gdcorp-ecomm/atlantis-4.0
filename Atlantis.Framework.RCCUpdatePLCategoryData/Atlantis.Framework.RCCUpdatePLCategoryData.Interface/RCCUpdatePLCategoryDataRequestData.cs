using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RCCUpdatePLCategoryData.Interface
{
    public class RCCUpdatePLCategoryDataRequestData : RequestData
    {
        private Dictionary<string, PLDataItem> _plDataUpdateList;

        public RCCUpdatePLCategoryDataRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
            : base(shopperId, sourceUrl, orderId, pathway, pageCount)
        {
            this._plDataUpdateList = new Dictionary<string,PLDataItem>();
        }

        public RCCUpdatePLCategoryDataRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int privateLabelId, int plCategoryId, string plCategoryData)
            : base(shopperId, sourceUrl, orderId, pathway, pageCount)
        {
            this._plDataUpdateList = new Dictionary<string,PLDataItem>();
            this._plDataUpdateList.Add(privateLabelId.ToString() + plCategoryId.ToString(), new PLDataItem(privateLabelId, plCategoryId, plCategoryData));
        }

        public RCCUpdatePLCategoryDataRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, List<PLDataItem> plDataToUpdate)
            : base(shopperId, sourceUrl, orderId, pathway, pageCount)
        {
            this._plDataUpdateList = new Dictionary<string, PLDataItem>();
            foreach (PLDataItem item in plDataToUpdate)
            {
                this._plDataUpdateList.Add(item.PrivateLabelId.ToString() + item.PlDataCategoryId.ToString(), item);
            }
        }

        public List<PLDataItem> PlDataItems
        {
            get { return this._plDataUpdateList.Values.ToList<PLDataItem>(); }
        }

        public void AddPlDataUpdateItem(int privateLabelId, int plDataCategoryId, string plData)
        {
            if (!this._plDataUpdateList.ContainsKey(privateLabelId.ToString() + plDataCategoryId.ToString()))
                this._plDataUpdateList.Add(privateLabelId.ToString() + plDataCategoryId.ToString(),  new PLDataItem(privateLabelId, plDataCategoryId, plData));
        }

        public void RemovePlDataUpdateItem(PLDataItem itemToRemove)
        {
            if (this._plDataUpdateList.ContainsKey(itemToRemove.PrivateLabelId.ToString() + itemToRemove.PlDataCategoryId.ToString()))
                this._plDataUpdateList.Remove(itemToRemove.PrivateLabelId.ToString() + itemToRemove.PlDataCategoryId.ToString());
        }

        public override string GetCacheMD5()
        {
            throw new Exception("Caching is not supported by this request");
        }

        public override string ToXML()
        {
            XElement request = new XElement("request");

            foreach (PLDataItem item in this._plDataUpdateList.Values.ToList<PLDataItem>())
            {
                XElement xitem = new XElement("plupdateitem");
                xitem.Add(new XAttribute("privateLabelId", item.PrivateLabelId));
                xitem.Add(new XAttribute("plDataCategoryId", item.PlDataCategoryId));
                xitem.Add(new XAttribute("plData", item.PlData ?? string.Empty));

                request.Add(xitem);
            }

            return request.ToString();
        }
    }
}
