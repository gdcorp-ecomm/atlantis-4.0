using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.NewsCenter.Interface;

namespace Atlantis.Framework.NewsCenter.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.NewsCenter.Impl.dll")]
  public class ReleaseByDateAndTypeTests
  {
    [TestMethod]
    public void CachingByDateOnly()
    {
      DateTime now = DateTime.Now;
      DateTime inABit = DateTime.Now.AddMinutes(4);

      ReleaseByDateAndTypeRequestData request1 = new ReleaseByDateAndTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, now, "1");
      ReleaseByDateAndTypeRequestData request2 = new ReleaseByDateAndTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, inABit, "1");

      Assert.AreEqual(request1.GetCacheMD5(), request2.GetCacheMD5());

      ReleaseByDateAndTypeResponseData response1 = (ReleaseByDateAndTypeResponseData)DataCache.DataCache.GetProcessRequest(request1, 631);
      ReleaseByDateAndTypeResponseData response2 = (ReleaseByDateAndTypeResponseData)DataCache.DataCache.GetProcessRequest(request2, 631);

      Assert.AreEqual(response1, response2);
    }

    [TestMethod]
    public void CachingDifferentTypes()
    {
      DateTime now = DateTime.Now;

      ReleaseByDateAndTypeRequestData request1 = new ReleaseByDateAndTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, now, "1");
      ReleaseByDateAndTypeRequestData request2 = new ReleaseByDateAndTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, now, "2");

      Assert.AreNotEqual(request1.GetCacheMD5(), request2.GetCacheMD5());

      ReleaseByDateAndTypeResponseData response1 = (ReleaseByDateAndTypeResponseData)DataCache.DataCache.GetProcessRequest(request1, 631);
      ReleaseByDateAndTypeResponseData response2 = (ReleaseByDateAndTypeResponseData)DataCache.DataCache.GetProcessRequest(request2, 631);

      Assert.AreNotEqual(response1, response2);
    }


    [TestMethod]
    public void EnumerateUrlPaths()
    {
      ReleaseByDateAndTypeRequestData request = new ReleaseByDateAndTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, DateTime.Now, "1");
      ReleaseByDateAndTypeResponseData response = (ReleaseByDateAndTypeResponseData)DataCache.DataCache.GetProcessRequest(request, 631);

      foreach (NewsRelease release in response.NewsReleases)
      {
        Console.WriteLine(release.UrlPath);
      }
    }

    [TestMethod]
    public void GetReleaseByUrlPath()
    {
      string url = "help-wanted-go-daddy-hits-hiring-milestone";
      ReleaseByDateAndTypeRequestData request = new ReleaseByDateAndTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, DateTime.Now, "1");
      ReleaseByDateAndTypeResponseData response = (ReleaseByDateAndTypeResponseData)DataCache.DataCache.GetProcessRequest(request, 631);
      NewsRelease release = response.GetReleaseByUrlPath(url);
      Assert.IsNotNull(release);
    }

    [TestMethod]
    public void GetReleaseByUrlPathMixedCase()
    {
      string url = "HELP-wanted-go-daddy-hits-hiring-milestone";
      ReleaseByDateAndTypeRequestData request = new ReleaseByDateAndTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, DateTime.Now, "1");
      ReleaseByDateAndTypeResponseData response = (ReleaseByDateAndTypeResponseData)DataCache.DataCache.GetProcessRequest(request, 631);
      NewsRelease release = response.GetReleaseByUrlPath(url);
      Assert.IsNotNull(release);
    }

    [TestMethod]
    public void GetReleaseByUrlPathMiss()
    {
      string url = "arghwhatverbluegreensturff";
      ReleaseByDateAndTypeRequestData request = new ReleaseByDateAndTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, DateTime.Now, "1");
      ReleaseByDateAndTypeResponseData response = (ReleaseByDateAndTypeResponseData)DataCache.DataCache.GetProcessRequest(request, 631);
      NewsRelease release = response.GetReleaseByUrlPath(url);
      Assert.IsNull(release);
    }

    [TestMethod]
    public void GetReleaseById()
    {
      string id = "214";
      ReleaseByDateAndTypeRequestData request = new ReleaseByDateAndTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, DateTime.Now, "1");
      ReleaseByDateAndTypeResponseData response = (ReleaseByDateAndTypeResponseData)DataCache.DataCache.GetProcessRequest(request, 631);
      NewsRelease release = response.GetReleaseById(id);
      Assert.IsNotNull(release);
    }

    [TestMethod]
    public void GetReleaseByIdMiss()
    {
      string id = "thisisnotanvalidid";
      ReleaseByDateAndTypeRequestData request = new ReleaseByDateAndTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, DateTime.Now, "1");
      ReleaseByDateAndTypeResponseData response = (ReleaseByDateAndTypeResponseData)DataCache.DataCache.GetProcessRequest(request, 631);
      NewsRelease release = response.GetReleaseById(id);
      Assert.IsNull(release);
    }

    [TestMethod]
    public void FindReleasesByStatus()
    {
      ReleaseByDateAndTypeRequestData request = new ReleaseByDateAndTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, DateTime.Now, "1");
      ReleaseByDateAndTypeResponseData response = (ReleaseByDateAndTypeResponseData)DataCache.DataCache.GetProcessRequest(request, 631);
      List<NewsRelease> status1Releases = response.FindAll(n => n.StatusId == "1");
      foreach (var release in status1Releases)
      {
        Assert.AreEqual("1", release.StatusId);
      }
    }

    [TestMethod]
    public void FindReleasesByWordInTitle()
    {
      ReleaseByDateAndTypeRequestData request = new ReleaseByDateAndTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, DateTime.Now, "1");
      ReleaseByDateAndTypeResponseData response = (ReleaseByDateAndTypeResponseData)DataCache.DataCache.GetProcessRequest(request, 631);
      List<NewsRelease> status1Releases = response.FindAll(n => n.Title.ToLowerInvariant().Contains("godaddy"));
      foreach (var release in status1Releases)
      {
        Assert.IsTrue(release.Title.ToLowerInvariant().Contains("godaddy"));
      }
    }

    [TestMethod]
    public void ExternalFeatureLink()
    {
      ReleaseByDateAndTypeRequestData request = new ReleaseByDateAndTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, DateTime.Now, "1");
      ReleaseByDateAndTypeResponseData response = (ReleaseByDateAndTypeResponseData)DataCache.DataCache.GetProcessRequest(request, 631);
      List<NewsRelease> status1Releases = response.FindAll(n => !string.IsNullOrEmpty(n.ExternalFeatureLink));
      Assert.IsTrue(status1Releases.Count > 0);      
    }
  }
}
