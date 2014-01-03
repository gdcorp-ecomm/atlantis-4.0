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
  public class CssWebStashTests
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
    public void CssStashTestValidWithScript()
    {
      const string content = @"<head>
    <div>
      <atlantis:csswebstash>
        <style type=""text/css"">
          var i = 3;
        </style>
      </atlantis:csswebstash>
    </div>
    <div>
      <atlantis:csswebstash>
        <style type=""text/css"">
          var k = 3;
        </style>
      </atlantis:csswebstash>
    </div>
    <hr/>
  </head>";

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> {new CssWebStashRenderHandler()});

      var cssContent = renderedContent.Substring(renderedContent.IndexOf("<hr/>", StringComparison.OrdinalIgnoreCase) + 5).TrimStart('\r', '\n').Trim();
      var cssArray = cssContent.Split(new[] { "<style" }, StringSplitOptions.None);

      Assert.IsTrue(!renderedContent.Contains("<atlantis:csswebstash>") &&
                    !renderedContent.Contains("</atlantis:csswebstash>") &&
                    cssArray.Length == 3);
    }

    [TestMethod]
    public void CssStashTestValidEmpty()
    {
      const string content = @"<head>
    <div>
      <atlantis:csswebstash>
      </atlantis:csswebstash>
    </div>
    <div>
      <atlantis:csswebstash>
      </atlantis:csswebstash>
    </div>
    <hr/>
  </head>";

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new CssWebStashRenderHandler() });

      var cssContent = renderedContent.Substring(renderedContent.IndexOf("<hr/>", StringComparison.OrdinalIgnoreCase) + 5).TrimStart('\r', '\n').Trim();

      Assert.IsTrue(!renderedContent.Contains("<atlantis:csswebstash>") &&
                    !renderedContent.Contains("</atlantis:csswebstash>") &&
                    cssContent == "</head>");
    }

    [TestMethod]
    public void CssStashTestValidText()
    {
      const string content = @"<head>
    <div>
      <atlantis:csswebstash>
abc
      </atlantis:csswebstash>
    </div>
    <div>
      <atlantis:csswebstash>
def
      </atlantis:csswebstash>
    </div>
    <hr/>
  </head>";

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new CssWebStashRenderHandler() });

      var cssContent = renderedContent.Substring(renderedContent.IndexOf("<hr/>", StringComparison.OrdinalIgnoreCase) + 5).TrimStart('\r', '\n').Trim();

      Assert.IsTrue(!renderedContent.Contains("<atlantis:csswebstash>") &&
                    !renderedContent.Contains("</atlantis:csswebstash>") &&
                    cssContent.Contains("abc") && cssContent.Contains("def") && cssContent.Contains("</head>")
                   );
    }

    [TestMethod]
    public void CssStashTestInValidCasing()
    {
      const string content = @"<head>
    <div>
      <aTlantis:csswebstash>
abc
      </atlantis:cssweBStash>
    </div>
    <div>
      <atlantis:cSSwebstash>
def
      </atlanTis:csswebstash>
    </div>
    <hr/>
  </head>";

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new CssWebStashRenderHandler() });

      Assert.IsTrue(renderedContent.Equals(content));
    }

    [TestMethod]
    public void CssStashTestInValidNoClosingTag()
    {
      const string content = @"<head>
    <div>
      <atlantis:csswebstash>
        <style type=""text/css"">
          var k = 3;
        </style>
    </div>
    <hr/>
  </head>";

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new CssWebStashRenderHandler() });

      Assert.IsTrue(renderedContent.Equals(content));
    }

    [TestMethod]
    public void CssStashTestInValidNull()
    {
      const string content = null;

      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(content, new List<IRenderHandler> { new CssWebStashRenderHandler() });

      Assert.IsTrue(renderedContent == null);
    }

    [TestMethod]
    public void CssStashTestInValidEmpty()
    {
      IRenderPipelineProvider renderPipelineProvider = ProviderContainer.Resolve<IRenderPipelineProvider>();
      string renderedContent = renderPipelineProvider.RenderContent(string.Empty, new List<IRenderHandler> { new CssWebStashRenderHandler() });

      Assert.IsTrue(renderedContent.Equals(string.Empty));
    }
  }
}
