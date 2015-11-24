using Atlantis.Framework.Providers.RenderPipeline;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Testing.MockProviders;
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
    private IProviderContainer _container;
    private ITokenProvider _tokenProvider;

    private TokenEvaluationResult _result;

    [TestInitialize]
    public void SetupTokenProvider()
    {
      _container = new MockProviderContainer();
      _container.RegisterProvider<IDebugContext, MockDebug>();
      _container.RegisterProvider<IRenderPipelineProvider, RenderPipelineProvider>();

      _tokenProvider = new TokenProvider(_container);
      _container.RegisterProvider<ITokenProvider, TokenProvider>();

      _result = TokenEvaluationResult.Errors;

      _tokenProvider.TokenReplaced += (tokenKey, tokenData, result) =>
      {
        _result = result;
      };
    }

    [TestMethod]
    public void TokenParsing()
    {
      TokenProvider.ClearHandlers();

      TokenProvider.RegisterTokenHandler(new SimpleTokenHandler());
      string inputText = TestData.GetTextDataResource("inputdata1.txt");

      List<Match> matches = TokenProvider.ParseTokenStrings(inputText);

      Assert.IsTrue(matches.Count > 0);
    }

    [TestMethod]
    public void TokenManagerAutoLoad()
    {
      TokenProvider.ClearHandlers();

      TokenProvider.AutoRegisterTokenHandlers(Assembly.GetExecutingAssembly());
      var handlers = TokenProvider.GetRegisteredTokenHandlers();
      Assert.IsTrue(handlers.Count > 0);
    }

    [TestMethod]
    public void TokenManagerAutoLoadInputData1()
    {
      TokenProvider.ClearHandlers();

      TokenProvider.AutoRegisterTokenHandlers(Assembly.GetExecutingAssembly());
      string inputText = TestData.GetTextDataResource("inputdata1.txt");

      

      string outputText;
      _tokenProvider.ReplaceTokens(inputText, out outputText);

      //because we do not have a samplejson handler, we should have errors
      Assert.AreEqual(TokenEvaluationResult.Errors, _result);

      //no more tokens should exist
      List<Match> matches = TokenProvider.ParseTokenStrings(outputText);
      Assert.IsTrue(matches.Count == 0);

      IDebugContext debug = _container.Resolve<IDebugContext>();
      Assert.IsTrue(debug.GetDebugTrackingData().Count > 0);
    }

    [TestMethod]
    public void TokenManagerEvaluateError()
    {
      TokenProvider.ClearHandlers();

      TokenProvider.RegisterTokenHandler(new ErrorTokenHandler());
      string inputText = TestData.GetTextDataResource("inputdata1.txt");

      string outputText;
      _tokenProvider.ReplaceTokens(inputText, out outputText);

      //because we do not have a samplejson handler, we should have errors
      Assert.AreEqual(TokenEvaluationResult.Errors, _result);

      IDebugContext debug = _container.Resolve<IDebugContext>();
      Assert.IsTrue(debug.GetDebugTrackingData().Count > 0);
    }

    [TestMethod]
    public void TokensInEncodedStrings()
    {
      TokenProvider.ClearHandlers();

      TokenProvider.RegisterTokenHandler(new XmlTokenHandler());
      string inputText = TestData.GetTextDataResource("inputdata2.txt");

      ITokenEncoding tokenEncoding = new QuoteEncoding();

      string outputText;
      _tokenProvider.ReplaceTokens(inputText, tokenEncoding, out outputText);

      // output should contain encoded string 
      Assert.AreEqual("\\\"Success!\\\"", outputText);

    }

    [TestMethod]
    public void TokenRenderHandlerInputData()
    {
      TokenProvider.ClearHandlers();

      TokenProvider.AutoRegisterTokenHandlers(Assembly.GetExecutingAssembly());
      string inputText = TestData.GetTextDataResource("inputdata1.txt");


      IRenderPipelineProvider renderPipelineProvider = _container.Resolve<IRenderPipelineProvider>();
      string output = renderPipelineProvider.RenderContent(inputText, new List<IRenderHandler> {new TokenRenderHandler()});

      //no more tokens should exist
      List<Match> matches = TokenProvider.ParseTokenStrings(output);
      Assert.IsTrue(matches.Count == 0);

      IDebugContext debug = _container.Resolve<IDebugContext>();
      Assert.IsTrue(debug.GetDebugTrackingData().Count > 0);
    }
  }
}