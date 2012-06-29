using System;
using System.Collections.Generic;
using System.Diagnostics;
using Atlantis.Framework.ResourceIdGetByExtResource.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ResourceIdGetByExtResource.Tests
{
  [TestClass]
  public class ResourceIdGetByExtResourceTests
  {
    private const int _requestType = 540;
    private const string _shopperID = "840420";
    private const string _dedHostExtResourceID = "03314d25-5ee2-11e0-b652-0050569575d8";
    private const string _dedHostOrionNamespace = "dhs";
    private const string _virHostExtResourceID = "d134cde4-8c69-11e0-a4bd-0050569575d8";
    private const string _virHostOrionNamespace = "vph";

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetMyaDedicatedHostingProductsValidShopperGet()
    {
      var requestData = new ResourceIdGetByExtResourceRequestData(_shopperID
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , _dedHostExtResourceID
         , _dedHostOrionNamespace);


      ResourceIdGetByExtResourceResponseData responseData;

      try
      {
        responseData = (ResourceIdGetByExtResourceResponseData)Engine.Engine.ProcessRequest(requestData, _requestType);

        Debug.WriteLine(string.Format("ResourceId: {0}", responseData.ResourceId));

        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetVirtualDedicatedHostingResourceIdValidShopperGet()
    {
      var requestData = new ResourceIdGetByExtResourceRequestData(_shopperID
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , _virHostExtResourceID
         , _virHostOrionNamespace);


      ResourceIdGetByExtResourceResponseData responseData;

      try
      {
        responseData = (ResourceIdGetByExtResourceResponseData)Engine.Engine.ProcessRequest(requestData, _requestType);

        Debug.WriteLine(string.Format("ResourceId: {0}", responseData.ResourceId));

        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetHostedExchangeResourceId()
    {
      var requestData = new ResourceIdGetByExtResourceRequestData("832652"
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , "429176"
         , "sharedexchange");


      ResourceIdGetByExtResourceResponseData responseData;

      try
      {
        responseData = (ResourceIdGetByExtResourceResponseData)Engine.Engine.ProcessRequest(requestData, _requestType);

        Debug.WriteLine(string.Format("ResourceId: {0}", responseData.ResourceId));

        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }


  }
}
