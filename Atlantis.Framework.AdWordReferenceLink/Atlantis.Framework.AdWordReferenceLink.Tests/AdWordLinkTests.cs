using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.AdWordReferenceLink.Interface;

namespace Atlantis.Framework.AdWordReferenceLink.Tests
{
  [TestClass]
  public class AdWordLinkTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    [DeploymentItem("Atlantis.Framework.AdWordReferenceLink.Impl.dll")]
    public void GetAdWordsLink()
    {
      AdWordReferenceLinkRequestData requestData = new AdWordReferenceLinkRequestData("856907", string.Empty, string.Empty, string.Empty, 0, "5317");

      AdWordReferenceLinkResponseData response = (AdWordReferenceLinkResponseData)Engine.Engine.ProcessRequest(requestData, 708);

      Console.WriteLine(response.AdWordReferenceLink);
    }
  }
}
