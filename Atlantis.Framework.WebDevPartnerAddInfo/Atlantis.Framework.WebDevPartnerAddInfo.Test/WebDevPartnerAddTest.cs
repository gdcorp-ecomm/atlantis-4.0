using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.WebDevPartnerAddInfo.Interface;

namespace Atlantis.Framework.WebDevPartnerAddInfo.Test
{
  [TestClass]
  public class WebDevPartnerAddTest
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AddWebDevInfo()
    {
      string shopperId = "123457";
      string sourceUrl = "http://www.godaddy.com/business/certified-web-developer.aspx";
      string hashKey = "123aaa456bbb789ddd";
      string applicationType = "contact";
      string applicationState = "new";
      WebDevPartnerAddInfoRequestData request = new WebDevPartnerAddInfoRequestData(shopperId, sourceUrl, "0", "", 1, hashKey, applicationType, applicationState);
      WebDevPartnerAddInfoResponseData response = (WebDevPartnerAddInfoResponseData)Engine.Engine.ProcessRequest(request, 574);
    }
  }
}
