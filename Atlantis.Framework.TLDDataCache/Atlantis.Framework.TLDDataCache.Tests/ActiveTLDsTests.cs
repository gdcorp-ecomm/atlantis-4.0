using Atlantis.Framework.TLDDataCache.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atlantis.Framework.TLDDataCache.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Impl.dll")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  public class ActiveTLDsTests
  {
    const int _ACTIVETLDREQUEST = 635;

    [TestMethod]
    public void AllActiveTLDs()
    {
      var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var response = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _ACTIVETLDREQUEST);
      Assert.IsTrue(response.AllFlagNames.Count() > 0);
    }

    [TestMethod]
    public void COMLowerCase()
    {
      var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var response = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _ACTIVETLDREQUEST);
      Assert.IsTrue(response.IsTLDActive("com", "availcheckstatus"));
    }

    [TestMethod]
    public void COMUpperCase()
    {
      var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var response = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _ACTIVETLDREQUEST);
      Assert.IsTrue(response.IsTLDActive("com", "availcheckstatus"));
    }

    [TestMethod]
    public void InvalidTLD()
    {
      var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var response = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _ACTIVETLDREQUEST);
      Assert.IsFalse(response.IsTLDActive("madeup", "availcheckstatus"));
    }

    [TestMethod]
    public void UnionBasic()
    {
      var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var response = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _ACTIVETLDREQUEST);

      int mainpriceCount = response.GetActiveTLDCount("mainpricebox");
      int altpriceCount = response.GetActiveTLDCount("altpricebox");

      HashSet<string> union = response.GetActiveTLDUnion("mainpricebox", "altpricebox");

      int minUnion = (int)Math.Max(mainpriceCount, altpriceCount);
      int maxUnion = mainpriceCount + altpriceCount;
      bool isInRange = (minUnion <= union.Count) && (union.Count <= maxUnion);
      Assert.IsTrue(isInRange);
    }

    [TestMethod]
    public void UnionThree()
    {
      var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var response = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _ACTIVETLDREQUEST);

      int mainpriceCount = response.GetActiveTLDCount("mainpricebox");
      int isdbpavailableCount = response.GetActiveTLDCount("isdbpavailable");
      int backorderCount = response.GetActiveTLDCount("backorderstatus");

      HashSet<string> union = response.GetActiveTLDUnion("mainpricebox", "isdbpavailable", "backorderstatus");

      int minUnion = (int)Math.Max(backorderCount, Math.Max(mainpriceCount, isdbpavailableCount));
      int maxUnion = mainpriceCount + isdbpavailableCount + backorderCount;
      bool isInRange = (minUnion <= union.Count) && (union.Count <= maxUnion);
      Assert.IsTrue(isInRange);
    }

    [TestMethod]
    public void UnionDoesNotChangeMemberSets()
    {
      var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var response = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _ACTIVETLDREQUEST);

      int mainpriceCount = response.GetActiveTLDCount("mainpricebox");
      int altpriceCount = response.GetActiveTLDCount("altpricebox");

      HashSet<string> union = response.GetActiveTLDUnion("mainpricebox", "altpricebox");

      int mainpriceCount2 = response.GetActiveTLDCount("mainpricebox");
      int altpriceCount2 = response.GetActiveTLDCount("altpricebox");

      Assert.AreEqual(mainpriceCount, mainpriceCount2);
      Assert.AreEqual(altpriceCount, altpriceCount2);
    }

    [TestMethod]
    public void UnionWithInvalid()
    {
      var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var response = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _ACTIVETLDREQUEST);

      int mainpriceCount = response.GetActiveTLDCount("mainpricebox");
      HashSet<string> union = response.GetActiveTLDUnion("mainpricebox", "specialtldnotvalidflag");
      Assert.AreEqual(mainpriceCount, union.Count);
    }

    [TestMethod]
    public void UnionAllInvalid()
    {
      var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var response = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _ACTIVETLDREQUEST);

      HashSet<string> union = response.GetActiveTLDUnion("specialtldnotvalidflag", "specialtldnotvalidflag2");
      Assert.AreEqual(0, union.Count);
    }


    [TestMethod]
    public void IntersectBasic()
    {
      var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var response = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _ACTIVETLDREQUEST);

      int mainpriceCount = response.GetActiveTLDCount("mainpricebox");
      int isdbpavailableCount = response.GetActiveTLDCount("isdbpavailable");

      HashSet<string> intersect = response.GetActiveTLDIntersect("mainpricebox", "isdbpavailable");

      int maxIntersect = (int)Math.Min(mainpriceCount, isdbpavailableCount);
      Assert.IsTrue(intersect.Count <= maxIntersect);
    }

    [TestMethod]
    public void IntersectThree()
    {
      var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var response = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _ACTIVETLDREQUEST);

      int mainpriceCount = response.GetActiveTLDCount("mainpricebox");
      int isdbpavailableCount = response.GetActiveTLDCount("isdbpavailable");
      int backorderCount = response.GetActiveTLDCount("backorderstatus");

      HashSet<string> intersect = response.GetActiveTLDIntersect("mainpricebox", "isdbpavailable", "backorderstatus");

      int maxIntersect = (int)Math.Min(backorderCount, Math.Min(mainpriceCount, isdbpavailableCount));
      Assert.IsTrue(intersect.Count <= maxIntersect);
    }


    [TestMethod]
    public void IntersectDoesNotChangeMemberSets()
    {
      var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var response = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _ACTIVETLDREQUEST);

      int mainpriceCount = response.GetActiveTLDCount("mainpricebox");
      int isdbpavailableCount = response.GetActiveTLDCount("isdbpavailable");

      HashSet<string> intersect = response.GetActiveTLDIntersect("mainpricebox", "isdbpavailable");

      int mainpriceCount2 = response.GetActiveTLDCount("mainpricebox");
      int isdbpavailableCount2 = response.GetActiveTLDCount("isdbpavailable");

      Assert.AreEqual(mainpriceCount, mainpriceCount2);
      Assert.AreEqual(isdbpavailableCount, isdbpavailableCount2);
    }

    [TestMethod]
    public void IntersectWithInvalid()
    {
      var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var response = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _ACTIVETLDREQUEST);
      HashSet<string> intersect = response.GetActiveTLDIntersect("mainpricebox", "specialtldnotvalidflag");
      Assert.AreEqual(0, intersect.Count);
    }

    [TestMethod]
    public void IntersectAllInvalid()
    {
      var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var response = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _ACTIVETLDREQUEST);
      HashSet<string> intersect = response.GetActiveTLDIntersect("specialtldnotvalidflag", "specialtldnotvalidflag2");
      Assert.AreEqual(0, intersect.Count);
    }

    [TestMethod]
    public void TLDMLEnabled()
    {
      var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var response = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _ACTIVETLDREQUEST);
      var tldmlTlds = response.GetActiveTLDUnion("tldml_supported");
      Assert.IsTrue(tldmlTlds.Count > 0);
    }

  }
}
