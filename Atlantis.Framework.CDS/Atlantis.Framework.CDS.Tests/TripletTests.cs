using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.DataCache;
using System.Net;
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
      CDSRequestData requestData = new CDSRequestData(shopperId, string.Empty, string.Empty, string.Empty, 1, query);
      //requestData.RequestTimeout = TimeSpan.FromSeconds(20);

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
      CDSRequestData requestData = new CDSRequestData(shopperId, string.Empty, string.Empty, string.Empty, 1, query);
      //requestData.RequestTimeout = TimeSpan.FromSeconds(20);

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
      CDSRequestData requestData = new CDSRequestData(shopperId, string.Empty, string.Empty, string.Empty, 1, query);
      //requestData.RequestTimeout = TimeSpan.FromSeconds(20);

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
      string query = "content/sales/whitelist";
      string pathway = Guid.NewGuid().ToString();
      string errorDescription = "this is a test error descrption!";
      CDSRequestData requestData = new CDSRequestData(shopperId, string.Empty, string.Empty, pathway, 1, query);

      //Act
      UrlWhitelistResponseData responseData = (UrlWhitelistResponseData)Engine.Engine.ProcessRequest(requestData, requestType);

      //Assert
      Assert.IsTrue(responseData.Contains("/default.aspx"));
      Assert.IsTrue(responseData.Contains("/hosting/web-hosting.aspx"));
      Assert.IsTrue(responseData.Contains("/hosting/email-hosting"));

      Assert.IsFalse(responseData.Contains("/default1.aspx"));
      Assert.IsFalse(responseData.Contains("/hosting1/web-hosting.aspx"));
      Assert.IsFalse(responseData.Contains("/hosting/email-hosting-"));
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
      string errorDescription = "this is a test error descrption!";
      CDSRequestData requestData = new CDSRequestData(shopperId, string.Empty, string.Empty, pathway, 1, query);

      //Act
      ContentVersionResponseData responseData = (ContentVersionResponseData)Engine.Engine.ProcessRequest(requestData, requestType);

      //Assert
      Assert.IsNotNull(responseData.Content);

    }
  }
}
