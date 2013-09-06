using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Render.Pipeline;
using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Tokens.Tests.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Tokens.Interface;
using Atlantis.Framework.Tokens.Tests.Handlers;
using Atlantis.Framework.Tokens.Tests.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Tokens.Tests
{
  [TestClass]
  public class TokenTests
  {
    [TestMethod]
    public void TokenParsing()
    {
      TokenManager.ClearHandlers();

      TokenManager.RegisterTokenHandler(new SimpleTokenHandler());
      string inputText = TestData.GetTextDataResource("inputdata1.txt");

      List<Match> matches = TokenManager.ParseTokenStrings(inputText);

      Assert.IsTrue(matches.Count > 0);
    }

    [TestMethod]
    public void TokenManagerAutoLoad()
    {
      TokenManager.ClearHandlers();

      TokenManager.AutoRegisterTokenHandlers(Assembly.GetExecutingAssembly());
      var handlers = TokenManager.GetRegisteredTokenHandlers();
      Assert.IsTrue(handlers.Count > 0);
    }

    [TestMethod]
    public void TokenManagerAutoLoadInputData1()
    {
      TokenManager.ClearHandlers();

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

    [TestMethod]
    public void TokenManagerEvaluateError()
    {
      TokenManager.ClearHandlers();

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
      TokenManager.ClearHandlers();

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

    [TestMethod]
    public void TokenRenderHandlerInputData()
    {
      TokenManager.ClearHandlers();

      TokenManager.AutoRegisterTokenHandlers(Assembly.GetExecutingAssembly());
      string inputText = TestData.GetTextDataResource("inputdata1.txt");


      IProviderContainer container = new ObjectProviderContainer();
      container.RegisterProvider<IDebugContext, MockDebug>();
      
      RenderPipelineManager renderPipelineManager = new RenderPipelineManager();
      renderPipelineManager.AddRenderHandler(new TokenRenderHandler());

      IRenderContent renderContent = new TestRenderContent(inputText);

      IProcessedRenderContent processedRenderContent = renderPipelineManager.RenderContent(renderContent, container);

      //no more tokens should exist
      List<Match> matches = TokenManager.ParseTokenStrings(processedRenderContent.Content);
      Assert.IsTrue(matches.Count == 0);

      IDebugContext debug = container.Resolve<IDebugContext>();
      Assert.IsTrue(debug.GetDebugTrackingData().Count > 0);
    }
  }
}