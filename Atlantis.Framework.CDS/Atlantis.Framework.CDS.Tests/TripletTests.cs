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
      Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, responseData.StatusCode);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [ExpectedException(typeof(AtlantisException))]
    public void Triplet_Receives_404_If_Document_Does_Not_Exist()
    {
      //Arrange
      string shopperId = "860316";
      int requestType = 424;
      string query = "sales/1/lp/nonexistent";
      CDSRequestData requestData = new CDSRequestData(shopperId, string.Empty, string.Empty, string.Empty, 1, query);
      //requestData.RequestTimeout = TimeSpan.FromSeconds(20);

      //Act 
      try
      {
        CDSResponseData responseData = (CDSResponseData)Engine.Engine.ProcessRequest(requestData, requestType);
      }
      catch (AtlantisException ex)
      {
        Assert.AreEqual("The remote server returned an error: (404) Not Found.", ex.Message);
        throw;
      }
    }
  }
}
