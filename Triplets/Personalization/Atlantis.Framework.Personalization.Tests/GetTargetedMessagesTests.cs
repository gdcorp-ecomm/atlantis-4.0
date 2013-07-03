using System.Collections.Generic;
using System.Diagnostics;
using Atlantis.Framework.Personalization.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Personalization.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetTargetedMessageTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.Personalization.Impl.dll")]
    public void GetTargetedMessagesTest()
    {
      const string appId = "2";
      const string interactionPoint = "ProductUpsell";

      TargetedMessagesRequestData request = new TargetedMessagesRequestData("12345", "1", appId, interactionPoint);
      TargetedMessagesResponseData response = (TargetedMessagesResponseData)Engine.Engine.ProcessRequest(request, 669);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.TargetedMessagesData.ResultCode == 0);
    }
  }
}
