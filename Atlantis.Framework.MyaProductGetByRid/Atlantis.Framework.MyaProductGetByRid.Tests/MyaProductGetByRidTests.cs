using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.MyaProductGetByRid.Interface;

namespace Atlantis.Framework.MyaProductGetByRid.Tests
{
  [TestClass]
  public class MyaProductGetByRidTests
  {
    private const int _requestType = 383;
    private const string _shopperID = "840420";
    private const int _dedHostResourceID = 414599;
    private const int _dedHostProductTypeID = 98;
    private const int _virHostResourceID = 414598;
    private const int _virHostProductTypeID = 222;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetMyaDedicatedHostingProductsValidShopperGet()
    {
      var requestData = new MyaProductGetByRidRequestData(_shopperID
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , 1
         , _dedHostResourceID
         , _dedHostProductTypeID);


      MyaProductGetByRidResponseData responseData;

      try
      {
        responseData = (MyaProductGetByRidResponseData)Engine.Engine.ProcessRequest(requestData, _requestType);

        Debug.WriteLine(string.Format("CommonName: {0}, ProductTypeId: {4}, ProductType: {5}, Is Free: {1}, Account Expiration: {2}, IsRenwable: {3}", responseData.ProductAccount.CommonName, responseData.ProductAccount.IsFree, responseData.ProductAccount.AccountExpirationDate.ToLongDateString(), responseData.ProductAccount.IsRenewable, responseData.ProductAccount.ProductTypeId, responseData.ProductAccount.ProductType));
        Debug.WriteLine(string.Format("Dedicated Hosting property count: {0}", responseData.ProductAccount.PropertiesDictionary.Count));
        foreach (KeyValuePair<string, object> property in responseData.ProductAccount.PropertiesDictionary)
        {
          Debug.WriteLine(property.ToString());
        }

        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetMyaVirtualDedicatedHostingProductsValidShopperGet()
    {
      var requestData = new MyaProductGetByRidRequestData(_shopperID
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , 1
         , _virHostResourceID
         , _virHostProductTypeID);


      MyaProductGetByRidResponseData responseData;

      try
      {
        responseData = (MyaProductGetByRidResponseData)Engine.Engine.ProcessRequest(requestData, _requestType);

        Debug.WriteLine(string.Format("CommonName: {0}, ProductTypeId: {4}, ProductType: {5}, Is Free: {1}, Account Expiration: {2}, IsRenwable: {3}", responseData.ProductAccount.CommonName, responseData.ProductAccount.IsFree, responseData.ProductAccount.AccountExpirationDate.ToLongDateString(), responseData.ProductAccount.IsRenewable, responseData.ProductAccount.ProductTypeId, responseData.ProductAccount.ProductType));
        Debug.WriteLine(string.Format("Virtual Dedicated Hosting property count: {0}", responseData.ProductAccount.PropertiesDictionary.Count));
        foreach (KeyValuePair<string, object> property in responseData.ProductAccount.PropertiesDictionary)
        {
          Debug.WriteLine(property.ToString());
        }

        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetMyaHostedExchangeProduct()
    {
      var requestData = new MyaProductGetByRidRequestData("832652"
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , 1
         , 429176
         , 305);


      MyaProductGetByRidResponseData responseData;

      try
      {
        responseData = (MyaProductGetByRidResponseData)Engine.Engine.ProcessRequest(requestData, _requestType);

        Debug.WriteLine(string.Format("CommonName: {0}, ProductTypeId: {4}, ProductType: {5}, Is Free: {1}, Account Expiration: {2}, IsRenwable: {3}", responseData.ProductAccount.CommonName, responseData.ProductAccount.IsFree, responseData.ProductAccount.AccountExpirationDate.ToLongDateString(), responseData.ProductAccount.IsRenewable, responseData.ProductAccount.ProductTypeId, responseData.ProductAccount.ProductType));
        Debug.WriteLine(string.Format("Dedicated Hosting property count: {0}", responseData.ProductAccount.PropertiesDictionary.Count));
        foreach (KeyValuePair<string, object> property in responseData.ProductAccount.PropertiesDictionary)
        {
          Debug.WriteLine(property.ToString());
        }

        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }


  }
}
