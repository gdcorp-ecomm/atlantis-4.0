using System;
using System.Xml;
using Atlantis.Framework.DomainLookup.Interface;
using Atlantis.Framework.Interface;
using NUnit.Framework;

namespace Atlantis.Framework.DomainLookup.Tests
{
  [TestFixture]
  public class DomainLookupTests
  {
    const int _DOMAINLOOKUPREQUESTTYPE = 728;
    const int _DOMAINLOOKUPREQUESTTYPE_WSNOTVALID = 729;
    
    [Test]
    public void CheckActiveResellerDomain()
    {
      DomainLookupRequestData request = new DomainLookupRequestData("ELEVENCATS.INFO");
      DomainLookupResponseData response = (DomainLookupResponseData)Engine.Engine.ProcessRequest(request, _DOMAINLOOKUPREQUESTTYPE);

      Assert.AreEqual(response.domainData.DomainId, 2146871);
      Assert.AreEqual(response.domainData.HasSuspectTerms, false);
      Assert.AreEqual(response.domainData.IsActive, true);
      bool privateLabelCheck = false;

      if (response.domainData.PrivateLabelId > 3)
        privateLabelCheck = true;

      Assert.IsTrue(privateLabelCheck);
    }

    [Test]
    public void CheckActiveDomain()
    {
      DomainLookupRequestData request = new DomainLookupRequestData("jeffmcookietest1.info");
      DomainLookupResponseData response = (DomainLookupResponseData)Engine.Engine.ProcessRequest(request, _DOMAINLOOKUPREQUESTTYPE);

      DateTime xferAwayDate = DateTime.MinValue;
      DateTime.TryParse("2013-03-18T07:58:23-07:00", out xferAwayDate);

      DateTime createDate = DateTime.MinValue;
      DateTime.TryParse("2013-01-17T14:59:06-07:00", out createDate);

      Assert.AreEqual(response.domainData.XfrAwayDateUpdateReason, 1);
      Assert.AreEqual(response.domainData.XfrAwayDate, xferAwayDate);
      Assert.AreEqual(response.domainData.CreateDate, createDate);
      Assert.AreEqual(response.domainData.IsActive, true);
      bool privateLabelCheck = false;

      if (response.domainData.PrivateLabelId == 1)
        privateLabelCheck = true;

      Assert.IsTrue(privateLabelCheck);
    }

    [Test]
    public void CheckForEmptyResponse()
    {
      DomainLookupRequestData request = new DomainLookupRequestData("gghhasdd");
      DomainLookupResponseData response = (DomainLookupResponseData)Engine.Engine.ProcessRequest(request, _DOMAINLOOKUPREQUESTTYPE);
      Assert.AreEqual(response.domainData.Shopperid, "");
      Assert.AreEqual(response.domainData.IsActive, false);
      Assert.AreEqual(response.domainData.IsSmartDomain, false);
    }

    [Test]
    public void SimulateAnExcpetion()
    {
      DomainLookupRequestData request = new DomainLookupRequestData("elevencats.info");
      AtlantisException ex = new AtlantisException(request, this.GetType().ToString() + "." + "SimulateAnException", "This is testing the error handling of the repsonse", "domain=elevencats.info");
      DomainLookupResponseData response = DomainLookupResponseData.FromData(ex);
      Assert.IsNotNull(response.AtlantisEx);
      
    }
  }
}
