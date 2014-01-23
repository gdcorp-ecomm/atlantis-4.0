using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Atlantis.Framework.Products.Interface;

namespace Atlantis.Framework.Products.Tests
{
  [TestClass]
  public class ProductGroupsOfferedMarketsRequestDataTests
  {
    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void ProductGroupOfferedMarketsRequestData_ConstructorTest()
    {
      var target = new ProductGroupsOfferedMarketsRequestData();
      Assert.IsNotNull(target);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void ProductGroupOfferedmarketsRequestData_GetCacheMD5Test()
    {
      var target = new ProductGroupsOfferedMarketsRequestData();
      Assert.IsNotNull(target);

      var expected = "data";
      var actual = target.GetCacheMD5();
      Assert.AreEqual(expected, actual);
    }
  }
}

