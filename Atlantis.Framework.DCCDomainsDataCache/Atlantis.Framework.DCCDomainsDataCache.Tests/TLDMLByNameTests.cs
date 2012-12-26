using Atlantis.Framework.DCCDomainsDataCache.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DCCDomainsDataCache.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DCCDomainsDataCache.Impl.dll")]
  public class TLDMLByNameTests
  {
    const int _GETBYNAMEREQUEST = 634;

    [TestMethod]
    public void TLDMLFoundUpperCase()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "COM.AU");
      var response = DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
    }

    [TestMethod]
    public void TLDMLFoundLowerCase()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "com.au");
      var response = DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void TLDMLNotFound()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "INFO");
      var response = DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void TLDMLNull()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, null);
      var response = DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void TLDMLEmptyString()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, string.Empty);
      var response = DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
    }

  }
}
