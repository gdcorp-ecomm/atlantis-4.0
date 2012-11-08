using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.RazorEngine.Tests
{
  [TestClass]
  public class RazorEngineTests
  {
    private const string RAZOR_TEMPLATE = "<!DOCTYPE html><html><head><title>@Model.Title</title></head><body><h1>@Model.HeadingOne</h1><div>@Model.Body</div></body></html>";

    [TestMethod]
    public void RenderRazorTemplate()
    {
      SimpleModel model = new SimpleModel { Title = "Some Page", HeadingOne = "Hellow Meow", Body = "Go Now, Now Go" };

      string renderedHtml = Razor.CompileAndRun(RAZOR_TEMPLATE, model, Path.GetTempPath());

      Debug.WriteLine("Template: " + RAZOR_TEMPLATE);
      Debug.WriteLine("Rendered Html: " + renderedHtml);

      Console.WriteLine("Template: " + RAZOR_TEMPLATE);
      Console.WriteLine("Rendered Html: " + renderedHtml);
    }
  }
}
