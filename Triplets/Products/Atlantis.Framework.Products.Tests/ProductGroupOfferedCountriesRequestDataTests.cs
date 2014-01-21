using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Products.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Atlantis.Framework.Products.Tests
{
  [TestClass]
  public class ProductGroupOfferedCountriesRequestDataTests
  {
    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void ProductGroupOfferedCountriesRequestData_ConstructorTest()
    {
      const int productGroupId = 99;

      var target = new ProductGroupOfferedCountriesRequestData(productGroupId);
      Assert.IsNotNull(target);
      
      Assert.AreEqual(productGroupId, target.ProductGroupId);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void ProductGroupOfferedCountriesRequestData_CacheKeyTest()
    {
      const int productGroupId = 99;

      var target = new ProductGroupOfferedCountriesRequestData(productGroupId);
      Assert.IsNotNull(target);

      var actual = target.GetCacheMD5();
      Assert.AreEqual(productGroupId.ToString(), actual);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void ProductGroupOfferedCountriesRequestData_CacheKeySame()
    {
      const int productGroupId = 99;
      ProductGroupOfferedCountriesRequestData request1 = new ProductGroupOfferedCountriesRequestData(productGroupId);
      ProductGroupOfferedCountriesRequestData request2 = new ProductGroupOfferedCountriesRequestData(productGroupId);
      Assert.AreEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void NonUnifiedPfidRequestCacheKeyDifferent()
    {
      const int productGroupId = 99;
      ProductGroupOfferedCountriesRequestData request1 = new ProductGroupOfferedCountriesRequestData(productGroupId);
      ProductGroupOfferedCountriesRequestData request2 = new ProductGroupOfferedCountriesRequestData(productGroupId + 1);
      ProductGroupOfferedCountriesRequestData request3 = new ProductGroupOfferedCountriesRequestData(productGroupId + 2);

      Assert.AreNotEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
      Assert.AreNotEqual(request1.GetCacheMD5(), request3.GetCacheMD5());
      Assert.AreNotEqual(request2.GetCacheMD5(), request3.GetCacheMD5());
    }
  }
}
