using System.Linq;
using Atlantis.Framework.BazaarVoiceAPI.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.BazaarVoiceAPI.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.BazaarVoiceAPI.Impl.dll")]
  public class ReviewsAPITests
  {
    [TestMethod]
    public void GetReview()
    {
      ReviewsAPIRequestData request = new ReviewsAPIRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);

      request.AddQueryParameter("sort", "rating:desc");
      request.AddQueryParameter("limit", "1");
      request.AddQueryParameter("include", "products");
      request.AddQueryParameter("stats", "reviews");

      ReviewsAPIResponseData response = (ReviewsAPIResponseData)DataCache.DataCache.GetProcessRequest(request, 638);
      Assert.IsNotNull(response.Reviews);
    }

    [TestMethod]
    public void GetReviews()
    {
      ReviewsAPIRequestData request = new ReviewsAPIRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);

      request.AddQueryParameter("sort", "rating:desc");
      request.AddQueryParameter("limit", "10");
      request.AddQueryParameter("include", "products");
      request.AddQueryParameter("stats", "reviews");

      ReviewsAPIResponseData response = (ReviewsAPIResponseData)DataCache.DataCache.GetProcessRequest(request, 638);
      Assert.IsNotNull(response.Reviews);
    }

    [TestMethod]
    public void GetReviewInvalid()
    {
      ReviewsAPIRequestData request = new ReviewsAPIRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);

      request.AddQueryParameter("sort", "rating:desc");
      request.AddQueryParameter("limit", "0");
      request.AddQueryParameter("include", "products");
      request.AddQueryParameter("stats", "reviews");

      ReviewsAPIResponseData response = (ReviewsAPIResponseData)DataCache.DataCache.GetProcessRequest(request, 638);
      Assert.IsTrue(response.Reviews != null && response.Reviews.Count() == 0);
    }

    [TestMethod]
    public void CheckMD5KeysMatch()
    {
      ReviewsAPIRequestData request1 = new ReviewsAPIRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      ReviewsAPIRequestData request2 = new ReviewsAPIRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);

      request1.AddQueryParameter("sort", "rating:desc");
      request1.AddQueryParameter("limit", "1");
      request1.AddQueryParameter("include", "products");
      request1.AddQueryParameter("stats", "reviews");

      request2.AddQueryParameter("sort", "rating:desc");
      request2.AddQueryParameter("limit", "1");
      request2.AddQueryParameter("include", "products");
      request2.AddQueryParameter("stats", "reviews");

      Assert.AreEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void CheckMD5KeysDiffer()
    {
      ReviewsAPIRequestData request1 = new ReviewsAPIRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      ReviewsAPIRequestData request2 = new ReviewsAPIRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);

      request1.AddQueryParameter("sort", "rating:desc");
      request1.AddQueryParameter("limit", "1");
      request1.AddQueryParameter("include", "products");
      request1.AddQueryParameter("stats", "reviews");

      request2.AddQueryParameter("sort", "rating:desc");
      request2.AddQueryParameter("limit", "2");
      request2.AddQueryParameter("include", "products");
      request2.AddQueryParameter("stats", "reviews");

      Assert.AreNotEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void CheckMD5KeyOrder()
    {
      ReviewsAPIRequestData request1 = new ReviewsAPIRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      ReviewsAPIRequestData request2 = new ReviewsAPIRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);

      request1.AddQueryParameter("include", "products");
      request1.AddQueryParameter("sort", "rating:desc");
      request1.AddQueryParameter("stats", "reviews");
      request1.AddQueryParameter("limit", "1");

      request2.AddQueryParameter("sort", "rating:desc");
      request2.AddQueryParameter("limit", "1");
      request2.AddQueryParameter("include", "products");
      request2.AddQueryParameter("stats", "reviews");

      Assert.AreEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
    }
  }
}
