using System.Linq;
using Atlantis.Framework.BazaarVoiceAPI.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Interface;
using System.Collections.Specialized;

namespace Atlantis.Framework.BazaarVoiceAPI.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.BazaarVoiceAPI.Impl.dll")]
  public class ReviewsSEOAPITests
  {
    [TestMethod]
    public void GetReviewsWithoutSlashinConfigConfig()
    {
      ReviewsSEORequestData request = new ReviewsSEORequestData("ssl-certificates", null, 1);
      ReviewsSEOResponseData response = (ReviewsSEOResponseData)Engine.Engine.ProcessRequest(request, 1000);

      Assert.IsTrue(!string.IsNullOrEmpty(response.HTML));
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void GetReviewsWithBadParams()
    {
      ReviewsSEORequestData request = new ReviewsSEORequestData("ssl-certificates", null, 1, "1224-en_us");
      ReviewsSEOResponseData response = (ReviewsSEOResponseData)Engine.Engine.ProcessRequest(request, 712);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void GetReviewsWitSeparateAPIKey()
    {
      ReviewsSEORequestData request = new ReviewsSEORequestData("ssl-certificates", null, 1);
      request.APIKey = "godaddy-skjdhfkjewh5kj4h";
      ReviewsSEOResponseData response = (ReviewsSEOResponseData)Engine.Engine.ProcessRequest(request, 712);
    }

    [TestMethod]
    public void CheckURIReplacement()
    {
      ReviewsSEORequestData request = new ReviewsSEORequestData("ssl-certificates", null, 1);
      ReviewsSEOResponseData response = (ReviewsSEOResponseData)Engine.Engine.ProcessRequest(request, 1000);

      Assert.AreNotSame(response.HTML, response.GetHtmlWithReplacementUri("http://www.godaddy.com/ssl/ssl-certificates.aspx"));
    }

    [TestMethod]
    public void CheckSEOMD5CacheMatch()
    {
      ReviewsSEORequestData request1 = new ReviewsSEORequestData("ssl-certificates", null, 1);
      ReviewsSEORequestData request2 = new ReviewsSEORequestData("ssl-certificates", null, 1);

      Assert.IsTrue(request1.GetCacheMD5().Equals(request2.GetCacheMD5()));

    }

    [TestMethod]
    public void CheckSEOMD5CacheDiff()
    {
      ReviewsSEORequestData request1 = new ReviewsSEORequestData("ssl-certificates", null, 1);
      ReviewsSEORequestData request2 = new ReviewsSEORequestData("ssl-certificates", null, 2);

      Assert.IsTrue(!request1.GetCacheMD5().Equals(request2.GetCacheMD5()));

    }

    [TestMethod]
    public void CheckSEOMD5CacheDiffAPIKeyIsNUll()
    {
      ReviewsSEORequestData request1 = new ReviewsSEORequestData("ssl-certificates", null, 1);
      request1.APIKey = null;
      ReviewsSEORequestData request2 = new ReviewsSEORequestData("ssl-certificates", null, 2);

      Assert.IsTrue(!request1.GetCacheMD5().Equals(request2.GetCacheMD5()));
    }

    [TestMethod]
    public void CheckSEOPageNumberLessThen1()
    {
      ReviewsSEORequestData request = new ReviewsSEORequestData("ssl-certificates", null, 0);
      ReviewsSEOResponseData response = (ReviewsSEOResponseData)Engine.Engine.ProcessRequest(request, 712);

      Assert.IsTrue(!string.IsNullOrEmpty(response.HTML));
    }

    [TestMethod]
    public void CheckQueryStringLoad()
    {
      NameValueCollection qs = new NameValueCollection();
      qs.Add("bvrrp", "6574-en_us/reviews/product/2/ssl-certificates.htm");

      ReviewsSEORequestData request1 = new ReviewsSEORequestData("ssl-certificates", qs);
      ReviewsSEORequestData request2 = new ReviewsSEORequestData("ssl-certificates", null, 2, "6574-en_us");

      ReviewsSEOResponseData response1 = (ReviewsSEOResponseData)Engine.Engine.ProcessRequest(request1, 712);
      ReviewsSEOResponseData response2 = (ReviewsSEOResponseData)Engine.Engine.ProcessRequest(request2, 712);

      Assert.AreEqual(response1.GetHtmlWithReplacementUri("http://www.godaddy.com/ssl/ssl-certificates.aspx"), response2.GetHtmlWithReplacementUri("http://www.godaddy.com/ssl/ssl-certificates.aspx"));
    }

    [TestMethod]
    public void CheckOnlyBVProductID()
    {      
      ReviewsSEORequestData request = new ReviewsSEORequestData("ssl-certificates");
      ReviewsSEOResponseData response = (ReviewsSEOResponseData)Engine.Engine.ProcessRequest(request, 712);
      Assert.IsTrue(!string.IsNullOrEmpty(response.HTML));
    }

  }
}
