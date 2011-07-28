using Atlantis.Framework.GetAccountXML.Interface;
using Atlantis.Framework.Interface;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.GetAccountXML.Tests
{
  [TestClass]
  public class GetAccountXMLTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetAccountXMLBasic()
    {
      GetAccountXMLRequestData requestData = new GetAccountXMLRequestData("847235", "http://localhost",
        "", "", 0, "383392", "hosting", "billing", 0, 1);
      GetAccountXMLResponseData responseData = (GetAccountXMLResponseData)Engine.Engine.ProcessRequest(requestData, 74);
      Assert.AreEqual("<Bonsai xmlns=\"#Bonsai\"><Bonsai PrivateLabelID=\"1\" ResourceID=\"17c5c91d-49b3-11df-b65b-005056956427\"", responseData.AccountXML.Substring(0, 100), false);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [ExpectedException(typeof(AtlantisException))]
    public void CheckException()
    {
      GetAccountXMLRequestData requestData = new GetAccountXMLRequestData("ak5", "http://localhost",
        "", "", 0, "12345678-4af2-11de-baa9-005056956427", "sharedhosting", "orion", 0, 1);
      GetAccountXMLResponseData responseData = (GetAccountXMLResponseData)Engine.Engine.ProcessRequest(requestData, 74);
    }
  }
}
