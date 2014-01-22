using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.CDS.Interface;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CDS.Tests
{
  [TestClass]
  public class TripletTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
    public void Triplet_Receives_Content()
    {
      //Arrange
      string shopperId = "860316";
      int requestType = 424;
      string query = "sales/1/lp/email";
      CDSRequestData requestData = new CDSRequestData(query);
      requestData.ShopperID = shopperId;

      //Act
      CDSResponseData responseData = (CDSResponseData)DataCache.DataCache.GetProcessRequest(requestData, requestType);

      //Assert
      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsNotNull(responseData.ResponseData);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
    public void Triplet_Receives_200_If_Document_Does_Exist()
    {
      //Arrange
      string shopperId = "860316";
      int requestType = 424;
      string query = "sales/1/lp/email";
      CDSRequestData requestData = new CDSRequestData(query);
      requestData.ShopperID = shopperId;

      //Act
      CDSResponseData responseData = (CDSResponseData)DataCache.DataCache.GetProcessRequest(requestData, requestType);

      //Assert
      Assert.IsTrue(responseData.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
    [ExpectedException(typeof(Atlantis.Framework.Interface.AtlantisException))]
    public void Triplet_Receives_404_If_Document_Does_Not_Exist()
    {
      //Arrange
      string shopperId = "860316";
      int requestType = 424;
      string query = "sales/1/lp/nonexistent|";
      CDSRequestData requestData = new CDSRequestData(query);
      requestData.ShopperID = shopperId;

      //Act 
      CDSResponseData responseData = (CDSResponseData)Engine.Engine.ProcessRequest(requestData, requestType);
      Assert.IsFalse(responseData.IsSuccess);
    }

    [TestMethod()]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
    public void UrlWhiteListRequestTest()
    {
      //Arrange
      string shopperId = "12345";
      int requestType = 688;
      string query = "content/sales/whitelist?docid=5175b13e8b29c70404bc0163";
      string pathway = Guid.NewGuid().ToString();
      CDSRequestData requestData = new CDSRequestData(query);
      requestData.ShopperID = shopperId;

      //Act
      UrlWhitelistResponseData responseData = (UrlWhitelistResponseData)Engine.Engine.ProcessRequest(requestData, requestType);

      //Assert
      Assert.IsTrue(responseData.CheckWhitelist("/default.aspx").Exists);
      Assert.IsNotNull(responseData.CheckWhitelist("/default.aspx").UrlData);
      Assert.IsNotNull(responseData.CheckWhitelist("/default.aspx").UrlData["style"]);

      Assert.IsTrue(responseData.CheckWhitelist("/hosting/web-hosting.aspx").Exists);
      Assert.IsNotNull(responseData.CheckWhitelist("/hosting/web-hosting.aspx").UrlData);
      Assert.IsNotNull(responseData.CheckWhitelist("/hosting/web-hosting.aspx").UrlData["style"]);

      Assert.IsTrue(responseData.CheckWhitelist("/hosting/email-hosting").Exists);
      Assert.IsNotNull(responseData.CheckWhitelist("/hosting/email-hosting").UrlData);
      Assert.IsNotNull(responseData.CheckWhitelist("/hosting/email-hosting").UrlData["style"]);

      Assert.IsFalse(responseData.CheckWhitelist("/default1.aspx").Exists);
      Assert.AreEqual(responseData.CheckWhitelist("/default1.aspx").UrlData["style"], "null");

      Assert.IsFalse(responseData.CheckWhitelist("/hosting/ web-hosting.aspx").Exists);
      Assert.AreEqual(responseData.CheckWhitelist("/hosting/ web-hosting.aspx").UrlData["style"], "null");

      Assert.IsFalse(responseData.CheckWhitelist("/hosting1/email-hosting").Exists);
      Assert.AreEqual(responseData.CheckWhitelist("/hosting1/email-hosting").UrlData["style"], "null");

      ICDSDebugInfo debugInfo = responseData as ICDSDebugInfo;
      Assert.AreEqual(debugInfo.VersionId.oid, "5175b13e8b29c70404bc0163");
      Assert.IsTrue(!string.IsNullOrEmpty(debugInfo.DocumentId.oid));
    }

    [TestMethod()]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
    public void ContentVersionRequestTest()
    {
      //Arrange
      string shopperId = "12345";
      int requestType = 687;
      string query = "content/sales/whitelist";
      string pathway = Guid.NewGuid().ToString();
      CDSRequestData requestData = new CDSRequestData(query);
      requestData.ShopperID = shopperId;

      //Act
      ContentVersionResponseData responseData = (ContentVersionResponseData)Engine.Engine.ProcessRequest(requestData, requestType);

      //Assert
      Assert.IsNotNull(responseData.Content);

      ICDSDebugInfo debugInfo = responseData as ICDSDebugInfo;
      Assert.IsTrue(!string.IsNullOrEmpty(debugInfo.VersionId.oid));
      Assert.IsTrue(!string.IsNullOrEmpty(debugInfo.DocumentId.oid));
    }

    [TestMethod()]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
    public void RoutingRulesRequestTest()
    {
      //Arrange
      string shopperId = "12345";
      int requestType = 696;
      string query = "content/sales/hosting/webhostingrules?docid=5170752af778fc014c90b155";
      string pathway = Guid.NewGuid().ToString();
      CDSRequestData requestData = new CDSRequestData(query);
      requestData.ShopperID = shopperId;

      //Act
      RoutingRulesResponseData responseData = (RoutingRulesResponseData)Engine.Engine.ProcessRequest(requestData, requestType);

      //Assert
      ReadOnlyCollection<IRoutingRule> routingRules;
      if (responseData.TryGetValue("Redirect", out routingRules))
      {
        Assert.IsNotNull(routingRules);
        foreach (IRoutingRule rule in routingRules)
        {
          Assert.IsNotNull(rule.Type);
          Assert.IsTrue(rule.Type == "Redirect");
          Assert.IsNotNull(rule.Condition);
          Assert.IsNotNull(rule.Data);
        }
      }

      if (responseData.TryGetValue("Route", out routingRules))
      {
        Assert.IsNotNull(routingRules);
        foreach (IRoutingRule rule in routingRules)
        {
          Assert.IsNotNull(rule.Type);
          Assert.IsTrue(rule.Type == "Route");
          Assert.IsNotNull(rule.Condition);
          Assert.IsNotNull(rule.Data);
        }
      }
      ICDSDebugInfo debugInfo = responseData as ICDSDebugInfo;
      Assert.IsTrue(!string.IsNullOrEmpty(debugInfo.VersionId.oid));
      Assert.IsTrue(!string.IsNullOrEmpty(debugInfo.DocumentId.oid));
    }

    [TestMethod()]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
    public void PackageGroupRequestInvalidPackageTest()
    {
      string packageId = "GridHostUnlDiabloLin1Yr";
      string shopperId = "12345";
      int requestType = 766;
      string query = "content/packages/packagegrouping?docid=529f78d5f778fc1a64995ed8";
      CDSRequestData requestData = new CDSRequestData(query);
      requestData.ShopperID = shopperId;

      PackageGroupResponseData responseData = (PackageGroupResponseData)Engine.Engine.ProcessRequest(requestData, requestType);

      IPackageGroup packageGroup;

      Assert.IsTrue(!responseData.TryGetValue(packageId, out packageGroup));
      Assert.IsNull(packageGroup);
    }

    [TestMethod()]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
    public void PackageGroupRequestInvalidPackageURLTest()
    {
      string packageId = "GridHostUnlDiabloLin1Yr";
      string shopperId = "12345";
      int requestType = 766;
      string query = "content/packages/packagegroupingdfsf778fc1a64995ed8";
      CDSRequestData requestData = new CDSRequestData(query);
      requestData.ShopperID = shopperId;

      PackageGroupResponseData responseData = (PackageGroupResponseData)Engine.Engine.ProcessRequest(requestData, requestType);

      IPackageGroup packageGroup;

      Assert.IsTrue(!responseData.TryGetValue(packageId, out packageGroup));
      Assert.IsNull(packageGroup);
    }

    [TestMethod()]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
    public void PackageGroupRequestTest()
    {
      string packageId = "host_GridHostUnlDiabloLin1Yr_us";
      string shopperId = "12345";
      int requestType = 766;
      string query = "content/packages/packagegrouping";
      CDSRequestData requestData = new CDSRequestData(query);
      requestData.ShopperID = shopperId;

      PackageGroupResponseData responseData = (PackageGroupResponseData)Engine.Engine.ProcessRequest(requestData, requestType);

      IPackageGroup packageGroup;
      IEnumerable<string> packageIds;
      Assert.IsTrue(responseData.TryGetValue(packageId, out packageGroup));
      Assert.IsNotNull(packageGroup);
      Assert.IsTrue(responseData.TryGetValue(packageGroup.Name, out packageIds));

      List<string> packageIdList = (List<string>)packageIds;

      Assert.IsTrue(packageIdList.Contains("host_GridHostUnlDiabloLin4Yr_us"));

    }

    [TestMethod()]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
    public void PackageXMLRequestTest()
    {
      string packageId = "GridHostUnlDiabloLin1Yr";
      string shopperId = "12345";
      int requestType = 768;
      string packageQuery = "content/packages/hosting/basket?docid=529e5cd3f778fc4280723847";
      CDSRequestData packageRequestData = new CDSRequestData(packageQuery);
      packageRequestData.ShopperID = shopperId;

      PackageXMLResponseData packageResponseData = (PackageXMLResponseData)Engine.Engine.ProcessRequest(packageRequestData, requestType);

      XDocument xPackageDoc;
      Assert.IsTrue(packageResponseData.TryGetValue(packageId, out xPackageDoc));
      Assert.IsNotNull(xPackageDoc);
      Assert.AreEqual("false", xPackageDoc.Element("Package").Attribute("QtyIsDuration").Value);
    }


    [TestMethod()]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
    public void PackageXMLRequestInvalidPackageTest()
    {
      string packageId = "GridHostUnlDiabloLin1Yr_kjsdr4e344";
      string shopperId = "12345";
      int requestType = 768;
      string packageQuery = "content/packages/hosting/basket";
      CDSRequestData packageRequestData = new CDSRequestData(packageQuery);
      packageRequestData.ShopperID = shopperId;

      PackageXMLResponseData packageResponseData = (PackageXMLResponseData)Engine.Engine.ProcessRequest(packageRequestData, requestType);

      XDocument xPackageDoc;
      Assert.IsFalse(packageResponseData.TryGetValue(packageId, out xPackageDoc));
      Assert.IsNull(xPackageDoc);
    }

    [TestMethod()]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
    public void PackageXMLRequestInvalidBasketURLTest()
    {
      string packageId = "GridHostUnlDiabloLin1Yr_kjsdr4e344";
      string shopperId = "12345";
      int requestType = 768;
      string packageQuery = "content/packages/hosting/basketsd54e5";
      CDSRequestData packageRequestData = new CDSRequestData(packageQuery);
      packageRequestData.ShopperID = shopperId;

      PackageXMLResponseData packageResponseData = (PackageXMLResponseData)Engine.Engine.ProcessRequest(packageRequestData, requestType);

      XDocument xPackageDoc;
      Assert.IsFalse(packageResponseData.TryGetValue(packageId, out xPackageDoc));
      Assert.IsNull(xPackageDoc);
    }

    [TestMethod()]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
    public void PackageJSONRequestTest()
    {
      string packageId = "GridHostUnlDiabloLin1Yr";
      string shopperId = "12345";

      int requestType = 771;
      string packageQuery = "content/packages/hosting/config?docid=52a62254f778fc29a83ead5c";
      CDSRequestData packageRequestData = new CDSRequestData(packageQuery);
      packageRequestData.ShopperID = shopperId;

      PackageJSONResponseData packageResponseData = (PackageJSONResponseData)Engine.Engine.ProcessRequest(packageRequestData, requestType);

      string packageJson;
      Assert.IsTrue(packageResponseData.TryGetValue(packageId, out packageJson));
      Assert.IsNotNull(packageJson);
      Assert.IsTrue(packageJson.ToString().Contains("CurrentPlanDuration"));
    }

    [TestMethod()]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
    public void PackageJSONRequestInvalidConfigURLTest()
    {
      string packageId = "GridHostUnlDiabloLin1Yr_fdskjlk543ujw";
      string shopperId = "12345";

      int requestType = 771;
      string packageQuery = "content/packages/hosting/config849";
      CDSRequestData packageRequestData = new CDSRequestData(packageQuery);
      packageRequestData.ShopperID = shopperId;

      PackageJSONResponseData packageResponseData = (PackageJSONResponseData)Engine.Engine.ProcessRequest(packageRequestData, requestType);

      string packageJson;
      Assert.IsFalse(packageResponseData.TryGetValue(packageId, out packageJson));
      Assert.IsNull(packageJson);
    }

    [TestMethod()]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
    public void PackageJSONRequestInvalidPackageTest()
    {
      string packageId = "GridHostUnlDiabloLin1Yr_fdskjlk543ujw";
      string shopperId = "12345";

      int requestType = 771;
      string packageQuery = "content/packages/hosting/config";
      CDSRequestData packageRequestData = new CDSRequestData(packageQuery);
      packageRequestData.ShopperID = shopperId;

      PackageJSONResponseData packageResponseData = (PackageJSONResponseData)Engine.Engine.ProcessRequest(packageRequestData, requestType);

      string packageJson;
      Assert.IsFalse(packageResponseData.TryGetValue(packageId, out packageJson));
      Assert.IsNull(packageJson);
    }
  }

}
