using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.TLDDataCache.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Atlantis.Framework.TLDDataCache.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Impl.dll")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  public class TLDPhaseDateListTests
  {
    [TestMethod]
    public void RequestDataProperties()
    {
      var request = new TLDPhaseDateListRequestData(1640);
      Assert.AreEqual(1640, request.TldId);

      var request2 = new TLDPhaseDateListRequestData(1640);
      Assert.AreEqual(Convert.ToString(1640), request2.GetCacheMD5());
    }

    [TestMethod]
    public void RequestToXml()
    {
      var request = new TLDPhaseDateListRequestData(1640);
      Assert.IsTrue(!string.IsNullOrEmpty(request.ToXML()));
    }

    [TestMethod]
    public void TldPhaseDateListResponseDataFromBadXml()
    {
      var response = TLDPhaseDateListResponseData.FromDataCacheElement(XElement.Parse("<data>hello</data>"));
      Assert.AreEqual(false, response.TldPhaseDates.Any());
    }

    [TestMethod]
    public void TldPhaseDateListResponseDataNoItems()
    {
      const string data = "<data count=\"0\"></data>";
      var response = TLDPhaseDateListResponseData.FromDataCacheElement(XElement.Parse(data));
      Assert.AreEqual(false, response.TldPhaseDates.Any());
    }

    [TestMethod]
    public void TldPhaseDateListResponseDataWithMissingAttribute()
    {
      const string data = "<data count=\"1\"><item phaseStartDate=\"\" phaseEndDate=\"\" /></data>";
      var response = TLDPhaseDateListResponseData.FromDataCacheElement(XElement.Parse(data));
      var firstOrDefault = response.TldPhaseDates.FirstOrDefault();
      Assert.AreEqual(false, response.TldPhaseDates.Any());
    }


    [TestMethod]
    public void TldPhaseDateListResponseDataWithEmptyItems()
    {
      const string data = "<data count=\"1\"><item gdshop_tldPhase=\"\" phaseStartDate=\"\" phaseEndDate=\"\" /></data>";
      var response = TLDPhaseDateListResponseData.FromDataCacheElement(XElement.Parse(data));
      var firstOrDefault = response.TldPhaseDates.FirstOrDefault();
      Assert.AreEqual(true, firstOrDefault != null && firstOrDefault.StartDate == DateTime.MinValue && firstOrDefault.EndDate == DateTime.MinValue && firstOrDefault.PhaseCode == string.Empty);
    }

    [TestMethod]
    public void TldPhaseDateListResponseDataWithValidItems()
    {
      const string data = "<data count=\"1\"><item gdshop_tldPhase=\"LR\" phaseStartDate=\"04/22/2013\" phaseEndDate=\"12/22/2014\" /></data>";
      var response = TLDPhaseDateListResponseData.FromDataCacheElement(XElement.Parse(data));
      var firstOrDefault = response.TldPhaseDates.FirstOrDefault();
      Assert.AreEqual(true, firstOrDefault != null && firstOrDefault.StartDate != DateTime.MinValue && firstOrDefault.EndDate != DateTime.MinValue && !string.IsNullOrEmpty(firstOrDefault.PhaseCode));
    }

    [TestMethod]
    public void TldPhaseDateListResponseDataWithValidItemsToXml()
    {
      const string data = "<data count=\"1\"><item gdshop_tldPhase=\"LR\" phaseStartDate=\"04/22/2013\" phaseEndDate=\"12/22/2014\" /></data>";
      var response = TLDPhaseDateListResponseData.FromDataCacheElement(XElement.Parse(data));
      var xmlData = response.ToXML();
      Assert.AreEqual(true, !string.IsNullOrEmpty(xmlData) && !xmlData.Contains("<exception/>"));
    }

    const int _TLDPHASEDATELISTREQUEST = 744;

    [TestMethod]
    public void TldPhaseDateListRequestWithBadRequestData()
    {
      var request = new XData(string.Empty, string.Empty, string.Empty, string.Empty, 0);

      try
      {
        Engine.Engine.ProcessRequest(request, _TLDPHASEDATELISTREQUEST);
      }
      catch (Exception ex)
      {
        Assert.AreEqual(true, !string.IsNullOrEmpty(ex.ToString()));
      }
    }

    [TestMethod]
    public void TldPhaseDateListRequestWithNoData()
    {
      var request = new TLDPhaseDateListRequestData(0);
      try
      {
        var response = (TLDPhaseDateListResponseData)Engine.Engine.ProcessRequest(request, _TLDPHASEDATELISTREQUEST);
      }
      catch (Exception ex)
      {
        Assert.AreEqual(true, !string.IsNullOrEmpty(ex.ToString()));
      }
    }

    [TestMethod]
    public void TldPhaseDateListRequestWithValidData()
    {
      var request = new TLDPhaseDateListRequestData(1);
      var response = (TLDPhaseDateListResponseData)Engine.Engine.ProcessRequest(request, _TLDPHASEDATELISTREQUEST);

      Assert.AreEqual(true, response.TldPhaseDates.Any());
    }

    internal class XData : RequestData
    {
      internal XData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount)
        : base(shopperId, sourceURL, orderId, pathway, pageCount)
      {

      }

      public override string GetCacheMD5()
      {
        throw new System.NotImplementedException();
      }
    }
  }
}
