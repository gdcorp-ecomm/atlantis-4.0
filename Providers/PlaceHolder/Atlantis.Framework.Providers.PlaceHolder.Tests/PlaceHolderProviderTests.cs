using System;
using System.Collections.Generic;
using System.Reflection;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.RenderPipeline;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Atlantis.Framework.Providers.PlaceHolder.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
  public class PlaceHolderProviderTests
  {
    public static IProviderContainer InitializeProviderContainer()
    {
      IProviderContainer providerContainer = new MockProviderContainer();
      providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      providerContainer.RegisterProvider<IManagerContext, MockManagerContext>();
      providerContainer.RegisterProvider<IPlaceHolderProvider, PlaceHolderProvider>();
      providerContainer.RegisterProvider<ICDSContentProvider, CDSContentProvider>();
      providerContainer.RegisterProvider<IRenderPipelineProvider, RenderPipelineProvider>();


      return providerContainer;
    }

    private void WriteOutput(string message)
    {
#if (DEBUG)
      Debug.WriteLine(message);
#else
      Console.WriteLine(message);
#endif
    }

    [TestInitialize]
    public void Initialize()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
    }

    [TestMethod]
    public void NullPlaceHolder()
    {
      string content = @"<div>
                          [@P[doesNotExist:<Data location=""sdfdfsafsf""><Parameters><Parameter key=""Hello"" value=""World"" /></Parameters></Data>]@P]
                         </div>";

      IProviderContainer providerContainer = InitializeProviderContainer();
      IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();

      string finalContent = placeHolderProvider.ReplacePlaceHolders(content, null);

      WriteOutput(finalContent);
      Assert.IsFalse(finalContent.Contains("[@P[") || finalContent.Contains("]@P]"));
    }

    [TestMethod]
    public void NoParametersPlaceHolder()
    {
      string content = @"<div>
                          [@P[userControl:<Data location=""sdfdfsafsf""></Data>]@P]
                         </div>";

      IProviderContainer providerContainer = InitializeProviderContainer();
      IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();
      
      placeHolderProvider.ReplacePlaceHolders(content, null);
    }

    [TestMethod]
    public void NoPlaceHolders()
    {
      string content = @"<div>
                          Hello World
                         </div>";

      IProviderContainer providerContainer = InitializeProviderContainer();
      IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();
      
      string finalContent = placeHolderProvider.ReplacePlaceHolders(content, null);

      WriteOutput(finalContent);
      Assert.IsTrue(finalContent.Contains("Hello World"));
    }

    [TestMethod]
    public void NullContent()
    {
      string content = null;

      IProviderContainer providerContainer = InitializeProviderContainer();
      IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();
      
      string finalContent = placeHolderProvider.ReplacePlaceHolders(content, null);

      WriteOutput(finalContent);
      Assert.IsTrue(finalContent.Equals(string.Empty));
    }

    [TestMethod]
    public void CreateUserControlPlaceHolderValid()
    {
      IPlaceHolder placeHolder = new UserControlPlaceHolder("~/somepath/control.ascx", new List<KeyValuePair<string, string>>(0));

      string markup = placeHolder.ToMarkup();

      WriteOutput(markup);
      Assert.IsTrue(markup.Equals("[@P[" + PlaceHolderTypes.UserControl + ":<Data location=\"~/somepath/control.ascx\" />]@P]"));
    }

    [TestMethod]
    public void CreateUserControlPlaceHolderWithParamsValid()
    {
      IList<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>(2);
      parameters.Add(new KeyValuePair<string, string>("title", "Hello World!"));
      parameters.Add(new KeyValuePair<string, string>("text", "My Name Is Timbo"));

      IPlaceHolder placeHolder = new UserControlPlaceHolder("~/somepath/control.ascx", parameters);

      string markup = placeHolder.ToMarkup();

      WriteOutput(markup);
      Assert.IsTrue(markup.Equals("[@P[" + PlaceHolderTypes.UserControl + ":<Data location=\"~/somepath/control.ascx\"><Parameters><Parameter key=\"title\" value=\"Hello World!\" /><Parameter key=\"text\" value=\"My Name Is Timbo\" /></Parameters></Data>]@P]"));
    }

    [TestMethod]
    public void CreateUserControlPlaceHolderWithNullParams()
    {
      IPlaceHolder placeHolder = new UserControlPlaceHolder("~/somepath/control.ascx", null);

      string markup = placeHolder.ToMarkup();

      WriteOutput(markup);
      Assert.IsTrue(markup.Equals("[@P[" + PlaceHolderTypes.UserControl + ":<Data location=\"~/somepath/control.ascx\" />]@P]"));
    }

    [TestMethod]
    public void CreateWebControlPlaceHolderValid()
    {
      IPlaceHolder placeHolder = new WebControlPlaceHolder(Assembly.GetExecutingAssembly().FullName,
                                                           "Atlantis.Framework.Providers.PlaceHolder.Tests.WebControls.WebControlOne",
                                                           new List<KeyValuePair<string, string>>(0));

      string markup = placeHolder.ToMarkup();

      WriteOutput(markup);
      Assert.IsTrue(markup.Equals("[@P[" + PlaceHolderTypes.WebControl + ":<Data assembly=\"" + Assembly.GetExecutingAssembly().FullName + "\" type=\"Atlantis.Framework.Providers.PlaceHolder.Tests.WebControls.WebControlOne\" />]@P]"));
    }

    [TestMethod]
    public void CreateWebControlPlaceHolderWithParamsValid()
    {
      IList<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>(2);
      parameters.Add(new KeyValuePair<string, string>("title", "Hello World!"));
      parameters.Add(new KeyValuePair<string, string>("text", "My Name Is Timbo"));

      IPlaceHolder placeHolder = new WebControlPlaceHolder(Assembly.GetExecutingAssembly().FullName,
                                                           "Atlantis.Framework.Providers.PlaceHolder.Tests.WebControls.WebControlOne",
                                                           parameters);

      string markup = placeHolder.ToMarkup();

      WriteOutput(markup);
      Assert.IsTrue(markup.Equals("[@P[" + PlaceHolderTypes.WebControl + ":<Data assembly=\"" + Assembly.GetExecutingAssembly().FullName + "\" type=\"Atlantis.Framework.Providers.PlaceHolder.Tests.WebControls.WebControlOne\"><Parameters><Parameter key=\"title\" value=\"Hello World!\" /><Parameter key=\"text\" value=\"My Name Is Timbo\" /></Parameters></Data>]@P]"));
    }

    [TestMethod]
    public void CreateWebControlPlaceHolderWithNullParams()
    {
      IPlaceHolder placeHolder = new WebControlPlaceHolder(Assembly.GetExecutingAssembly().FullName,
                                                           "Atlantis.Framework.Providers.PlaceHolder.Tests.WebControls.WebControlOne",
                                                           null);

      string markup = placeHolder.ToMarkup();

      WriteOutput(markup);
      Assert.IsTrue(markup.Equals("[@P[" + PlaceHolderTypes.WebControl + ":<Data assembly=\"" + Assembly.GetExecutingAssembly().FullName + "\" type=\"Atlantis.Framework.Providers.PlaceHolder.Tests.WebControls.WebControlOne\" />]@P]"));
    }

    [TestMethod]
    public void RenderWebControlPlaceHolderValid()
    {
      IPlaceHolder placeHolder = new WebControlPlaceHolder(Assembly.GetExecutingAssembly().FullName,
                                                           "Atlantis.Framework.Providers.PlaceHolder.Tests.WebControls.WebControlOne",
                                                           new List<KeyValuePair<string, string>>(0));

      IProviderContainer providerContainer = InitializeProviderContainer();
      IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();
      
      string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup(), null);
      
      WriteOutput(renderedContent);
      Assert.IsTrue(renderedContent.Equals("Web Control One!" + "Init event fired!!!" + "Load event fired!!!" + "PreRender event fired!!!"));
    }

    [TestMethod]
    public void RenderWebControlPlaceHolderWithChildValid()
    {
      IPlaceHolder placeHolder = new WebControlPlaceHolder(Assembly.GetExecutingAssembly().FullName,
                                                           "Atlantis.Framework.Providers.PlaceHolder.Tests.WebControls.Parent",
                                                           new List<KeyValuePair<string, string>>(0));

      IProviderContainer providerContainer = InitializeProviderContainer();
      IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();
      
      string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup(), null);

      WriteOutput(renderedContent);
      Assert.IsTrue(renderedContent.Equals("<div>Child Control</div>"));
    }

    [TestMethod]
    public void RenderWebControlWithParametersValid()
    {
      IList<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>(2);
      parameters.Add(new KeyValuePair<string, string>("Title", "Hello World!"));
      parameters.Add(new KeyValuePair<string, string>("Text", "My Name Is Timbo"));
      parameters.Add(new KeyValuePair<string, string>("boolVal", "true"));
      parameters.Add(new KeyValuePair<string, string>("intVal", "1"));
      parameters.Add(new KeyValuePair<string, string>("dateVal", "08/27/2013"));



      IPlaceHolder placeHolder = new WebControlPlaceHolder(Assembly.GetExecutingAssembly().FullName,
                                                           "Atlantis.Framework.Providers.PlaceHolder.Tests.WebControls.WebControlOne",
                                                           parameters);

      IProviderContainer providerContainer = InitializeProviderContainer();
      IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();
      
      string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup(), null);

      WriteOutput(renderedContent);
      Assert.IsTrue(renderedContent.Equals("Web Control One!" + "Hello World!" + "My Name Is Timbo" + "int value is 1!" + "bool value is True!" + "datetime value is 8/27/2013 12:00:00 AM!" + "Init event fired!!!" + "Load event fired!!!" + "PreRender event fired!!!"));
    }

    [TestMethod]
    public void RenderWebControlTwiceForTypeCache()
    {
      IPlaceHolder placeHolder = new WebControlPlaceHolder(Assembly.GetExecutingAssembly().FullName,
                                                           "Atlantis.Framework.Providers.PlaceHolder.Tests.WebControls.WebControlOne",
                                                           new List<KeyValuePair<string, string>>(0));

      IProviderContainer providerContainer = InitializeProviderContainer();
      IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();
      
      string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup() + Environment.NewLine + placeHolder.ToMarkup(), null);

      WriteOutput(renderedContent);
      Assert.IsTrue(renderedContent.Equals("Web Control One!" + "Init event fired!!!" + "Load event fired!!!" + "PreRender event fired!!!" + Environment.NewLine + "Web Control One!" + "Init event fired!!!" + "Load event fired!!!" + "PreRender event fired!!!"));
    }

    [TestMethod]
    public void CreateCDSDocumentPlaceHolderValid()
    {
      IPlaceHolder placeHolder = new CDSDocumentPlaceHolder("atlantis",
                                                            "home");

      string markup = placeHolder.ToMarkup();

      WriteOutput(markup);
      Assert.IsTrue(markup.Equals("[@P[" + PlaceHolderTypes.CDSDocument + ":<Data app=\"atlantis\" location=\"home\" />]@P]"));
    }

    [TestMethod]
    public void RenderCDSDocumentValid()
    {
      IPlaceHolder placeHolder = new CDSDocumentPlaceHolder("atlantis",
                                                            "home");

      IProviderContainer providerContainer = InitializeProviderContainer();
      IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();
      
      string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup(), null);

      WriteOutput(renderedContent);

      Assert.IsTrue(!string.IsNullOrEmpty(renderedContent), "Empty document returned!");
      Assert.IsTrue(renderedContent.StartsWith("<!DOCTYPE html>"), "Document did not start with doctype declaration");
    }

    [TestMethod]
    public void RenderCDSDocumentOneNestedPlaceHolder()
    {
      IPlaceHolder placeHolder = new CDSDocumentPlaceHolder("atlantis",
                                                            "_global/nesteddocument");

      IProviderContainer providerContainer = InitializeProviderContainer();
      IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();
      
      string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup(), null);

      WriteOutput(renderedContent);

      Assert.IsFalse(renderedContent.Contains("[@P["), "Nested CDS Document placeholder not rendered");
    }

    [TestMethod]
    public void RenderCDSDocumentCircularReference()
    {
      IPlaceHolder placeHolder = new CDSDocumentPlaceHolder("atlantis",
                                                            "_global/circularreference");

      IProviderContainer providerContainer = InitializeProviderContainer();
      IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();
      
      string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup(), null);

      WriteOutput(renderedContent);

      Assert.IsTrue(renderedContent == string.Empty);
    }

    [TestMethod]
    public void RenderCDSDocumentEmptyAppPath()
    {
      IPlaceHolder placeHolder = new CDSDocumentPlaceHolder(string.Empty,
                                                            string.Empty);

      IProviderContainer providerContainer = InitializeProviderContainer();
      IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();
      
      string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup(), null);

      WriteOutput(renderedContent);

      Assert.IsTrue(renderedContent == string.Empty, "Document should return as a string.Empty");
    }

    [TestMethod]
    public void RenderCDSDocumentNullAppPath()
    {
      IPlaceHolder placeHolder = new CDSDocumentPlaceHolder(null,
                                                            null);

      IProviderContainer providerContainer = InitializeProviderContainer();
      IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();
      
      string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup(), null);

      WriteOutput(renderedContent);

      Assert.IsTrue(renderedContent == string.Empty, "Document should return as a string.Empty");
    }

    [TestMethod]
    public void RenderWebControlPlaceHolderValidAppendRenderHandler()
    {
      IPlaceHolder placeHolder = new WebControlPlaceHolder(Assembly.GetExecutingAssembly().FullName,
                                                           "Atlantis.Framework.Providers.PlaceHolder.Tests.WebControls.WebControlOne",
                                                           new List<KeyValuePair<string, string>>(0));

      IProviderContainer providerContainer = InitializeProviderContainer();
      IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();
      
      string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup(), new List<IRenderHandler> { new AppendRenderHandler("APPENDED RENDER HANDLER CONTENT!!!") });

      WriteOutput(renderedContent);
      Assert.IsTrue(renderedContent.Equals("Web Control One!" + "Init event fired!!!" + "Load event fired!!!" + "PreRender event fired!!!" + "APPENDED RENDER HANDLER CONTENT!!!"));
    }

    [TestMethod]
    public void RenderCDSDocumentValidAppendRenderHandler()
    {
      IPlaceHolder placeHolder = new CDSDocumentPlaceHolder("atlantis",
                                                            "home");

      IProviderContainer providerContainer = InitializeProviderContainer();
      IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();
      
      string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup(), new List<IRenderHandler> { new AppendRenderHandler("APPENDED RENDER HANDLER CONTENT!!!") });

      WriteOutput(renderedContent);

      Assert.IsTrue(!string.IsNullOrEmpty(renderedContent), "Empty document returned!");
      Assert.IsTrue(renderedContent.EndsWith("APPENDED RENDER HANDLER CONTENT!!!"), "Document did not end with \"APPENDED RENDER HANDLER CONTENT!!!\"");
    }

    [TestMethod]
    public void RenderCDSDocumentOneNestedPlaceHolderAppendRenderHandler()
    {
      IPlaceHolder placeHolder = new CDSDocumentPlaceHolder("atlantis",
                                                            "_global/nesteddocument");

      IProviderContainer providerContainer = InitializeProviderContainer();
      IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();
      
      string renderedContent = placeHolderProvider.ReplacePlaceHolders(placeHolder.ToMarkup(), new List<IRenderHandler> { new AppendRenderHandler("APPENDED RENDER HANDLER CONTENT!!!") });

      WriteOutput(renderedContent);

      Assert.IsFalse(renderedContent.Contains("[@P["), "Nested CDS Document placeholder not rendered");
      Assert.IsTrue(renderedContent.Contains("/>APPENDED RENDER HANDLER CONTENT!!!APPENDED RENDER HANDLER CONTENT!!!"));
    }

  }
}
