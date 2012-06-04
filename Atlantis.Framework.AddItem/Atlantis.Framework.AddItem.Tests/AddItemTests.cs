using System;
using Atlantis.Framework.AddItem.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AddItem.Tests
{
  [TestClass]
  public class AddItemTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.AddItem.Impl.dll")]
    public void BasicAddItem()
    {
      AddItemRequestData request = new AddItemRequestData("832652", string.Empty, string.Empty, string.Empty, 0, "127.0.0.2");
      request.AddItem("58", "1");

      AddItemResponseData response = (AddItemResponseData)Engine.Engine.ProcessRequest(request, 4);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.AddItem.Impl.dll")]
    public void AddItemWithCustomXmlString()
    {
      AddItemRequestData request = new AddItemRequestData("832652", string.Empty, string.Empty, string.Empty, 0, "127.0.0.2");
      request.AddItem("58", "1", "<domain name=\"blue.com\" />");
      Assert.IsNotNull(request.ToXML());
    }
  }
}
