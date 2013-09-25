using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.DataCache;
using System.Net;
using Atlantis.Framework.Interface;
using System.Collections.ObjectModel;

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
      Assert.IsNotNull(responseData.CheckWhitelist("/default.aspx").UrlData.Style);

      Assert.IsTrue(responseData.CheckWhitelist("/hosting/web-hosting.aspx").Exists);
      Assert.IsNotNull(responseData.CheckWhitelist("/hosting/web-hosting.aspx").UrlData);
      Assert.IsNotNull(responseData.CheckWhitelist("/hosting/web-hosting.aspx").UrlData.Style);

      Assert.IsTrue(responseData.CheckWhitelist("/hosting/email-hosting").Exists);
      Assert.IsNotNull(responseData.CheckWhitelist("/hosting/email-hosting").UrlData);
      Assert.IsNotNull(responseData.CheckWhitelist("/hosting/email-hosting").UrlData.Style);

      Assert.IsFalse(responseData.CheckWhitelist("/default1.aspx").Exists);
      Assert.IsNull(responseData.CheckWhitelist("/default1.aspx").UrlData);

      Assert.IsFalse(responseData.CheckWhitelist("/hosting/ web-hosting.aspx").Exists);
      Assert.IsNull(responseData.CheckWhitelist("/hosting/ web-hosting.aspx").UrlData);

      Assert.IsFalse(responseData.CheckWhitelist("/hosting1/email-hosting").Exists);
      Assert.IsNull(responseData.CheckWhitelist("/hosting1/email-hosting").UrlData);
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

    }
  }
}
