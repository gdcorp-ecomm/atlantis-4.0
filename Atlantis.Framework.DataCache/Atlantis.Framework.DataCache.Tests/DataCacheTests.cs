using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace Atlantis.Framework.DataCache.Tests
{
  [TestClass]
  //[DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DataCacheService.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Products.Impl.dll")]
  public class DataCacheTests
  {
    [TestMethod]
    public void CustomClassBasic()
    {
      CustomClass resultFirst = DataCache.GetCustomCacheData<CustomClass>("First", CustomClass.GetCustomClass);
      CustomClass resultSecond = DataCache.GetCustomCacheData<CustomClass>("Second", CustomClass.GetCustomClass);
      CustomClass resultThird = DataCache.GetCustomCacheData<CustomClass>("First", CustomClass.GetCustomClass);

      Assert.AreEqual(resultFirst.Name, resultThird.Name);
      Assert.AreNotEqual(resultThird.Name, resultSecond.Name);

    }

    [TestMethod]
    public void CustomClassStruct()
    {
      int number = DataCache.GetCustomCacheData<int>("1", GetIntValue);
      int number2 = DataCache.GetCustomCacheData<int>("2", GetIntValue);
      int number3 = DataCache.GetCustomCacheData<int>("1", GetIntValue);
      Assert.AreEqual(number, number3);
    }

    private static int GetIntValue(string key)
    {
      if (key == "1")
        return 1;
      else
        return 2;
    }

    [TestMethod]
    public void CacheDataLinkInfo()
    {
      string xml = DataCache.GetCacheData("<LinkInfo><param name=\"contextID\" value=\"1\" /></LinkInfo>");
      Assert.IsNotNull(xml);
    }

    [TestMethod]
    public void GetNonUnifiedPfid()
    {
      int pfid = DataCache.GetPFIDByUnifiedID(101, 2);
      Assert.AreNotEqual(101, pfid);
    }

    [TestMethod]
    public void GetAppSetting()
    {
      string value = DataCache.GetAppSetting("SALES_VALID_COUNTRY_SITES");
      Assert.IsNotNull(value);
    }

    [TestMethod]
    public void GetProgId()
    {
      string progId = DataCache.GetProgID(1724);
      Assert.AreEqual("hunter", progId);
    }

    [TestMethod]
    public void GetPrivateLabelId()
    {
      int privateLabelId = DataCache.GetPrivateLabelId("hunter");
      Assert.AreEqual(1724, privateLabelId);
    }

    [TestMethod]
    public void GetPrivateLabelType()
    {
      int privateLabelType = DataCache.GetPrivateLabelType(1724);
      Assert.AreEqual(2, privateLabelType);
    }

    [TestMethod]
    public void IsPrivateLabelActive()
    {
      bool isActive = DataCache.IsPrivateLabelActive(1724);
      Assert.IsTrue(isActive);
    }

    [TestMethod]
    public void GetPLData()
    {
      string company = DataCache.GetPLData(1724, 0);
      Assert.IsNotNull(company);
    }

    [TestMethod]
    public void GetPLDataTwice()
    {
      string company = DataCache.GetPLData(1724, 0);
      string company2 = DataCache.GetPLData(1724, 0);
      Assert.ReferenceEquals(company, company2);
    }

    [TestMethod]
    public void ClearInProcessCachedData()
    {
      string company = DataCache.GetPLData(1724, 0);
      string company2 = DataCache.GetPLData(1724, 0);
      Assert.ReferenceEquals(company, company2);

      DataCache.ClearCachedData(659);

      string company3 = DataCache.GetPLData(1724, 0);
      Assert.IsFalse(object.ReferenceEquals(company, company3));
    }

  }
}
