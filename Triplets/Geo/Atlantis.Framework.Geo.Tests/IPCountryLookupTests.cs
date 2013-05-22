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
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("GeoIP.dat")]
  public class IPCountryLookupTests
  {
    [TestMethod]
    public void IPCountryLookupRequestDataProperties()
    {
      IPCountryLookupRequestData request = new IPCountryLookupRequestData("127.0.0.1");
      Assert.AreEqual("127.0.0.1", request.IpAddress);
      XElement.Parse(request.ToXML());
    }

    [TestMethod]
    public void IPCountryLookupResponseNull()
    {
      IPCountryLookupResponseData response = IPCountryLookupResponseData.FromCountry(null);
      Assert.AreEqual(false, response.CountryFound);
      Assert.AreEqual("--", response.CountryCode);
    }

    [TestMethod]
    public void IPCountryLookupResponseUnknownCountry()
    {
      IPCountryLookupResponseData response = IPCountryLookupResponseData.FromCountry("--");
      Assert.AreEqual(false, response.CountryFound);
      Assert.AreEqual("--", response.CountryCode);
    }

    [TestMethod]
    public void IPCountryLookupResponseCountry()
    {
      IPCountryLookupResponseData response = IPCountryLookupResponseData.FromCountry("us");
      Assert.AreEqual(true, response.CountryFound);
      Assert.AreEqual("us", response.CountryCode);
      Assert.IsNull(response.GetException());
      XElement.Parse(response.ToXML());
    }

    const int _REQUESTTYPE = 697;

    [TestMethod]
    public void CountryLookupBasic()
    {
      ValidateIPCountry("97.74.104.201", "us");
    }

    private void ValidateIPCountry(string ipAddress, string expectedCountry)
    {
      IPCountryLookupRequestData request = new IPCountryLookupRequestData(ipAddress);
      IPCountryLookupResponseData response = (IPCountryLookupResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(response.CountryFound);

      string message = string.Format("{0} returned '{1}': expected '{2}'.", ipAddress, response.CountryCode, expectedCountry);
      Assert.AreEqual(expectedCountry, response.CountryCode, message);
    }

    [TestMethod]
    public void CountryLookupInternalIndia()
    {
      ValidateIPCountry("182.50.145.33", "in");
      ValidateIPCountry("182.94.14.140", "in");
      ValidateIPCountry("172.29.32.201", "in");
      ValidateIPCountry("172.29.37.201", "us");
      ValidateIPCountry("182.50.145.39", "sg");
    }

    [TestMethod]
    public void CountryLookupReplacedGBwithUK()
    {
      /// This test ensures that a developer that updates the CountryCodes array in GeoDataFileBase.cs remembers to
      /// update 'GB' to 'UK' in the array.
      IPCountryLookupRequestData request = new IPCountryLookupRequestData("5.61.32.40");
      var response = (IPCountryLookupResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreNotEqual("gb", response.CountryCode, "When updating the GeoDataFileBase country code array, please ensure 'GB' is changed to 'UK'.");
    }

    [TestMethod]
    public void CountryLookupSpotCheck()
    {
      ValidateIPCountry("5.158.255.220", "fr");
      ValidateIPCountry("5.2.96.40", "uk");
      ValidateIPCountry("1.179.3.3", "au");
      ValidateIPCountry("148.204.3.3", "mx");
      ValidateIPCountry("24.36.3.3", "ca");
      ValidateIPCountry("1.4.16.3", "cn");
    }

    private void ReloadCountryData()
    {
      Type requestType = typeof(Atlantis.Framework.Geo.Impl.IPCountryLookupRequest);
      MethodInfo loadCountryDataMethod = requestType.GetMethod("LoadCountryData", BindingFlags.Static | BindingFlags.NonPublic);
      loadCountryDataMethod.Invoke(null, null);
    }

    [TestMethod]
    public void CountryLookupMissingFile()
    {
      IPLookupDataFiles.CountryFile = @"c:\missing.dat";
      IPLookupDataFiles.PathType = IPLookupPathTypes.DirectoryPath;
      ReloadCountryData();

      IPCountryLookupResponseData response;

      try
      {
        IPCountryLookupRequestData request = new IPCountryLookupRequestData("182.50.145.39");
        response = (IPCountryLookupResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      }
      finally
      {
        IPLookupDataFiles.CountryFile = @"GeoIP.dat";
        IPLookupDataFiles.PathType = IPLookupPathTypes.AssemblyLocation;
        ReloadCountryData();
      }

      Assert.IsFalse(response.CountryFound);
    }

  }
}
