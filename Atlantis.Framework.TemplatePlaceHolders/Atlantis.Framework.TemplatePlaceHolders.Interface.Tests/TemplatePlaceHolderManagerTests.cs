using System;
using System.Diagnostics;
using Atlantis.Framework.Interface;
using Atlantis.Framework.TemplatePlaceHolders.Interface.Tests.Templates.en.sales._1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface.Tests
{
  [TestClass]
  public class TemplatePlaceHolderManagerTests
  {
    private IProviderContainer ProviderContainer 
    { 
      get
      {
        IProviderContainer providerContainer = new TestProviderContainer();

        providerContainer.RegisterProvider<ISiteContext, TestSiteContext>();
        providerContainer.RegisterProvider<IShopperContext, TestShopperContext>();

        
        providerContainer.RegisterProvider<ITemplateRequestKeyHandlerProvider, TestTemplateRequestKeyHandlerProvider>();
        providerContainer.RegisterProvider<TestTemplateContentProvider, TestTemplateContentProvider>();
        providerContainer.RegisterProvider<TestTemplateDataSoureProvider, TestTemplateDataSoureProvider>();
        
        return providerContainer;
      }
    }

    [TestMethod]
    public void SimpleStaticTemplateSourceTest()
    {
      string rawText = @"<!DOCTYPE html><html><head></head><title>Browser Title</title><body><div>[@TemplatePlaceHolder[{ ""templateSource"": { ""format"":""razor"", ""source"":""codeclass"", ""sourceAssembly"":""Atlantis.Framework.TemplatePlaceHolders.Interface.Tests"", ""requestKey"":""Atlantis.Framework.TemplatePlaceHolders.Interface.Tests.Templates.{0}.{1}._{2}.TestTemplateContentProvider"" }, ""dataSource"":{ ""providerAssembly"":""Atlantis.Framework.TemplatePlaceHolders.Interface.Tests"",""providerType"":""Atlantis.Framework.TemplatePlaceHolders.Interface.Tests.TestTemplateDataSoureProvider"", ""defaultDataSourceId"":""136"",""dataSourceOptions"":[ { ""key"":""us"",""value"":""136"" },{ ""key"":""eu"",""value"":""137"" },{ ""key"":""ap"",""value"":""138"" }]}}]@TemplatePlaceHolder]</div></body></html>";
      string output = TemplatePlaceHolderManager.ReplacePlaceHolders(rawText, ProviderContainer);

      Console.WriteLine("Original: {0}", rawText);
      Debug.WriteLine("Original: {0}", rawText);

      Console.WriteLine(string.Empty);
      Debug.WriteLine(string.Empty);
      Console.WriteLine(string.Empty);
      Debug.WriteLine(string.Empty);

      Console.WriteLine("Output: {0}", output);
      Debug.WriteLine("Output: {0}", output);

      Assert.IsTrue(!string.IsNullOrEmpty(output));
    }
  }
}
