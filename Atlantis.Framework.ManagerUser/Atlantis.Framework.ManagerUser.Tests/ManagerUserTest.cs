using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.ManagerUser.Interface;
using Atlantis.Framework.DataCache;

namespace TestProject1
{
  [TestClass]
  public class UnitTest1
  {
    public string shopperId = "859147";
    public string userId = "mbenzel";
    public string domain = "jomax";
       
    [TestMethod]
    [DeploymentItem("atlantis.config")]
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
