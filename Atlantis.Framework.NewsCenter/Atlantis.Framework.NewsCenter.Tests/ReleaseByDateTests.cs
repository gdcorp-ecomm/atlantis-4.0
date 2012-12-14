using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.NewsCenter.Interface;
using System.Threading;

namespace Atlantis.Framework.NewsCenter.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.NewsCenter.Impl.dll")]
  public class ReleaseByDateTests
  {
    [TestMethod]
    public void CachingByDateOnly()
    {
      DateTime now = DateTime.Now;
      DateTime inABit = DateTime.Now.AddMinutes(4);

      ReleaseByDateRequestData request1 = new ReleaseByDateRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, now);
      ReleaseByDateRequestData request2 = new ReleaseByDateRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, inABit);

      Assert.AreEqual(request1.GetCacheMD5(), request2.GetCacheMD5());

      ReleaseByDateResponseData response1 = (ReleaseByDateResponseData)DataCache.DataCache.GetProcessRequest(request1, 629);
      ReleaseByDateResponseData response2 = (ReleaseByDateResponseData)DataCache.DataCache.GetProcessRequest(request2, 629);

      Assert.AreEqual(response1, response2);
    }

    [TestMethod]
    public void EnumerateUrlPaths()
    {
      ReleaseByDateRequestData request = new ReleaseByDateRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, DateTime.Now);
      ReleaseByDateResponseData response = (ReleaseByDateResponseData)DataCache.DataCache.GetProcessRequest(request, 629);

      foreach (NewsRelease release in response.NewsReleases)
      {
        Console.WriteLine(release.UrlPath);
      }
    }

    [TestMethod]
    public void GetReleaseByUrlPath()
    {
      string url = "help-wanted-go-daddy-hits-hiring-milestone";
      ReleaseByDateRequestData request = new ReleaseByDateRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, DateTime.Now);
      ReleaseByDateResponseData response = (ReleaseByDateResponseData)DataCache.DataCache.GetProcessRequest(request, 629);
      NewsRelease release = response.GetReleaseByUrlPath(url);
      Assert.IsNotNull(release);
    }

    [TestMethod]
    public void GetReleaseByUrlPathMixedCase()
    {
      string url = "HELP-wanted-go-daddy-hits-hiring-milestone";
      ReleaseByDateRequestData request = new ReleaseByDateRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, DateTime.Now);
      ReleaseByDateResponseData response = (ReleaseByDateResponseData)DataCache.DataCache.GetProcessRequest(request, 629);
      NewsRelease release = response.GetReleaseByUrlPath(url);
      Assert.IsNotNull(release);
    }

    [TestMethod]
    public void GetReleaseByUrlPathMiss()
    {
      string url = "arghwhatverbluegreensturff";
      ReleaseByDateRequestData request = new ReleaseByDateRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, DateTime.Now);
      ReleaseByDateResponseData response = (ReleaseByDateResponseData)DataCache.DataCache.GetProcessRequest(request, 629);
      NewsRelease release = response.GetReleaseByUrlPath(url);
      Assert.IsNull(release);
    }

    [TestMethod]
    public void GetReleaseById()
    {
      string id = "214";
      ReleaseByDateRequestData request = new ReleaseByDateRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, DateTime.Now);
      ReleaseByDateResponseData response = (ReleaseByDateResponseData)DataCache.DataCache.GetProcessRequest(request, 629);
      NewsRelease release = response.GetReleaseById(id);
      Assert.IsNotNull(release);
    }

    [TestMethod]
    public void GetReleaseByIdMiss()
    {
      string id = "thisisnotanvalidid";
      ReleaseByDateRequestData request = new ReleaseByDateRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, DateTime.Now);
      ReleaseByDateResponseData response = (ReleaseByDateResponseData)DataCache.DataCache.GetProcessRequest(request, 629);
      NewsRelease release = response.GetReleaseById(id);
      Assert.IsNull(release);
    }

  }
}
