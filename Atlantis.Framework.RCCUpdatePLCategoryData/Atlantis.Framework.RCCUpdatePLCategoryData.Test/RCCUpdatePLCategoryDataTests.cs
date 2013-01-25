using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Engine;
using Atlantis.Framework.RCCUpdatePLCategoryData.Interface;
using Atlantis.Framework.RCCUpdatePLCategoryData.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.RCCUpdatePLCategoryData.Test
{
    [DeploymentItem("atlantis.config")]
    public class RCCUpdatePLCategoryDataTests
    {
        private int _testRequestType = 646;

        [TestMethod]
        public void TestUpdatePlDataHappyPath()
        {
            var pldata = new RCCUpdatePLCategoryDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 466440, 240, "/RCC/PortraitTemplates/Reseller_Woman1.gif");
            pldata.AddPlDataUpdateItem(466440, 240, "test test");
            pldata.AddPlDataUpdateItem(466440, 240, "/RCC/PortraitTemplates/Reseller_Woman1.gif");
            var response = Engine.Engine.ProcessRequest(pldata, _testRequestType) as RCCUpdatePLCategoryDataResponseData;
            Assert.AreEqual<UpdatePLCategoryDataResponseType>(UpdatePLCategoryDataResponseType.Success, response.IsSuccessful());
        }

        [TestMethod]
        public void TestUpdatePlDataPartialSuccess()
        {
            var pldata = new RCCUpdatePLCategoryDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 466440, 240, "/RCC/PortraitTemplates/Reseller_Woman1.gif");
            pldata.AddPlDataUpdateItem(9999999, 240, "test test");
            pldata.AddPlDataUpdateItem(466440, 99999999, "/RCC/PortraitTemplates/Reseller_Woman1.gif");
            var response = Engine.Engine.ProcessRequest(pldata, _testRequestType) as RCCUpdatePLCategoryDataResponseData;
            Assert.AreEqual<UpdatePLCategoryDataResponseType>(UpdatePLCategoryDataResponseType.PartialSuccess, response.IsSuccessful());
        }

        [TestMethod]
        public void TestUpdatePlDataNoReseller()
        {
            var pldata = new RCCUpdatePLCategoryDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 99999999, 240, "/RCC/PortraitTemplates/Reseller_Woman1.gif");
            var response = Engine.Engine.ProcessRequest(pldata, _testRequestType) as RCCUpdatePLCategoryDataResponseData;
            Assert.AreEqual<UpdatePLCategoryDataResponseType>(UpdatePLCategoryDataResponseType.Failure, response.IsSuccessful());
        }

        [TestMethod]
        public void TestUpdatePlDataNoCategoryId()
        {
            var pldata = new RCCUpdatePLCategoryDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 466440, 99999999, "/RCC/PortraitTemplates/Reseller_Woman1.gif");
            var response = Engine.Engine.ProcessRequest(pldata, _testRequestType) as RCCUpdatePLCategoryDataResponseData;
            Assert.AreEqual<UpdatePLCategoryDataResponseType>(UpdatePLCategoryDataResponseType.Failure, response.IsSuccessful());
        }
    }
}
