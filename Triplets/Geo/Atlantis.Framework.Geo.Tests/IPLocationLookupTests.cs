using Atlantis.Framework.Geo.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Xml.Linq;

namespace Atlantis.Framework.Geo.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Geo.Impl.dll")]
  [DeploymentItem("GeoIPCity.dat")]
  public class IPLocationLookupTests
  {
    [TestMethod]
    public void IpLocationInstanceTest()
    {
      var ipl = new IPLocation();
      Assert.IsTrue(ipl.CountryCode != null);
      Assert.IsTrue(ipl.City != null);
      Assert.IsTrue(ipl.PostalCode != null);
      Assert.IsTrue(ipl.Region != null);
      Assert.IsTrue(ipl.RegionName != null);
      Assert.IsTrue(ipl.MetroCode == 0);
      Assert.IsTrue(ipl.Latitude < 1);
      Assert.IsTrue(ipl.Longitude < 1);
    }

    [TestMethod]
    public void IPLocationLookupRequestDataProperties()
    {
      IPLocationLookupRequestData request = new IPLocationLookupRequestData("127.0.0.1");
      Assert.AreEqual("127.0.0.1", request.IpAddress);
      XElement.Parse(request.ToXML());
    }

    [TestMethod]
    public void IPLocationLookupResponseNull()
    {
      IPLocationLookupResponseData response = IPLocationLookupResponseData.FromIPLocation(null);
      Assert.AreEqual(false, response.LocationFound);
      Assert.AreEqual("--", response.Location.CountryCode);
    }

    [TestMethod]
    public void IPLocationLookupResponseNotFound()
    {
      IPLocationLookupResponseData response = IPLocationLookupResponseData.FromIPLocation(IPLocation.Unknown);
      Assert.AreEqual(IPLocation.Unknown, response.Location);
      Assert.AreEqual(false, response.LocationFound);
    }

    [TestMethod]
    public void IPLocationLookupResponseCountry()
    {
      IPLocation phoenix = new IPLocation();
      phoenix.City = "Phoenix";
      phoenix.CountryCode = "us";
      phoenix.Latitude = 30;
      phoenix.Longitude = -125;
      phoenix.MetroCode = 44;
      phoenix.PostalCode = "85260";
      phoenix.Region = "az";
      phoenix.RegionName = "Arizona";

      IPLocationLookupResponseData response = IPLocationLookupResponseData.FromIPLocation(phoenix);
      Assert.AreEqual(true, response.LocationFound);
      Assert.AreEqual("us", response.Location.CountryCode);
      Assert.IsNull(response.GetException());
      XElement.Parse(response.ToXML());
    }

    const int _REQUESTTYPE = 719;

    [TestMethod]
    public void LocationLookupBasic()
    {
      ValidateIPLocation("97.74.104.201", "us", "Scottsdale");
    }

    private void ValidateIPLocation(string ipAddress, string expectedCountry, string expectedCity = null)
    {
      IPLocationLookupRequestData request = new IPLocationLookupRequestData(ipAddress);
      IPLocationLookupResponseData response = (IPLocationLookupResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(response.LocationFound);

      string message = string.Format("{0} returned '{1}': expected '{2}'.", ipAddress, response.Location.CountryCode, expectedCountry);
      Assert.AreEqual(expectedCountry, response.Location.CountryCode, message);

      if (expectedCity != null)
      {
        message = string.Format("{0} returned '{1}': expected '{2}'.", ipAddress, response.Location.City, expectedCity);
        Assert.AreEqual(expectedCity, response.Location.City, message);
      }
    }

    [TestMethod]
    public void LocationLookupInternalIndia()
    {
      ValidateIPLocation("182.50.145.33", "in");
      ValidateIPLocation("182.94.14.140", "in");
      ValidateIPLocation("172.29.32.201", "in");
      ValidateIPLocation("172.29.37.201", "us");
      ValidateIPLocation("182.50.145.39", "sg");
    }

    [TestMethod]
    public void LocationLookupReplacedGBwithUK()
    {
      /// This test ensures that a developer that updates the CountryCodes array in GeoDataFileBase.cs remembers to
      /// update 'GB' to 'UK' in the array.
      IPLocationLookupRequestData request = new IPLocationLookupRequestData("5.61.32.40");
      var response = (IPLocationLookupResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreNotEqual("gb", response.Location.CountryCode, "When updating the GeoDataFileBase country code array, please ensure 'GB' is changed to 'UK'.");
    }

    [TestMethod]
    public void LocationLookupSpotCheck()
    {
      ValidateIPLocation("5.158.255.220", "fr");
      ValidateIPLocation("5.2.96.40", "uk");
      ValidateIPLocation("1.179.3.3", "au");
      ValidateIPLocation("148.204.3.3", "mx", "Mexico");
      ValidateIPLocation("24.36.3.3", "ca", "Hamilton");
      ValidateIPLocation("1.4.16.3", "cn", "Guangzhou");
    }

    private void ReloadCountryData()
    {
      Type requestType = typeof(Atlantis.Framework.Geo.Impl.IPLocationLookupRequest);
      MethodInfo loadCountryDataMethod = requestType.GetMethod("LoadLocationData", BindingFlags.Static | BindingFlags.NonPublic);
      loadCountryDataMethod.Invoke(null, null);
    }

    [TestMethod]
    public void LocationLookupMissingFile()
    {
      IPLookupDataFiles.LocationFile = @"c:\missing.dat";
      IPLookupDataFiles.PathType = IPLookupPathTypes.DirectoryPath;
      ReloadCountryData();

      IPLocationLookupResponseData response;

      try
      {
        IPLocationLookupRequestData request = new IPLocationLookupRequestData("182.50.145.39");
        response = (IPLocationLookupResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      }
      finally
      {
        IPLookupDataFiles.LocationFile = @"GeoIPCity.dat";
        IPLookupDataFiles.PathType = IPLookupPathTypes.AssemblyLocation;
        ReloadCountryData();
      }

      Assert.IsFalse(response.LocationFound);
    }
  }
}
