using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.EcommScheduleAdd.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace Atlantis.Framework.EcommScheduleAdd.Test
{
    [TestClass]
    public class ScheduleTest
    {
        [TestMethod]
        [DeploymentItem("atlantis.config")]
        public void AddValidSchedule()
        {
            string shopperId = "862548";
            string sourceUrl = "http://cart.dev.godaddy.com/basket.aspx";
            string orderId = "1454370";

            EcommScheduleAddRequestData request = new EcommScheduleAddRequestData(shopperId, sourceUrl,
                orderId, "", 1, DateTime.Now.AddDays(1), 9, "discussionText", "");
            Engine.Engine.ProcessRequest(request, 334);

        }
    }
}
