using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Tokens.Interface;
using Atlantis.Framework.Tokens.Tests.Handlers;
using Atlantis.Framework.Tokens.Tests.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.ProviderContainer.Impl;

namespace Atlantis.Framework.Tokens.Tests
{
  [TestClass]
  public class TokenTests
  {
    [TestMethod]
    public void TokenParsing()
    {
      TokenManager.Clear();
      TokenManager.RegisterTokenHandler(new SimpleTokenHandler());
      string inputText = TestData.GetTextDataResource("inputdata1.txt");

      List<Match> matches = TokenManager.ParseTokenStrings(inputText);
      Assert.IsTrue(matches.Count > 0);
    }

    [TestMethod]
    public void TokenManagerAutoLoad()
    {
      TokenManager.Clear();
      TokenManager.AutoRegisterTokenHandlers(Assembly.GetExecutingAssembly());
      var handlers = TokenManager.GetRegisteredTokenHandlers();
      Assert.IsTrue(handlers.Count > 0);
    }

    [TestMethod]
    public void TokenManagerAutoLoadInputData1()
    {
      TokenManager.Clear();
      TokenManager.AutoRegisterTokenHandlers(Assembly.GetExecutingAssembly());
      string inputText = TestData.GetTextDataResource("inputdata1.txt");

      IProviderContainer container = new ObjectProviderContainer();
      container.RegisterProvider<IDebugContext, MockDebug>();

      string outputText;
      TokenEvaluationResult result = TokenManager.ReplaceTokens(inputText, container, out outputText);

      //because we do not have a samplejson handler, we should have errors
      Assert.AreEqual(TokenEvaluationResult.Errors, result);

      //no more tokens should exist
      List<Match> matches = TokenManager.ParseTokenStrings(outputText);
      Assert.IsTrue(matches.Count == 0);

      IDebugContext debug = container.Resolve<IDebugContext>();
      Assert.IsTrue(debug.GetDebugTrackingData().Count > 0);
    }

  }
}
