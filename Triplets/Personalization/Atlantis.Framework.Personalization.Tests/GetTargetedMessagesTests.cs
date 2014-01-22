using System.Collections.Generic;
using System.Diagnostics;
using Atlantis.Framework.Personalization.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Atlantis.Framework.Personalization.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>

  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Personalization.Impl.dll")]
  [TestClass]
  public class GetTargetedMessageTests
  {
    [TestMethod]
    public void GetTargetedMessagesWithoutISCOrAnonId()
    {
      const string appId = "2";
      const string interactionPoint = "ProductUpsell";

      TargetedMessagesRequestData request = new TargetedMessagesRequestData("12345", "1", appId, interactionPoint, string.Empty, string.Empty);
      TargetedMessagesResponseData response = (TargetedMessagesResponseData)Engine.Engine.ProcessRequest(request, 669);
      Debug.Write(response.ToXML());
      Assert.IsTrue(response.TargetedMessagesData.ResultCode == 0);
    }

    [TestMethod]
    public void GetTargetedMessagesWithoutShopperId()
    {
      const string appId = "2";
      const string interactionPoint = "ProductUpsell";

      TargetedMessagesRequestData request = new TargetedMessagesRequestData(string.Empty, "1", appId, interactionPoint, string.Empty, string.Empty);
      TargetedMessagesResponseData response = (TargetedMessagesResponseData)Engine.Engine.ProcessRequest(request, 669);
      Debug.Write(response.ToXML());
      Assert.IsTrue(response.TargetedMessagesData.ResultCode == 0);
    }

    [TestMethod]
    public void GetTargetedMessagesWithISC()
    {
      const string appId = "2";
      const string interactionPoint = "ProductUpsell";

      TargetedMessagesRequestData request = new TargetedMessagesRequestData(string.Empty, "1", appId, interactionPoint, Guid.NewGuid().ToString(), "test");
      TargetedMessagesResponseData response = (TargetedMessagesResponseData)Engine.Engine.ProcessRequest(request, 669);

      Debug.Write(response.ToXML());
      Assert.IsTrue(response.TargetedMessagesData.ResultCode == 0);
    }
  }
}
