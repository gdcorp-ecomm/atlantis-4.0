using System;
using Atlantis.Framework.VanityHost.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Specialized;

namespace Atlantis.Framework.VanityHost.Tests
{
  [TestClass]
  public class VanityHostTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("VanityHost.xml")]
    [DeploymentItem("Atlantis.Framework.VanityHost.Impl.dll")]
    public void GetVanityHosts()
    {
      VanityHostRequestData request = new VanityHostRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      VanityHostResponseData response = (VanityHostResponseData)DataCache.DataCache.GetProcessRequest(request, 526);

      Assert.IsNotNull(response.VanityHostItems);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("VanityHost.xml")]
    [DeploymentItem("Atlantis.Framework.VanityHost.Impl.dll")]
    public void SetQueryItems()
    {
      VanityHostRequestData request = new VanityHostRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      VanityHostResponseData response = (VanityHostResponseData)DataCache.DataCache.GetProcessRequest(request, 526);

      foreach (VanityHostItem item in response.VanityHostItems)
      {
        NameValueCollection myValues = new NameValueCollection();
        myValues["myItemThatshouldntmatch"] = "myvalues";
        item.SetQueryItems(myValues);

        if (item.HasQueryItems)
        {
          Assert.IsTrue(myValues.Count > 1);
        }
        else
        {
          Assert.AreEqual(1, myValues.Count);
        }
      }
    }
  }
}
