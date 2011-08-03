using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.Document.Interface;

namespace Atlantis.Framework.Document.Tests
{
  [TestClass]
  public class DocumentTests
  {
    public DocumentTests()
    {}

    public TestContext TestContext { get; set; }

    #region Additional test attributes
    
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetUtosDocumentTest()
    {
      var request = new DocumentRequestData("857527", string.Empty, string.Empty, string.Empty, 1, 1, "utos"); 
      var response = Engine.Engine.ProcessRequest(request, 58) as DocumentResponseData;

      Assert.IsNotNull(response);
      Assert.IsNull(response.GetException());
      Assert.IsFalse(string.IsNullOrWhiteSpace(response.Html));
      Assert.IsTrue(response.Html.Contains("<html>"));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetFakeDocumentTest()
    {
      var request = new DocumentRequestData("857527", string.Empty, string.Empty, string.Empty, 1, 1, "03kq3dl3(@dk_k");
      var response = Engine.Engine.ProcessRequest(request, 58) as DocumentResponseData;

      Assert.IsNotNull(response);
      Assert.IsNull(response.GetException());
      Assert.IsFalse(string.IsNullOrWhiteSpace(response.Html));
      Assert.IsFalse(response.Html.Contains("<html>"));
    }
  }
}
