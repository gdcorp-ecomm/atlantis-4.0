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
  [TestClass]
  public class GetTargetedMessageTests
  {
    private readonly Lazy<bool> _canCallTMS = new Lazy<bool>(() => DataCache.DataCache.GetAppSetting("ATLANTIS_PERSONALIZATION_TRIPLET_TMS_ON").Equals("true", StringComparison.OrdinalIgnoreCase));

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.Personalization.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
    public void GetTargetedMessagesWithoutISCOrAnonId()
    {
      const string appId = "2";
      const string interactionPoint = "ProductUpsell";

      TargetedMessagesRequestData request = new TargetedMessagesRequestData("12345", "1", appId, interactionPoint, string.Empty, string.Empty);
      TargetedMessagesResponseData response = (TargetedMessagesResponseData)Engine.Engine.ProcessRequest(request, 669);
      if (_canCallTMS.Value)
      {
        Debug.Write(response.ToXML());
        Assert.IsFalse(response.TMSSwitchTurnedOff);
        Assert.IsTrue(response.TargetedMessagesData.ResultCode == 0);
      }
      else
      {
        Assert.IsTrue(response.TMSSwitchTurnedOff);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.Personalization.Impl.dll")]
    public void GetTargetedMessagesWithoutShopperId()
    {
      const string appId = "2";
      const string interactionPoint = "ProductUpsell";

      TargetedMessagesRequestData request = new TargetedMessagesRequestData(string.Empty, "1", appId, interactionPoint, string.Empty, string.Empty);
      TargetedMessagesResponseData response = (TargetedMessagesResponseData)Engine.Engine.ProcessRequest(request, 669);
      if (_canCallTMS.Value)
      {
        Debug.Write(response.ToXML());
        Assert.IsFalse(response.TMSSwitchTurnedOff);
        Assert.IsTrue(response.TargetedMessagesData.ResultCode == 0);
      }
      else
      {
        Assert.IsTrue(response.TMSSwitchTurnedOff);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.Personalization.Impl.dll")]
    public void GetTargetedMessagesWithISC()
    {
      const string appId = "2";
      const string interactionPoint = "ProductUpsell";

      TargetedMessagesRequestData request = new TargetedMessagesRequestData(string.Empty, "1", appId, interactionPoint, Guid.NewGuid().ToString(), "test");
      TargetedMessagesResponseData response = (TargetedMessagesResponseData)Engine.Engine.ProcessRequest(request, 669);

      if (_canCallTMS.Value)
      {
        Debug.Write(response.ToXML());
        Assert.IsFalse(response.TMSSwitchTurnedOff);
        Assert.IsTrue(response.TargetedMessagesData.ResultCode == 0);
      }
      else
      {
        Assert.IsTrue(response.TMSSwitchTurnedOff);
      }
    }
  }
}
