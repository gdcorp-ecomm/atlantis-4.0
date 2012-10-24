using System;
using System.Collections.Generic;
using System.Diagnostics;
using Atlantis.Framework.ProductPackager.Interface;
using Atlantis.Framework.ProductPackagerProductPackage.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ProductPackagerProductPackage.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.ProductPackagerProductPackage.Impl.dll")]
  public class ProductPackagerProductPackageTests
  {
    [TestMethod]
    public void GetValidProductPackage()
    {
      ProductPackagerProductPackageRequestData requestData = new ProductPackagerProductPackageRequestData("847235",
                                                                                                          "http://www.FbProductPackageProductPackageTests.com",
                                                                                                          string.Empty,
                                                                                                          Guid.NewGuid().ToString(),
                                                                                                          1,
                                                                                                          new List<string> { "73" });

      ProductPackagerProductPackageResponseData responseData = (ProductPackagerProductPackageResponseData)Engine.Engine.ProcessRequest(requestData, 610);

      IProductPackageData productPackage = responseData.ProductPackageData["73"];

      Console.Write(string.Format("Package Type: {0}, Product Id: {1}", productPackage.PackageType, productPackage.ParentProduct.ProductId));
      Debug.Write(string.Format("Package Type: {0}, Product Id: {1}", productPackage.PackageType, productPackage.ParentProduct.ProductId));
    }

    [TestMethod]
    public void GetInValidProductPackage()
    {

    }
  }
}
