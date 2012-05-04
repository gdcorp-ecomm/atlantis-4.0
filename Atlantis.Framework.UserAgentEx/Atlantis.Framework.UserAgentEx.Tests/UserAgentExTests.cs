﻿using System;
using Atlantis.Framework.UserAgentEx.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.UserAgentEx.Tests
{
  [TestClass]
  public class UserAgentExTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("Atlantis.Framework.UserAgentEx.Impl.dll")]
    public void SearchEngineBotExpressions()
    {
      UserAgentExRequestData request = new UserAgentExRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 10);
      UserAgentExResponseData response = (UserAgentExResponseData)DataCache.DataCache.GetProcessRequest(request, 528);

      string testAgent = "Mozilla/5.0+(compatible;+bingbot/2.0;++http://www.bing.com/bingbot.htm)";
      Assert.IsTrue(response.IsMatch(testAgent));
    }
  }
}
