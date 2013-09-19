using System.Diagnostics;
using Atlantis.Framework.DCCSetNameservers.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DCCSetNameservers.Tests
{
  [TestClass]
  public class DCCSetNameserversTests
  {
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.DCCSetNameservers.Impl.dll")]
    [TestMethod]
    public void TestValidSetNameserversPark()
    {
      DCCSetNameserversRequestData request = new DCCSetNameserversRequestData("840820", string.Empty, string.Empty, string.Empty, 0, DCCSetNameserversRequestData.NameserverType.Park, 1, 1667576, "MOBILE_CSA_DCC");
      request.TldId = 101;
      request.RegistrarId = "1";
      request.AddCustomNameserver("NS1.SECURESERVER.NET");
      request.AddCustomNameserver("NS2.SECURESERVER.NET");

      DCCSetNameserversResponseData response = (DCCSetNameserversResponseData)Engine.Engine.ProcessRequest(request, 114);
      Debug.Write(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.DCCSetNameservers.Impl.dll")]
    [TestMethod]
    public void TestValidSetNameserversForward()
    {
      DCCSetNameserversRequestData request = new DCCSetNameserversRequestData("840820", string.Empty, string.Empty, string.Empty, 0, DCCSetNameserversRequestData.NameserverType.Forward, 1, 1667576, "MOBILE_CSA_DCC");
      request.TldId = 101;
      request.RegistrarId = "1";
      request.AddCustomNameserver("NS1.SECURESERVER.NET");
      request.AddCustomNameserver("NS2.SECURESERVER.NET");

      DCCSetNameserversResponseData response = (DCCSetNameserversResponseData)Engine.Engine.ProcessRequest(request, 114);
      Debug.Write(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.DCCSetNameservers.Impl.dll")]
    [TestMethod]
    public void TestValidSetNameserversHost()
    {
      DCCSetNameserversRequestData request = new DCCSetNameserversRequestData("840820", string.Empty, string.Empty, string.Empty, 0, DCCSetNameserversRequestData.NameserverType.Host, 1, 1667576, "MOBILE_CSA_DCC");
      request.TldId = 101;
      request.RegistrarId = "1";
      request.AddCustomNameserver("NS1.SECURESERVER.NET");
      request.AddCustomNameserver("NS2.SECURESERVER.NET");

      DCCSetNameserversResponseData response = (DCCSetNameserversResponseData)Engine.Engine.ProcessRequest(request, 114);
      Debug.Write(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.DCCSetNameservers.Impl.dll")]
    [TestMethod]
    public void TestValidSetNameserversCustom()
    {
      DCCSetNameserversRequestData request = new DCCSetNameserversRequestData("840820", string.Empty, string.Empty, string.Empty, 0, DCCSetNameserversRequestData.NameserverType.Custom, 1, 1667586, "MOBILE_CSA_DCC");
      request.TldId = 101;
      request.RegistrarId = "1";
      request.AddCustomNameserverWithIp("ns08.dhivehichicks.org", "192.168.1.6");
      request.AddCustomNameserverWithIp("ns8.dhivehichicks.org", "125.1.1.6");
      request.AddCustomNameserverWithIp("ns9.dhivehichicks.org", null);
      request.AddCustomNameserverWithIp("ns9.dhivehichicks.org", "125.1.1.6");

      DCCSetNameserversResponseData response = (DCCSetNameserversResponseData)Engine.Engine.ProcessRequest(request, 114);
      Debug.Write(response.ToXML());
      foreach (var nameserverError in response.Errors.NameserverErrors)
      {
        Debug.Write(nameserverError.Description);
        Debug.Write(nameserverError.ErrorType);
        Debug.Write(nameserverError.Id);
        Debug.Write(nameserverError.Name); 
      }
      Assert.IsFalse(response.IsSuccess);
    }

    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.DCCSetNameservers.Impl.dll")]
    [TestMethod]
    public void TestValidSetNameserversRedundantChange()
    {
      DCCSetNameserversRequestData request = new DCCSetNameserversRequestData("840820", string.Empty, string.Empty, string.Empty, 0, DCCSetNameserversRequestData.NameserverType.Park, 1, 1667576, "MOBILE_CSA_DCC");
      request.TldId = 101;
      request.RegistrarId = "1";
      request.AddCustomNameserver("PARK3.SECURESERVER.NET");
      request.AddCustomNameserver("PARK4.SECURESERVER.NET");

      DCCSetNameserversResponseData response = (DCCSetNameserversResponseData)Engine.Engine.ProcessRequest(request, 114);
      Debug.Write(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.DCCSetNameservers.Impl.dll")]
    [TestMethod]
    public void TestSetNameserversErrors()
    {
      DCCSetNameserversRequestData request = new DCCSetNameserversRequestData("857020", string.Empty, string.Empty, string.Empty, 0, DCCSetNameserversRequestData.NameserverType.Forward, 1, 1666955, "MOBILE_CSA_DCC");
      request.TldId = 101;
      request.RegistrarId = "1";
      request.AddCustomNameserver("ns01.godaddy.org");
      request.AddCustomNameserver("ns1.dhivehichicks.org");

      DCCSetNameserversResponseData response = (DCCSetNameserversResponseData)Engine.Engine.ProcessRequest(request, 114);
      Assert.IsFalse(response.IsSuccess);
      Assert.IsTrue(response.Errors.NameserverErrors.Count > 0);
    }

    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.DCCSetNameservers.Impl.dll")]
    [TestMethod]
    public void TestSetNameserversEmpty()
    {
      DCCSetNameserversRequestData request = new DCCSetNameserversRequestData("857020", string.Empty, string.Empty, string.Empty, 0, DCCSetNameserversRequestData.NameserverType.Forward, 1, 1666955, "MOBILE_CSA_DCC");
      request.TldId = 101;
      request.RegistrarId = "1";
      request.AddCustomNameserver(string.Empty);
      request.AddCustomNameserver(" "); 

      DCCSetNameserversResponseData response = (DCCSetNameserversResponseData)Engine.Engine.ProcessRequest(request, 114);
      Debug.Write(response.ToXML());
      Assert.IsFalse(response.IsSuccess);
    }

    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.DCCSetNameservers.Impl.dll")]
    [TestMethod]
    public void TestSetNameserversForShopperThatDoesNotOwnDomain()
    {
      DCCSetNameserversRequestData request = new DCCSetNameserversRequestData("857020123", string.Empty, string.Empty, string.Empty, 0, DCCSetNameserversRequestData.NameserverType.Park, 1, 1667588, "MOBILE_CSA_DCC");
      request.TldId = 101;
      request.RegistrarId = "1";
      request.AddCustomNameserver("NS3.SECURESERVER.NET");
      request.AddCustomNameserver("NS4.SECURESERVER.NET");

      DCCSetNameserversResponseData response = (DCCSetNameserversResponseData)Engine.Engine.ProcessRequest(request, 114);

      Debug.Write(response.ToXML());
      // Pending updates by the DCC team.  They are returning success in this case.
      // As of 9-18-2013, John Roling said he would create a story that would fix bogus shopper updating nameservers.
      // Change this to IsFalse after the fix.
      Assert.IsTrue(response.IsSuccess);
    }

    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.DCCSetNameservers.Impl.dll")]
    [TestMethod]
    public void SetNameserversForPremiumDnsShopper()
    {
      // TEST Shopper, APPLY-MY-PREMIUM-NS-ANDROID.ORG
      DCCSetNameserversRequestData request = new DCCSetNameserversRequestData("87409", string.Empty, string.Empty, string.Empty, 0, DCCSetNameserversRequestData.NameserverType.Park, 1, 1710527, "MOBILE_CSA_DCC");
      request.TldId = 984;
      request.RegistrarId = "1";
      request.AddPremiumNameserver("PDNS01.TEST.DOMAINCONTROL.COM");
      request.AddPremiumNameserver("PDNS02.TEST.DOMAINCONTROL.COM");

      DCCSetNameserversResponseData response = (DCCSetNameserversResponseData)Engine.Engine.ProcessRequest(request, 114);

      // Pending updates by the DCC team.  They are returning success in this case.
      Debug.Write(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.DCCSetNameservers.Impl.dll")]
    [TestMethod]
    public void TestNoRegistrarIdSetNameservers()
    {
      DCCSetNameserversRequestData request = new DCCSetNameserversRequestData("840820", string.Empty, string.Empty, string.Empty, 0, DCCSetNameserversRequestData.NameserverType.Park, 1, 1667576, "MOBILE_CSA_DCC");
      request.TldId = 101;
      request.AddCustomNameserver("NS1.SECURESERVER.NET");
      request.AddCustomNameserver("NS1.SECURESERVER.NET"); 

      request.AddCustomNameserver("NS2.SECURESERVER.NET");

      DCCSetNameserversResponseData response = (DCCSetNameserversResponseData)Engine.Engine.ProcessRequest(request, 114);
      Debug.Write(response.ToXML());
      Assert.IsFalse(response.IsSuccess);
    }

    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.DCCSetNameservers.Impl.dll")]
    [TestMethod]
    public void TestIntentionalExceptionWrongRequest()
    {
      var request = new TestRequest("840820", string.Empty, string.Empty, string.Empty, 0);
      DCCSetNameserversResponseData response = null;

      try
      {
        response = (DCCSetNameserversResponseData) Engine.Engine.ProcessRequest(request, 114);
      }
      catch
      {
      }

      Assert.IsTrue(response == null);
    }
  }

  internal class TestRequest : RequestData
  {
    internal TestRequest(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {

    }

    public override string GetCacheMD5()
    {
      throw new System.NotImplementedException();
    }
  }

}
