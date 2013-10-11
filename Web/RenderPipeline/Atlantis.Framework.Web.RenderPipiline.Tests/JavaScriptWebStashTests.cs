using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.RenderPipeline;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Web.RenderPipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Web.RenderPipiline.Tests
{
  [TestClass]
  public class JavaScriptWebStashTests
  {
    private static IProviderContainer _providerContainer;
    public static IProviderContainer ProviderContainer
    {
      get
      {
        if (_providerContainer == null)
        {
          _providerContainer = new MockProviderContainer();
          _providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
          _providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
          _providerContainer.RegisterProvider<IManagerContext, MockManagerContext>();

          _providerContainer.RegisterProvider<IRenderPipelineProvider, RenderPipelineProvider>();
        }

        return _providerContainer;
      }
    }

    [TestMethod]
    public void JavaScriptStashTestValidWithScript()
    {
      const string content = @"<body>
    <div>
      <atlantis:javascriptwebstash>
        <script type=""text/javascript"">
          var i = 3;
        </script>
      </atlantis:javascriptwebstash>
    </div>
    <div>
      <atlantis:javascriptwebstash>
        <script type=""text/javascript"">
          var k = 3;
        </script>
      </atlantis:javascriptwebstash>
    </div>
    <hr/>
  </body>";

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> {new JavaScriptWebStashRenderHandler()});

      var scriptContent = renderedContent.Substring(renderedContent.IndexOf("<hr/>", StringComparison.OrdinalIgnoreCase) + 5).TrimStart('\r', '\n').Trim();
      var scriptArray = scriptContent.Split(new[] { "<script" }, StringSplitOptions.None);

      Assert.IsTrue(!renderedContent.Contains("<atlantis:javascriptwebstash>") &&
                    !renderedContent.Contains("</atlantis:javascriptwebstash>") &&
                    scriptArray.Length == 3);
    }

    [TestMethod]
    public void JavaScriptStashTestValidEmpty()
    {
      const string content = @"<body>
    <div>
      <atlantis:javascriptwebstash>
      </atlantis:javascriptwebstash>
    </div>
    <div>
      <atlantis:javascriptwebstash>
      </atlantis:javascriptwebstash>
    </div>
    <hr/>
  </body>";

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new JavaScriptWebStashRenderHandler() });

      var scriptContent = renderedContent.Substring(renderedContent.IndexOf("<hr/>", StringComparison.OrdinalIgnoreCase) + 5).TrimStart('\r', '\n').Trim();

      Assert.IsTrue(!renderedContent.Contains("<atlantis:javascriptwebstash>") &&
                    !renderedContent.Contains("</atlantis:javascriptwebstash>") &&
                    scriptContent == "</body>");
    }

    [TestMethod]
    public void JavaScriptStashTestValidText()
    {
      const string content = @"<body>
    <div>
      <atlantis:javascriptwebstash>
abc
      </atlantis:javascriptwebstash>
    </div>
    <div>
      <atlantis:javascriptwebstash>
def
      </atlantis:javascriptwebstash>
    </div>
    <hr/>
  </body>";

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new JavaScriptWebStashRenderHandler() });

      var scriptContent = renderedContent.Substring(renderedContent.IndexOf("<hr/>", StringComparison.OrdinalIgnoreCase) + 5).TrimStart('\r', '\n').Trim();

      Assert.IsTrue(!renderedContent.Contains("<atlantis:javascriptwebstash>") &&
                    !renderedContent.Contains("</atlantis:javascriptwebstash>") &&
                    scriptContent.Contains("abc") && scriptContent.Contains("def") && scriptContent.Contains("</body>")
                   );
    }

    [TestMethod]
    public void JavaScriptStashTestInValidCasing()
    {
      const string content = @"<body>
    <div>
      <aTlantis:javascriptwebstash>
abc
      </atlantis:javascriptweBStash>
    </div>
    <div>
      <atlantis:javascRIptwebstash>
def
      </atlanTis:javascriptwebstash>
    </div>
    <hr/>
  </body>";

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new JavaScriptWebStashRenderHandler() });

      Assert.IsTrue(renderedContent.Equals(content));
    }

    [TestMethod]
    public void JavaScriptStashTestInValidNoClosingTag()
    {
      const string content = @"<body>
    <div>
      <atlantis:javascriptwebstash>
        <script type=""text/javascript"">
          var k = 3;
        </script>
    </div>
    <hr/>
  </body>";

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new JavaScriptWebStashRenderHandler() });

      Assert.IsTrue(renderedContent.Equals(content));
    }

    [TestMethod]
    public void JavaScriptStashTestInValidNull()
    {
      const string content = null;

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new JavaScriptWebStashRenderHandler() });

      Assert.IsTrue(renderedContent == null);
    }

    [TestMethod]
    public void JavaScriptStashTestInValidEmpty()
    {
      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(string.Empty, new List<IRenderHandler> { new JavaScriptWebStashRenderHandler() });

      Assert.IsTrue(renderedContent.Equals(string.Empty));
    }
  }
}
