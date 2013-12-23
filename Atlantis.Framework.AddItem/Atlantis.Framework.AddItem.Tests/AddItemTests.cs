using System;
using System.Xml.Linq;
using Atlantis.Framework.AddItem.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AddItem.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.AddItem.Impl.dll")]
  public class AddItemTests
  {
    [TestMethod]
    public void BasicAddItem()
    {
      AddItemRequestData request = new AddItemRequestData("832652", string.Empty, string.Empty, string.Empty, 0, "127.0.0.2");
      request.AddItem("58", "1");

      AddItemResponseData response = (AddItemResponseData)Engine.Engine.ProcessRequest(request, 4);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void AddItemWithCustomXmlString()
    {
      AddItemRequestData request = new AddItemRequestData("832652", string.Empty, string.Empty, string.Empty, 0, "127.0.0.2");
      request.AddItem("58", "1", "<domain name=\"blue.com\" />");
      Assert.IsNotNull(request.ToXML());
    }

    [TestMethod]
    public void SplitTestAddItem()
    {
        AddItemRequestData request = new AddItemRequestData("832652", string.Empty, string.Empty, string.Empty, 0, "127.0.0.2");
        XElement element = new XElement("NoFreeDBP");
        element.Add(new XAttribute("noFreeDBP", "0"));
        request.AddSplitTestItem(element);

        AddItemResponseData response = (AddItemResponseData)Engine.Engine.ProcessRequest(request, 4);
        Assert.IsTrue(response.IsSuccess);
    }
  }
}
