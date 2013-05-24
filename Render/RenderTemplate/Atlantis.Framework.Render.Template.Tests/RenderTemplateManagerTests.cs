using System;
using System.Diagnostics;
using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Render.Template.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Render.Template.Tests
{
  [TestClass]
  public class RenderTemplateManagerTests
  {
    private void WriteOutput(string output)
    {
      #if DEBUG
      Debug.WriteLine(output);
      #else
      Console.WriteLine(output);
      #endif
    }

    [TestMethod]
    public void ValidRenderContentMultipleLines()
    {
      string content = @"<template-data type=""masterpage"" location=""~/MasterPages/CDS/WideMaster.master"" />
                        <template-section name=""head"">
	                        <title>Test Page</title>
                          <meta name=""viewport"" content=""width=device-width,initial-scale=1.0,maximum-scale=1.0"" />
                          <link rel=""Stylesheet"" type=""text/css"" href=""http://img2.wsimg.com/mobile/sales/css/webkit/1/gd_webkit_20130422.min.css"" />
                        </template-section>
                        <template-section name=""content"">
	                        <div>Hello World!!!</div>
                          <div>Buy Domains</div>
                        </template-section>
                        <template-section name=""footer"">
	                        [@P[cdsSnippet:<Data location=""sales/_snippets/social/icons"" />]@P]
                        </template-section>";

      IRenderContent renderContent = new TestRenderContent(content);

      IRenderTemplateContent renderTemplateContent = RenderTemplateManager.ParseTemplateContent(renderContent);

      WriteOutput("---- Template Data ----");
      WriteOutput(string.Format("Type: {0}", renderTemplateContent.RenderTemplateData.Type));
      WriteOutput(string.Format("Location: {0}", renderTemplateContent.RenderTemplateData.Location));

      Assert.IsTrue(renderTemplateContent.RenderTemplateData.Type == "masterpage");
      Assert.IsTrue(renderTemplateContent.RenderTemplateData.Location == "~/MasterPages/CDS/WideMaster.master");

      IRenderTemplateSection headSection;
      bool headSectionExists = renderTemplateContent.TryGetSection("head", out headSection);

      IRenderTemplateSection contentSection;
      bool contentSectionExists = renderTemplateContent.TryGetSection("content", out contentSection);

      IRenderTemplateSection footerSection;
      bool footerSectionExists = renderTemplateContent.TryGetSection("footer", out footerSection);
      
      WriteOutput("---- Template Sections ----");
      WriteOutput(string.Format("{0}:", headSection.Name));
      WriteOutput(headSection.Content);

      WriteOutput(string.Format("{0}:", contentSection.Name));
      WriteOutput(contentSection.Content);

      WriteOutput(string.Format("{0}:", footerSection.Name));
      WriteOutput(footerSection.Content);

      Assert.IsTrue(headSectionExists &&
                    headSection.Content.Contains(@"<title>Test Page</title>") &&
                    headSection.Content.Contains(@"<meta name=""viewport"" content=""width=device-width,initial-scale=1.0,maximum-scale=1.0"" />") &&
                    headSection.Content.Contains(@"<link rel=""Stylesheet"" type=""text/css"" href=""http://img2.wsimg.com/mobile/sales/css/webkit/1/gd_webkit_20130422.min.css"" />"));

      Assert.IsTrue(contentSectionExists && 
                    contentSection.Content.Contains(@"<div>Hello World!!!</div>") &&
                    contentSection.Content.Contains(@"<div>Buy Domains</div>"));

      Assert.IsTrue(footerSectionExists && footerSection.Content.Contains(@"[@P[cdsSnippet:<Data location=""sales/_snippets/social/icons"" />]@P]"));
    }

    [TestMethod]
    public void MissingSectionEnd()
    {
      string content = @"<template-data type=""masterpage"" location=""~/MasterPages/CDS/WideMaster.master"" />
                        <template-section name=""head"">
	                        <title>Test Page</title>
                          <meta name=""viewport"" content=""width=device-width,initial-scale=1.0,maximum-scale=1.0"" />
                          <link rel=""Stylesheet"" type=""text/css"" href=""http://img2.wsimg.com/mobile/sales/css/webkit/1/gd_webkit_20130422.min.css"" />
                        <template-section name=""content"">
	                        <div>Hello World!!!</div>
                          <div>Buy Domains</div>
                        </template-section>
                        <template-section name=""footer"">
	                        [@P[cdsSnippet:<Data location=""sales/_snippets/social/icons"" />]@P]";

      IRenderContent renderContent = new TestRenderContent(content);

      IRenderTemplateContent renderTemplateContent = RenderTemplateManager.ParseTemplateContent(renderContent);

      WriteOutput("---- Template Data ----");
      WriteOutput(string.Format("Type: {0}", renderTemplateContent.RenderTemplateData.Type));
      WriteOutput(string.Format("Location: {0}", renderTemplateContent.RenderTemplateData.Location));

      Assert.IsTrue(renderTemplateContent.RenderTemplateData.Type == "masterpage");
      Assert.IsTrue(renderTemplateContent.RenderTemplateData.Location == "~/MasterPages/CDS/WideMaster.master");

      IRenderTemplateSection headSection;
      bool headSectionExists = renderTemplateContent.TryGetSection("head", out headSection);

      IRenderTemplateSection contentSection;
      bool contentSectionExists = renderTemplateContent.TryGetSection("content", out contentSection);

      IRenderTemplateSection footerSection;
      bool footerSectionExists = renderTemplateContent.TryGetSection("footer", out footerSection);

      WriteOutput("---- Template Sections ----");
      WriteOutput(string.Format("{0}:", headSection.Name));
      WriteOutput(headSection.Content);

      WriteOutput(string.Format("{0}:", contentSection.Name));
      WriteOutput(contentSection.Content);

      WriteOutput(string.Format("{0}:", footerSection.Name));
      WriteOutput(footerSection.Content);

      Assert.IsTrue(headSectionExists &&
                    headSection.Content.Contains(@"<title>Test Page</title>") &&
                    headSection.Content.Contains(@"<meta name=""viewport"" content=""width=device-width,initial-scale=1.0,maximum-scale=1.0"" />") &&
                    headSection.Content.Contains(@"<link rel=""Stylesheet"" type=""text/css"" href=""http://img2.wsimg.com/mobile/sales/css/webkit/1/gd_webkit_20130422.min.css"" />"));

      Assert.IsTrue(contentSectionExists &&
                    contentSection.Content.Contains(@"<div>Hello World!!!</div>") &&
                    contentSection.Content.Contains(@"<div>Buy Domains</div>"));

      Assert.IsTrue(footerSectionExists && footerSection.Content.Contains(@"[@P[cdsSnippet:<Data location=""sales/_snippets/social/icons"" />]@P]"));
    }

    [TestMethod]
    public void MissingSectionStart()
    {
      string content = @"<template-data type=""masterpage"" location=""~/MasterPages/CDS/WideMaster.master"" />
	                        <title>Test Page</title>
                          <meta name=""viewport"" content=""width=device-width,initial-scale=1.0,maximum-scale=1.0"" />
                          <link rel=""Stylesheet"" type=""text/css"" href=""http://img2.wsimg.com/mobile/sales/css/webkit/1/gd_webkit_20130422.min.css"" />
                        </template-section>
                        <template-section name=""content"">
	                        <div>Hello World!!!</div>
                          <div>Buy Domains</div>
                        </template-section>
	                        [@P[cdsSnippet:<Data location=""sales/_snippets/social/icons"" />]@P]
                        </template-section>";

      IRenderContent renderContent = new TestRenderContent(content);

      IRenderTemplateContent renderTemplateContent = RenderTemplateManager.ParseTemplateContent(renderContent);

      WriteOutput("---- Template Data ----");
      WriteOutput(string.Format("Type: {0}", renderTemplateContent.RenderTemplateData.Type));
      WriteOutput(string.Format("Location: {0}", renderTemplateContent.RenderTemplateData.Location));

      Assert.IsTrue(renderTemplateContent.RenderTemplateData.Type == "masterpage");
      Assert.IsTrue(renderTemplateContent.RenderTemplateData.Location == "~/MasterPages/CDS/WideMaster.master");

      IRenderTemplateSection contentSection;
      bool contentSectionExists = renderTemplateContent.TryGetSection("content", out contentSection);


      WriteOutput("---- Template Sections ----");

      WriteOutput(string.Format("{0}:", contentSection.Name));
      WriteOutput(contentSection.Content);

      Assert.IsTrue(contentSectionExists &&
                    contentSection.Content.Contains(@"<div>Hello World!!!</div>") &&
                    contentSection.Content.Contains(@"<div>Buy Domains</div>"));
    }

    [TestMethod]
    public void MissingAndEmptySectionName()
    {
      string content = @"<template-data type=""masterpage"" location=""~/MasterPages/CDS/WideMaster.master"" />
                        <template-section>
	                        <title>Test Page</title>
                          <meta name=""viewport"" content=""width=device-width,initial-scale=1.0,maximum-scale=1.0"" />
                          <link rel=""Stylesheet"" type=""text/css"" href=""http://img2.wsimg.com/mobile/sales/css/webkit/1/gd_webkit_20130422.min.css"" />
                        </template-section>
                        <template-section name=""content"">
	                        <div>Hello World!!!</div>
                          <div>Buy Domains</div>
                        </template-section>
                        <template-section name="""">
	                        [@P[cdsSnippet:<Data location=""sales/_snippets/social/icons"" />]@P]
                        </template-section>";

      IRenderContent renderContent = new TestRenderContent(content);

      IRenderTemplateContent renderTemplateContent = RenderTemplateManager.ParseTemplateContent(renderContent);

      WriteOutput("---- Template Data ----");
      WriteOutput(string.Format("Type: {0}", renderTemplateContent.RenderTemplateData.Type));
      WriteOutput(string.Format("Location: {0}", renderTemplateContent.RenderTemplateData.Location));

      Assert.IsTrue(renderTemplateContent.RenderTemplateData.Type == "masterpage");
      Assert.IsTrue(renderTemplateContent.RenderTemplateData.Location == "~/MasterPages/CDS/WideMaster.master");

      IRenderTemplateSection contentSection;
      bool contentSectionExists = renderTemplateContent.TryGetSection("content", out contentSection);

      WriteOutput("---- Template Sections ----");

      WriteOutput(string.Format("{0}:", contentSection.Name));
      WriteOutput(contentSection.Content);

      Assert.IsTrue(contentSectionExists &&
                    contentSection.Content.Contains(@"<div>Hello World!!!</div>") &&
                    contentSection.Content.Contains(@"<div>Buy Domains</div>"));
    }

    [TestMethod]
    public void MissingTemplateDataTag()
    {
      string content = @"<template-section name=""head"">
	                        <title>Test Page</title>
                          <meta name=""viewport"" content=""width=device-width,initial-scale=1.0,maximum-scale=1.0"" />
                          <link rel=""Stylesheet"" type=""text/css"" href=""http://img2.wsimg.com/mobile/sales/css/webkit/1/gd_webkit_20130422.min.css"" />
                        </template-section>
                        <template-section name=""content"">
	                        <div>Hello World!!!</div>
                          <div>Buy Domains</div>
                        </template-section>
                        <template-section name=""footer"">
	                        [@P[cdsSnippet:<Data location=""sales/_snippets/social/icons"" />]@P]
                        </template-section>";

      IRenderContent renderContent = new TestRenderContent(content);

      IRenderTemplateContent renderTemplateContent = RenderTemplateManager.ParseTemplateContent(renderContent);

      WriteOutput("---- Template Data ----");
      WriteOutput(string.Format("Type: {0}", renderTemplateContent.RenderTemplateData.Type));
      WriteOutput(string.Format("Location: {0}", renderTemplateContent.RenderTemplateData.Location));

      Assert.IsTrue(renderTemplateContent.RenderTemplateData.Type == string.Empty);
      Assert.IsTrue(renderTemplateContent.RenderTemplateData.Location == string.Empty);

      IRenderTemplateSection contentSection;
      bool contentSectionExists = renderTemplateContent.TryGetSection("content", out contentSection);

      WriteOutput("---- Template Sections ----");

      WriteOutput(string.Format("{0}:", contentSection.Name));
      WriteOutput(contentSection.Content);

      Assert.IsTrue(contentSectionExists &&
                    contentSection.Content.Contains(@"<div>Hello World!!!</div>") &&
                    contentSection.Content.Contains(@"<div>Buy Domains</div>"));
    }

    [TestMethod]
    public void MissingTemplateAttributes()
    {
      string content = @"<template-data />
                        <template-section name=""head"">
	                        <title>Test Page</title>
                          <meta name=""viewport"" content=""width=device-width,initial-scale=1.0,maximum-scale=1.0"" />
                          <link rel=""Stylesheet"" type=""text/css"" href=""http://img2.wsimg.com/mobile/sales/css/webkit/1/gd_webkit_20130422.min.css"" />
                        </template-section>
                        <template-section name=""content"">
	                        <div>Hello World!!!</div>
                          <div>Buy Domains</div>
                        </template-section>
                        <template-section name=""footer"">
	                        [@P[cdsSnippet:<Data location=""sales/_snippets/social/icons"" />]@P]
                        </template-section>";

      IRenderContent renderContent = new TestRenderContent(content);

      IRenderTemplateContent renderTemplateContent = RenderTemplateManager.ParseTemplateContent(renderContent);

      WriteOutput("---- Template Data ----");
      WriteOutput(string.Format("Type: {0}", renderTemplateContent.RenderTemplateData.Type));
      WriteOutput(string.Format("Location: {0}", renderTemplateContent.RenderTemplateData.Location));

      Assert.IsTrue(renderTemplateContent.RenderTemplateData.Type == string.Empty);
      Assert.IsTrue(renderTemplateContent.RenderTemplateData.Location == string.Empty);

      IRenderTemplateSection contentSection;
      bool contentSectionExists = renderTemplateContent.TryGetSection("content", out contentSection);

      WriteOutput("---- Template Sections ----");

      WriteOutput(string.Format("{0}:", contentSection.Name));
      WriteOutput(contentSection.Content);

      Assert.IsTrue(contentSectionExists &&
                    contentSection.Content.Contains(@"<div>Hello World!!!</div>") &&
                    contentSection.Content.Contains(@"<div>Buy Domains</div>"));
    }

    [TestMethod]
    public void EmptyContent()
    {
      string content = string.Empty;

      IRenderContent renderContent = new TestRenderContent(content);

      IRenderTemplateContent renderTemplateContent = RenderTemplateManager.ParseTemplateContent(renderContent);

      WriteOutput("---- Template Data ----");
      WriteOutput(string.Format("Type: {0}", renderTemplateContent.RenderTemplateData.Type));
      WriteOutput(string.Format("Location: {0}", renderTemplateContent.RenderTemplateData.Location));

      Assert.IsTrue(renderTemplateContent.RenderTemplateData.Type == string.Empty);
      Assert.IsTrue(renderTemplateContent.RenderTemplateData.Location == string.Empty);

      IRenderTemplateSection contentSection;
      bool contentSectionExists = renderTemplateContent.TryGetSection("content", out contentSection);

      Assert.IsFalse(contentSectionExists);
    }

    [TestMethod]
    public void NullContent()
    {
      string content = null;

      IRenderContent renderContent = new TestRenderContent(content);

      IRenderTemplateContent renderTemplateContent = RenderTemplateManager.ParseTemplateContent(renderContent);

      WriteOutput("---- Template Data ----");
      WriteOutput(string.Format("Type: {0}", renderTemplateContent.RenderTemplateData.Type));
      WriteOutput(string.Format("Location: {0}", renderTemplateContent.RenderTemplateData.Location));

      Assert.IsTrue(renderTemplateContent.RenderTemplateData.Type == string.Empty);
      Assert.IsTrue(renderTemplateContent.RenderTemplateData.Location == string.Empty);

      IRenderTemplateSection contentSection;
      bool contentSectionExists = renderTemplateContent.TryGetSection("content", out contentSection);

      Assert.IsFalse(contentSectionExists);
    }
  }
}
