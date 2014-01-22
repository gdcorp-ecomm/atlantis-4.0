using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Atlantis.Framework.Geo.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Geo.Impl.dll")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  public class StateTests
  {
    const int _STATEREQUESTTYPE = 665;

    [TestMethod]
    public void CreateStateObject()
    {
      string cacheXml = "<state id=\"74\" code=\"AL\" name=\"Alabama\" country=\"226\"  />";
      XElement cacheElement = XElement.Parse(cacheXml);

      State state = State.FromCacheElement(cacheElement);
      Assert.IsNotNull(state);
      Assert.AreEqual(74, state.Id);
      Assert.AreEqual("AL", state.Code);
      Assert.AreEqual("Alabama", state.Name);
    }

    [TestMethod]
    public void StateRequestCacheKeySame()
    {
      StateRequestData request = new StateRequestData(226);
      StateRequestData request2 = new StateRequestData(226);
      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void StateRequestCacheKeyDifferent()
    {
      StateRequestData request = new StateRequestData(226);
      StateRequestData request2 = new StateRequestData(200);
      Assert.AreNotEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void StateRequestXml()
    {
      StateRequestData request = new StateRequestData(226);
      string xml = request.ToXML();
      XElement.Parse(xml);
    }

    [TestMethod]
    public void StateResponseException()
    {
      AtlantisException exception = new AtlantisException("StateTests.StateResponseException", 0, "TestMessage", "TestData");
      StateResponseData response = StateResponseData.FromException(exception);
      Assert.IsNotNull(response.GetException());
      Assert.AreEqual(0, response.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(XmlException))]
    public void StateResponseBadXml()
    {
      string cacheXml = "<state id=\"1\" code=\"AL\" name=\"Alabama\" country=\"226\"";
      StateResponseData response = StateResponseData.FromDataCacheXml(cacheXml);
    }

    [TestMethod]
    public void StateResponseValid()
    {
      string cacheXml = "<states><state id=\"1\" code=\"AL\" name=\"Alabama\" country=\"226\" /></states>";
      StateResponseData response = StateResponseData.FromDataCacheXml(cacheXml);
      Assert.AreNotEqual(0, response.Count);
    }


    [TestMethod]
    public void StateResponseNoData()
    {
      string cacheXml = "<states></states>";
      StateResponseData response = StateResponseData.FromDataCacheXml(cacheXml);
      Assert.AreEqual(0, response.Count);
      Assert.AreEqual(StateResponseData.Empty, response);
    }

    [TestMethod]
    public void GetStates()
    {
      StateRequestData request = new StateRequestData(226);
      StateResponseData response = (StateResponseData)Engine.Engine.ProcessRequest(request, _STATEREQUESTTYPE);
      Assert.AreNotEqual(0, response.Count);
      Assert.AreEqual(response.States.Count(), response.Count);

      string xml = response.ToXML();
      XElement.Parse(xml);
    }

    [TestMethod]
    public void GetNoStates()
    {
      StateRequestData request = new StateRequestData(219);
      StateResponseData response = (StateResponseData)Engine.Engine.ProcessRequest(request, _STATEREQUESTTYPE);
      Assert.AreEqual(0, response.Count);
      Assert.AreEqual(StateResponseData.Empty, response);
    }


    [TestMethod]
    public void GetStateByName()
    {
      StateRequestData request = new StateRequestData(226);
      StateResponseData response = (StateResponseData)Engine.Engine.ProcessRequest(request, _STATEREQUESTTYPE);
      State state = response.FindStateByName("arizona");
      Assert.IsNotNull(state);
    }

    [TestMethod]
    public void GetStateById()
    {
      StateRequestData request = new StateRequestData(226);
      StateResponseData response = (StateResponseData)Engine.Engine.ProcessRequest(request, _STATEREQUESTTYPE);
      State state = response.FindStateById(70);
      Assert.IsNotNull(state);
    }

    [TestMethod]
    public void GetStateByCode()
    {
      StateRequestData request = new StateRequestData(226);
      StateResponseData response = (StateResponseData)Engine.Engine.ProcessRequest(request, _STATEREQUESTTYPE);
      State state = response.FindStateByCode("az");
      Assert.IsNotNull(state);

      State state2 = response.FindStateByCode("Az");
      Assert.AreEqual(state.Id, state2.Id, "Different case incorrectly returned different states.");
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void GetCountriesExecuteException()
    {
      var request = new InvalidRequestData();
      var response = (StateResponseData)Engine.Engine.ProcessRequest(request, _STATEREQUESTTYPE);
    }




  }
}
