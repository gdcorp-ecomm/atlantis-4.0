using System;
using System.Collections.Generic;
using Atlantis.Framework.RCCGetPLCategoryData.Interface;
using Atlantis.Framework.RCCGetPLCategoryData.Impl;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.RCCGetPLCategoryData.Test
{
    [TestClass]
    [DeploymentItem("atlantis.config")]
    public class HappyPathTests
    {
        [TestMethod]
        public void TestSingleItemRequest()
        {
            int privateLabelId = 466440;
            int plCategoryId = 21;
            string knownPlDataValue = "1";

            RCCGetPLCategoryDataRequestData request = new RCCGetPLCategoryDataRequestData(privateLabelId, plCategoryId);
            RCCGetPLCategoryDataResponseData response = Engine.Engine.ProcessRequest(request, 649) as RCCGetPLCategoryDataResponseData;

            Assert.AreEqual(response.PLDataQueryResponseList[0].PLCategoryData.ToLowerInvariant(), knownPlDataValue);
        }

        [TestMethod]
        public void TestMultipleItemRequest()
        {
            int privateLabelId1, privateLabelId2;
            int plCategoryId;
            string knownDataValue = "1";

            privateLabelId1 = 466440;
            privateLabelId2 = 441166;
            plCategoryId = 21;

            PLCategoryDataItemRequest item1 = new PLCategoryDataItemRequest(privateLabelId1, plCategoryId);
            PLCategoryDataItemRequest item2 = new PLCategoryDataItemRequest(privateLabelId2, plCategoryId);
            List<PLCategoryDataItemRequest> requestList = new List<PLCategoryDataItemRequest>();
            requestList.Add(item1);
            requestList.Add(item2);

            RCCGetPLCategoryDataRequestData request = new RCCGetPLCategoryDataRequestData(requestList);
            RCCGetPLCategoryDataResponseData response = Engine.Engine.ProcessRequest(request, 649) as RCCGetPLCategoryDataResponseData;

            Assert.AreEqual(response.PLDataQueryResponseList[0].PLCategoryData.ToLowerInvariant(), knownDataValue);
            Assert.AreEqual(response.PLDataQueryResponseList[1].PLCategoryData.ToLowerInvariant(), knownDataValue);
        }
    }
}
