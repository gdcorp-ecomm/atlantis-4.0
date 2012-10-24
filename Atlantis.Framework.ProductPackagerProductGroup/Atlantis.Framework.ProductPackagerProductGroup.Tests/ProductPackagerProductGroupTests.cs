using System;
using System.Collections.Generic;
using System.Diagnostics;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ProductPackagerProductGroup.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ProductPackagerProductGroup.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.ProductPackagerProductGroup.Impl.dll")]
  public class ProductPackagerProductGroupTests
  {
    [TestMethod]
    public void GetValidProductGroup()
    {
      ProductPackagerProductGroupRequestData requestData = new ProductPackagerProductGroupRequestData("847235",
                                                                                                        "http://www.FbProductPackageProductGroupTests.com",
                                                                                                        string.Empty,
                                                                                                        Guid.NewGuid().ToString(),
                                                                                                        1,
                                                                                                        new List<string> { "137" });

      ProductPackagerProductGroupResponseData responseData = (ProductPackagerProductGroupResponseData)Engine.Engine.ProcessRequest(requestData, 609);

      Console.Write(responseData.ProductGroupData["137"].Name);
      Debug.Write(responseData.ProductGroupData["137"].Name);
    }

    [TestMethod]
    public void GetInValidProductGroup()
    {
      bool thrownException = false;

      try
      {
        ProductPackagerProductGroupRequestData requestData = new ProductPackagerProductGroupRequestData("847235",
                                                                                                        "http://www.FbProductPackageProductGroupTests.com",
                                                                                                        string.Empty,
                                                                                                        Guid.NewGuid().ToString(),
                                                                                                        1,
                                                                                                        new List<string> { "abc123" });

        Engine.Engine.ProcessRequest(requestData, 609);
      }
      catch (AtlantisException atlEx)
      {
        thrownException = true;
        Console.Write(atlEx.Message);
        Debug.Write(atlEx.Message);
      }

      Assert.IsTrue(thrownException, "Expected exception to be thrown for bad product group id.");
    }
  }
}
