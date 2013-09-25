using Atlantis.Framework.Brand.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Brand.Impl;
using System.Xml.Linq;
using System.Collections.Generic;

namespace Atlantis.Framework.Brand.Tests
{
    [TestClass]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.Brand.Impl.dll")]
    public class ProductLineNamesTest
    {
        private const int GDContextID = 1;
        private const int PLContextID = 6;
        private const int PrivateLabelID = 1724;

        [TestMethod]
        public void SimpleGetProductLineNames()
        {
            ProductLineNameRequestData request = new ProductLineNameRequestData(GDContextID);

            ProductLineNameResponseData response = (ProductLineNameResponseData)Engine.Engine.ProcessRequest(request, 727);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void ProductLineNameRequestDataProperties()
        {
            ProductLineNameRequestData request = new ProductLineNameRequestData(PLContextID);

            Assert.AreEqual(PLContextID, request.ContextId);
            XElement.Parse(request.ToXML());

            Assert.AreEqual("6", request.GetCacheMD5());
        }

        [TestMethod]
        public void ProductLineNameRequestDataCacheKey()
        {
            ProductLineNameRequestData request1 = new ProductLineNameRequestData(GDContextID);
            ProductLineNameRequestData request2 = new ProductLineNameRequestData(PLContextID);
            ProductLineNameRequestData request3 = new ProductLineNameRequestData(PLContextID);

            Assert.AreNotEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
            Assert.AreEqual(request2.GetCacheMD5(), request3.GetCacheMD5());
        }

        [TestMethod]
        public void ProductLineNameResponseDataProperties()
        {
            var productNameDictionary = new Dictionary<string, string>();
            productNameDictionary.Add("FreeEmailForwarding", "100-Pack Email Forwarding");

            var productLineList = new Dictionary<string, Dictionary<string, string>>();
            productLineList.Add("ProductLineName", productNameDictionary);

            ProductLineNameRequestData request = new ProductLineNameRequestData(GDContextID);

            ProductLineNameResponseData response = (ProductLineNameResponseData)Engine.Engine.ProcessRequest(request, 727);                       

            XElement.Parse(response.ToXML());

            Assert.AreEqual(response.GetName("FreeEmailForwarding", 1), "100-Pack Email Forwarding");

        }

    }
}
