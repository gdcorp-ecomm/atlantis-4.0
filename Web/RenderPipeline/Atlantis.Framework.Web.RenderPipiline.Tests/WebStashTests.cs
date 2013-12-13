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
  public class WebStashTests
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
      <atlantis:webstash type=""js"">
        <script type=""text/javascript"">
          var i = 3;
        </script>
      </atlantis:webstash>
    </div>
    <div>
      <atlantis:webstash type=""js"">
        <script type=""text/javascript"">
          var k = 3;
        </script>
      </atlantis:webstash>
    </div>
    <hr/>
  </body>";

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> {new WebStashRenderHandler()});

      var scriptContent = renderedContent.Substring(renderedContent.IndexOf("<hr/>", StringComparison.OrdinalIgnoreCase) + 5).TrimStart('\r', '\n').Trim();
      var scriptArray = scriptContent.Split(new[] { "<script" }, StringSplitOptions.None);

      Assert.IsTrue(!renderedContent.Contains("<atlantis:webstash type=\"js\">") &&
                    !renderedContent.Contains("</atlantis:webstash>") &&
                    scriptArray.Length == 3);
    }

    [TestMethod]
    public void JavaScriptStashTestValidEmpty()
    {
      const string content = @"<body>
    <div>
      <atlantis:webstash type=""js"">
      </atlantis:webstash>
    </div>
    <div>
      <atlantis:webstash type=""js"">
      </atlantis:webstash>
    </div>
    <hr/>
  </body>";

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new WebStashRenderHandler() });

      var scriptContent = renderedContent.Substring(renderedContent.IndexOf("<hr/>", StringComparison.OrdinalIgnoreCase) + 5).TrimStart('\r', '\n').Trim();

      Assert.IsTrue(!renderedContent.Contains("<atlantis:webstash type=\"js\">") &&
                    !renderedContent.Contains("</atlantis:webstash>") &&
                    scriptContent == "</body>");
    }

    [TestMethod]
    public void JavaScriptStashTestValidText()
    {
      const string content = @"<body>
    <div>
      <atlantis:webstash type=""js"">
abc
      </atlantis:webstash>
    </div>
    <div>
      <atlantis:webstash type=""js"">
def
      </atlantis:webstash>
    </div>
    <hr/>
  </body>";

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new WebStashRenderHandler() });

      var scriptContent = renderedContent.Substring(renderedContent.IndexOf("<hr/>", StringComparison.OrdinalIgnoreCase) + 5).TrimStart('\r', '\n').Trim();

      Assert.IsTrue(!renderedContent.Contains("<atlantis:webstash type=\"js\">") &&
                    !renderedContent.Contains("</atlantis:webstash>") &&
                    scriptContent.Contains("abc") && scriptContent.Contains("def") && scriptContent.Contains("</body>")
                   );
    }

    [TestMethod]
    public void JavaScriptStashTestInValidCasing()
    {
      const string content = @"<body>
    <div>
      <aTlantis:webstash type=""js"">
abc
      </atlantis:webstash>
    </div>
    <div>
      <atlantis:weBstash type=""js"">
def
      </atlanTis:webstash>
    </div>
    <hr/>
  </body>";

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new WebStashRenderHandler() });

      Assert.IsTrue(renderedContent.Equals(content));
    }

    [TestMethod]
    public void JavaScriptStashTestInValidNoClosingTag()
    {
      const string content = @"<body>
    <div>
      <atlantis:webstash type=""js"">
        <script type=""text/javascript"">
          var k = 3;
        </script>
    </div>
    <hr/>
  </body>";

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new WebStashRenderHandler() });

      Assert.IsTrue(renderedContent.Equals(content));
    }

    [TestMethod]
    public void WebStashTestInValidNull()
    {
      const string content = null;

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new WebStashRenderHandler() });

      Assert.IsTrue(renderedContent == null);
    }

    [TestMethod]
    public void WebStashTestInValidEmpty()
    {
      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(string.Empty, new List<IRenderHandler> { new WebStashRenderHandler() });

      Assert.IsTrue(renderedContent.Equals(string.Empty));
    }

    [TestMethod]
    public void CssStashTestValidWithScript()
    {
      const string content = @"<head>
    <div>
      <atlantis:webstash type=""css"">
        <style>
          .color-red
          {
            color: red;
          }
        </style>
      </atlantis:webstash>
    </div>
    <div>
      <atlantis:webstash type=""css"">
        <style>
          .color-red
          {
            color: red;
          }
        </style>
      </atlantis:webstash>
    </div>
    <hr/>
  </head>";

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new WebStashRenderHandler() });

      var scriptContent = renderedContent.Substring(renderedContent.IndexOf("<hr/>", StringComparison.OrdinalIgnoreCase) + 5).TrimStart('\r', '\n').Trim();
      var scriptArray = scriptContent.Split(new[] { "<style" }, StringSplitOptions.None);

      Assert.IsTrue(!renderedContent.Contains("<atlantis:webstash type=\"css\">") &&
                    !renderedContent.Contains("</atlantis:webstash>") &&
                    scriptArray.Length == 3);
    }

    [TestMethod]
    public void CssStashTestValidEmpty()
    {
      const string content = @"<head>
    <div>
      <atlantis:webstash type=""css"">
      </atlantis:webstash>
    </div>
    <div>
      <atlantis:webstash type=""css"">
      </atlantis:webstash>
    </div>
    <hr/>
  </head>";

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new WebStashRenderHandler() });

      var scriptContent = renderedContent.Substring(renderedContent.IndexOf("<hr/>", StringComparison.OrdinalIgnoreCase) + 5).TrimStart('\r', '\n').Trim();

      Assert.IsTrue(!renderedContent.Contains("<atlantis:webstash type=\"css\">") &&
                    !renderedContent.Contains("</atlantis:webstash>") &&
                    scriptContent == "</head>");
    }

    [TestMethod]
    public void CssStashTestValidText()
    {
      const string content = @"<head>
    <div>
      <atlantis:webstash type=""css"">
abc
      </atlantis:webstash>
    </div>
    <div>
      <atlantis:webstash type=""css"">
def
      </atlantis:webstash>
    </div>
    <hr/>
  </head>";

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new WebStashRenderHandler() });

      var scriptContent = renderedContent.Substring(renderedContent.IndexOf("<hr/>", StringComparison.OrdinalIgnoreCase) + 5).TrimStart('\r', '\n').Trim();

      Assert.IsTrue(!renderedContent.Contains("<atlantis:webstash type=\"css\">") &&
                    !renderedContent.Contains("</atlantis:webstash>") &&
                    scriptContent.Contains("abc") && scriptContent.Contains("def") && scriptContent.Contains("</head>")
                   );
    }

    [TestMethod]
    public void CssStashTestInValidCasing()
    {
      const string content = @"<head>
    <div>
      <aTlantis:webstash type=""css"">
abc
      </atlantis:webstash>
    </div>
    <div>
      <atlantis:weBstash type=""css"">
def
      </atlanTis:webstash>
    </div>
    <hr/>
  </head>";

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new WebStashRenderHandler() });

      Assert.IsTrue(renderedContent.Equals(content));
    }

    [TestMethod]
    public void CssStashTestInValidNoClosingTag()
    {
      const string content = @"<head>
    <div>
      <atlantis:webstash type=""css"">
        <style>
          .color-red
          {
            color: red;
          }
        </style>
    </div>
    <hr/>
  </head>";

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new WebStashRenderHandler() });

      Assert.IsTrue(renderedContent.Equals(content));
    }
  }
}
