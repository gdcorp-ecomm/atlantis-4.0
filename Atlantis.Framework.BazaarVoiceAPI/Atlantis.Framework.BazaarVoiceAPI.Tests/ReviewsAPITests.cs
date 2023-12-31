﻿using System.Linq;
using Atlantis.Framework.BazaarVoiceAPI.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Interface;

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

      request.AddQueryParameter("Sort", "rating:desc");
      request.AddQueryParameter("Limit", "1");
      request.AddQueryParameter("Include", "products");
      request.AddQueryParameter("Stats", "reviews");

      ReviewsAPIResponseData response = (ReviewsAPIResponseData)Engine.Engine.ProcessRequest(request, 638);
      Assert.IsTrue(response.Reviews != null && response.Reviews.Count() == 1);
    }

    [TestMethod]
    public void GetReviews()
    {
      ReviewsAPIRequestData request = new ReviewsAPIRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);

      request.AddQueryParameter("Sort", "rating:desc");
      request.AddQueryParameter("Limit", "10");
      request.AddQueryParameter("Include", "products");
      request.AddQueryParameter("Stats", "reviews");

      ReviewsAPIResponseData response = (ReviewsAPIResponseData)Engine.Engine.ProcessRequest(request, 638);
      Assert.IsTrue(response.Reviews != null && response.Reviews.Count() == 10);
    }

    [TestMethod]
    public void GetReviewInvalid()
    {
      ReviewsAPIRequestData request = new ReviewsAPIRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);

      request.AddQueryParameter("Sort", "rating:desc");
      request.AddQueryParameter("Limit", "0");
      request.AddQueryParameter("Include", "products");
      request.AddQueryParameter("Stats", "reviews");

      ReviewsAPIResponseData response = (ReviewsAPIResponseData)Engine.Engine.ProcessRequest(request, 638);
      Assert.IsTrue(response.Reviews != null && response.Reviews.Count() == 0);
    }

    [TestMethod]
    public void CheckMD5KeysMatch()
    {
      ReviewsAPIRequestData request1 = new ReviewsAPIRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      ReviewsAPIRequestData request2 = new ReviewsAPIRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);

      request1.AddQueryParameter("Sort", "rating:desc");
      request1.AddQueryParameter("Limit", "1");
      request1.AddQueryParameter("Include", "products");
      request1.AddQueryParameter("Stats", "reviews");

      request2.AddQueryParameter("Sort", "rating:desc");
      request2.AddQueryParameter("Limit", "1");
      request2.AddQueryParameter("Include", "products");
      request2.AddQueryParameter("Stats", "reviews");

      Assert.AreEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void CheckMD5KeysDiffer()
    {
      ReviewsAPIRequestData request1 = new ReviewsAPIRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      ReviewsAPIRequestData request2 = new ReviewsAPIRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);

      request1.AddQueryParameter("Sort", "rating:desc");
      request1.AddQueryParameter("Limit", "1");
      request1.AddQueryParameter("Include", "products");
      request1.AddQueryParameter("Stats", "reviews");

      request2.AddQueryParameter("Sort", "rating:desc");
      request2.AddQueryParameter("Limit", "2");
      request2.AddQueryParameter("Include", "products");
      request2.AddQueryParameter("Stats", "reviews");

      Assert.AreNotEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void CheckMD5KeyOrder()
    {
      ReviewsAPIRequestData request1 = new ReviewsAPIRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      ReviewsAPIRequestData request2 = new ReviewsAPIRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);

      request1.AddQueryParameter("Include", "products");
      request1.AddQueryParameter("Sort", "rating:desc");
      request1.AddQueryParameter("Stats", "reviews");
      request1.AddQueryParameter("Limit", "1");

      request2.AddQueryParameter("Sort", "rating:desc");
      request2.AddQueryParameter("Limit", "1");
      request2.AddQueryParameter("Include", "products");
      request2.AddQueryParameter("Stats", "reviews");

      Assert.AreEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
    }
    
   
  }
}
