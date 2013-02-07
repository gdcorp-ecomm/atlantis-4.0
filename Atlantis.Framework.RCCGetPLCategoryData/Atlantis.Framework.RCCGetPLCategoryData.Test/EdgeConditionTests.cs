using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Engine;
using Atlantis.Framework.Interface;
using Atlantis.Framework.RCCGetPLCategoryData.Interface;
using Atlantis.Framework.RCCGetPLCategoryData.Impl;

namespace Atlantis.Framework.RCCGetPLCategoryData.Test
{
    [DeploymentItem("atlantis.config")]
    [TestClass]
    public class EdgeConditionTests
    {
        [TestMethod]
        public void TestSingleItemRequestNoPrivateLabelId()
        {
            int badPlId = -200;
            int plCategoryId = 21;
            RCCGetPLCategoryDataRequestData request = new RCCGetPLCategoryDataRequestData(badPlId, plCategoryId);
            RCCGetPLCategoryDataResponseData response = Engine.Engine.ProcessRequest(request, 649) as RCCGetPLCategoryDataResponseData;

            Assert.AreEqual(string.IsNullOrEmpty(response.PLDataQueryResponseList[0].PLCategoryData), true);
        }

        [TestMethod]
        public void TestSingleItemRequestNoPlCategoryId()
        {
            int privateLabelId = 466440;
            int badPLCategoryId = -222;
            RCCGetPLCategoryDataRequestData request = new RCCGetPLCategoryDataRequestData(privateLabelId, badPLCategoryId);
            RCCGetPLCategoryDataResponseData response = Engine.Engine.ProcessRequest(request, 649) as RCCGetPLCategoryDataResponseData;

            Assert.AreEqual(string.IsNullOrEmpty(response.PLDataQueryResponseList[0].PLCategoryData), true);
        }
    }
}
