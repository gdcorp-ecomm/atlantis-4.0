using System.IO;
using System.Reflection;
using System.Web;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.CH.SiteContext.Tests
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.CH.SiteContext.dll")]
  public class HasISCConditionHandlerTests
  {
    public HasISCConditionHandlerTests()
    {
      ConditionHandlerManager.AutoRegisterConditionHandlers(Assembly.GetExecutingAssembly());
    }

    private ExpressionParserManager GetExpressionParserManager(IProviderContainer container)
    {
      var expressionParserManager = new ExpressionParserManager(container);
      expressionParserManager.EvaluateExpressionHandler += ConditionHandlerManager.EvaluateCondition;

      return expressionParserManager;
    }

    private void InitializeHttpContext(string isc)
    {
      HttpContext.Current = new HttpContext(
        new HttpRequest("ssl-sertificates.aspx", "http://dev.godaddy-com.ide", "isc=" + isc),
        new HttpResponse(new StringWriter()));  
    }

    private const string IscExpression = "hasISC()";

    [TestMethod]
    public void HasIscIsTrue()
    {
      InitializeHttpContext("gtfo1234");

      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();

      var expressionParserManager = GetExpressionParserManager(container);

      bool result = expressionParserManager.EvaluateExpression(IscExpression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void HasIscIsEmptyString()
    {
      InitializeHttpContext(string.Empty);

      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();

      var expressionParserManager = GetExpressionParserManager(container);

      bool result = expressionParserManager.EvaluateExpression(IscExpression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void HasIscWithNoQueryString()
    {
      HttpContext.Current = new HttpContext(
        new HttpRequest("ssl-sertificates.aspx", "http://dev.godaddy-com.ide", string.Empty),
        new HttpResponse(new StringWriter()));

      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();

      var expressionParserManager = GetExpressionParserManager(container);

      bool result = expressionParserManager.EvaluateExpression(IscExpression);
      Assert.IsFalse(result);
    }
  }
}
