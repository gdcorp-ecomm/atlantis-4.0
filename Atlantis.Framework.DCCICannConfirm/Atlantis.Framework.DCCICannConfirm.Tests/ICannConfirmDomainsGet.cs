using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.DCCICannConfirm.Interface;

namespace Atlantis.Framework.DCCICannConfirm.Tests
{
  [TestClass]
  public class ICannConfirmDomainsGet
  {
    [TestMethod]
    public void Test1StandardGet()
    {
      DCCICannConfirmRequestData request = new DCCICannConfirmRequestData("Test", String.Empty, String.Empty,
        "292FB49F-66E1-47A3-892C-30528C33C2F3", String.Empty, String.Empty, 1, String.Empty);
      DCCICannConfirmResponseData response = (DCCICannConfirmResponseData)Engine.Engine.ProcessRequest(request, 608);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
