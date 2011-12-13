using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CRMTaskCreation.Interface
{
    public class CRMTaskCreationRequestData : RequestData
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ClientId { get; set; }

        public CRMTaskCreationRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
            DateTime startDate, DateTime endDate, int clientId)
            : base(shopperId, sourceUrl, orderId, pathway, pageCount)
        {
            StartDate = startDate;
            EndDate = endDate;
            ClientId = clientId;
            RequestTimeout = TimeSpan.FromSeconds(4);
        }

        public override string GetCacheMD5()
        {
            throw new NotImplementedException();
        }

        public string ToXml()
        {
            var xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("TaskProperties");
            XmlNode shopperId = xmlDoc.CreateElement("shopper_id");
            shopperId.InnerText = ShopperID;
            rootNode.AppendChild(shopperId);
            XmlNode orderId = xmlDoc.CreateElement("order_id");
            orderId.InnerText = OrderID;
            rootNode.AppendChild(orderId);
            XmlNode activeDate = xmlDoc.CreateElement("activeDate");
            activeDate.InnerText = StartDate.ToString();
            rootNode.AppendChild(activeDate);
            XmlNode activeEndDate = xmlDoc.CreateElement("activeEndDate");
            activeEndDate.InnerText = EndDate.ToString();
            rootNode.AppendChild(activeEndDate);
            XmlNode resourceId = xmlDoc.CreateElement("resourceID");
            resourceId.InnerText = OrderID;
            rootNode.AppendChild(resourceId);
            return rootNode.OuterXml;
        }
    }

    

}
