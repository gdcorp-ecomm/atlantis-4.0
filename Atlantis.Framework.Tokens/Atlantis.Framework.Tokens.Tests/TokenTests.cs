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
    private List<Match> ParseTokenStrings(string inputText)
    {
      MethodInfo method = typeof(TokenManager).GetMethod("ParseTokenStrings", BindingFlags.Static | BindingFlags.NonPublic);
      object[] parameters = new object[1] { inputText };
      object result = method.Invoke((object)null, parameters);
      return result as List<Match>;
    }

    private void ClearHandlers()
    {
      MethodInfo method = typeof(TokenManager).GetMethod("ClearHandlers", BindingFlags.Static | BindingFlags.NonPublic);
      object[] parameters = new object[0];
      method.Invoke((object)null, parameters);
    }

    [TestMethod]
    public void TokenParsing()
    {
      ClearHandlers();
      TokenManager.RegisterTokenHandler(new SimpleTokenHandler());
      string inputText = TestData.GetTextDataResource("inputdata1.txt");

      List<Match> matches = ParseTokenStrings(inputText);
      Assert.IsTrue(matches.Count > 0);
    }

    [TestMethod]
    public void TokenManagerAutoLoad()
    {
      ClearHandlers();
      TokenManager.AutoRegisterTokenHandlers(Assembly.GetExecutingAssembly());
      var handlers = TokenManager.GetRegisteredTokenHandlers();
      Assert.IsTrue(handlers.Count > 0);
    }

    [TestMethod]
    public void TokenManagerAutoLoadInputData1()
    {
      ClearHandlers();
      TokenManager.AutoRegisterTokenHandlers(Assembly.GetExecutingAssembly());
      string inputText = TestData.GetTextDataResource("inputdata1.txt");

      IProviderContainer container = new ObjectProviderContainer();
      container.RegisterProvider<IDebugContext, MockDebug>();

      string outputText;
      TokenEvaluationResult result = TokenManager.ReplaceTokens(inputText, container, out outputText);

      //because we do not have a samplejson handler, we should have errors
      Assert.AreEqual(TokenEvaluationResult.Errors, result);

      //no more tokens should exist
      List<Match> matches = ParseTokenStrings(outputText);
      Assert.IsTrue(matches.Count == 0);

      IDebugContext debug = container.Resolve<IDebugContext>();
      Assert.IsTrue(debug.GetDebugTrackingData().Count > 0);
    }

    [TestMethod]
    public void TokenManagerEvaluateError()
    {
      ClearHandlers();
      TokenManager.RegisterTokenHandler(new ErrorTokenHandler());
      string inputText = TestData.GetTextDataResource("inputdata1.txt");

      IProviderContainer container = new ObjectProviderContainer();
      container.RegisterProvider<IDebugContext, MockDebug>();

      string outputText;
      TokenEvaluationResult result = TokenManager.ReplaceTokens(inputText, container, out outputText);

      //because we do not have a samplejson handler, we should have errors
      Assert.AreEqual(TokenEvaluationResult.Errors, result);

      IDebugContext debug = container.Resolve<IDebugContext>();
      Assert.IsTrue(debug.GetDebugTrackingData().Count > 0);
    }

    [TestMethod]
    public void TokensInEncodedStrings()
    {
      ClearHandlers();
      TokenManager.RegisterTokenHandler(new XmlTokenHandler());
      string inputText = TestData.GetTextDataResource("inputdata2.txt");

      IProviderContainer container = new ObjectProviderContainer();
      container.RegisterProvider<IDebugContext, MockDebug>();

      ITokenEncoding tokenEncoding = new QuoteEncoding();

      string outputText;
      TokenEvaluationResult result = TokenManager.ReplaceTokens(inputText, container, tokenEncoding, out outputText);

      // output should contain encoded string 
      Assert.AreEqual("\\\"Success!\\\"", outputText);

    }

  }
}
