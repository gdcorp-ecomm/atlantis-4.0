﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.ManagerUser.Interface;
using Atlantis.Framework.DataCache;

namespace TestProject1
{
  [TestClass]
  public class UnitTest1
  {
    public string shopperId = "859147";
    public string userId = "mmicco";
    public string domain = "jomax";
       
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ManagerUser.Impl.dll")]
    public void GetManagerUser()
    {
      ManagerUserLookupRequestData lookupRequest = new ManagerUserLookupRequestData(
            shopperId, string.Empty,
            string.Empty, string.Empty, 0,domain, userId);
      ManagerUserLookupResponseData lookupResponse =
        (ManagerUserLookupResponseData)DataCache.GetProcessRequest(lookupRequest,65);

      Assert.IsTrue(lookupResponse.IsSuccess);    
    }
  }
}
