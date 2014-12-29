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
      AddItemRequestData request = new AddItemRequestData("852816", string.Empty, string.Empty, string.Empty, 0, "172.17.123.179");
      request.AddItem("58", "1");
      request.AuthToken ="eyJhbGciOiAiUlMyNTYiLCAia2lkIjogIjRyLTcwZjJzZncifQ.eyJmaXJzdG5hbWUiOiAiQW51cGFtYSIsICJmYWN0b3JzIjogeyJrX3B3IjogMTQxOTYwNjY0N30sICJsYXN0bmFtZSI6ICJSYW11IiwgInNob3BwZXJJZCI6ICI4NTI4MTYiLCAianRpIjogIjVyY280M3MxZ1o4cXBwcWI5cEt5TWciLCAiZXhwIjogMTQxOTY0OTg0NywgInBsaWQiOiAiMSIsICJpYXQiOiAxNDE5NjA2NjQ3LCAidHlwIjogImlkcCJ9.Iwdt8H1YuEEBnMQP0gyh3HsVYqKQ97GdFZbPkMbuAXnRDwRuPRE229-Aexzc9DqJv0khC7ODuvze2teuE7OfZW2NMiOAxIFrvb0_mtLZeizRdZVEFq-LO9aYm4ssoQkBf95WQg670-mhLR6lDBgVr-MmcyfWvvj2a8B76086-2o";

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
