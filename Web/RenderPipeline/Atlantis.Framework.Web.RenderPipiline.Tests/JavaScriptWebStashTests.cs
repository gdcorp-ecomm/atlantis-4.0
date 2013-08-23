using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Web.RenderPipeline;
using Atlantis.Framework.Web.RenderPipiline.Tests.RenderContent;
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

      var handler = new JavaScriptWebStashRenderHandler();
      IProcessedRenderContent processedRenderContent = new SimpleProcessedRenderContent(content);
      handler.ProcessContent(processedRenderContent, ProviderContainer);

      var scriptContent = processedRenderContent.Content.Substring(processedRenderContent.Content.IndexOf("<hr/>", StringComparison.OrdinalIgnoreCase) + 5).TrimStart('\r', '\n').Trim();
      var scriptArray = scriptContent.Split(new string[] { "<script" }, StringSplitOptions.None);

      Assert.IsTrue(!processedRenderContent.Content.Contains("<atlantis:javascriptwebstash>") &&
                    !processedRenderContent.Content.Contains("</atlantis:javascriptwebstash>") &&
                    scriptArray.Length == 3
                   );
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

      var handler = new JavaScriptWebStashRenderHandler();
      IProcessedRenderContent processedRenderContent = new SimpleProcessedRenderContent(content);
      handler.ProcessContent(processedRenderContent, ProviderContainer);

      var scriptContent = processedRenderContent.Content.Substring(processedRenderContent.Content.IndexOf("<hr/>", StringComparison.OrdinalIgnoreCase) + 5).TrimStart('\r', '\n').Trim();

      Assert.IsTrue(!processedRenderContent.Content.Contains("<atlantis:javascriptwebstash>") &&
                    !processedRenderContent.Content.Contains("</atlantis:javascriptwebstash>") &&
                    scriptContent == "</body>"
                   );
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

      var handler = new JavaScriptWebStashRenderHandler();
      IProcessedRenderContent processedRenderContent = new SimpleProcessedRenderContent(content);
      handler.ProcessContent(processedRenderContent, ProviderContainer);

      var scriptContent = processedRenderContent.Content.Substring(processedRenderContent.Content.IndexOf("<hr/>", StringComparison.OrdinalIgnoreCase) + 5).TrimStart('\r', '\n').Trim();

      Assert.IsTrue(!processedRenderContent.Content.Contains("<atlantis:javascriptwebstash>") &&
                    !processedRenderContent.Content.Contains("</atlantis:javascriptwebstash>") &&
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

      var handler = new JavaScriptWebStashRenderHandler();
      IProcessedRenderContent processedRenderContent = new SimpleProcessedRenderContent(content);
      handler.ProcessContent(processedRenderContent, ProviderContainer);

      Assert.IsTrue(processedRenderContent.Content.Equals(content));
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

      var handler = new JavaScriptWebStashRenderHandler();
      IProcessedRenderContent processedRenderContent = new SimpleProcessedRenderContent(content);
      handler.ProcessContent(processedRenderContent, ProviderContainer);

      Assert.IsTrue(processedRenderContent.Content.Equals(content));
    }

    [TestMethod]
    public void JavaScriptStashTestInValidNull()
    {
      const string content = null;

      var handler = new JavaScriptWebStashRenderHandler();
      IProcessedRenderContent processedRenderContent = new SimpleProcessedRenderContent(content);
      handler.ProcessContent(processedRenderContent, ProviderContainer);

      Assert.IsTrue(processedRenderContent.Content.Equals(string.Empty));
    }

    [TestMethod]
    public void JavaScriptStashTestInValidEmpty()
    {
      var handler = new JavaScriptWebStashRenderHandler();
      IProcessedRenderContent processedRenderContent = new SimpleProcessedRenderContent(string.Empty);
      handler.ProcessContent(processedRenderContent, ProviderContainer);

      Assert.IsTrue(processedRenderContent.Content.Equals(string.Empty));
    }
  }
}
