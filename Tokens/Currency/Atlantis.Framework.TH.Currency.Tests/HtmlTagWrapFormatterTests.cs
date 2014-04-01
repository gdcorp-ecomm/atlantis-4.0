using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.TH.Currency.Tests
{
  [TestClass]
  [ExcludeFromCodeCoverage]
  public class HtmlTagWrapFormatterTests
  {
    private TestContext _testContextInstance;


    public TestContext TestContext
    {
      get
      {
        return _testContextInstance;
      }
      set
      {
        _testContextInstance = value;
      }
    }

    [TestMethod]
    public void ConstructorTest()
    {
      var actual = new HtmlTagWrapFormatter("span");
      Assert.IsNotNull(actual);

      actual = new HtmlTagWrapFormatter("span", "myClass");
      Assert.IsNotNull(actual);
    }

    [TestMethod]
    public void StringFormatUseTest()
    {
      var fmtPrvdr = new HtmlTagWrapFormatter("span", "myClass");
      var testValue = "$";
      var expected = string.Format("<span class=\"myClass\">{0}</span>", testValue);
      var actual = string.Format(fmtPrvdr, "{0}", testValue);
      Assert.IsNotNull(actual);
      TestContext.WriteLine(actual);
      Assert.AreEqual(expected, actual);

      fmtPrvdr = new HtmlTagWrapFormatter("span");
      expected = string.Format("<span>{0}</span>", testValue);
      actual = string.Format(fmtPrvdr, "{0}", testValue);
      Assert.IsNotNull(actual);
      TestContext.WriteLine(actual);
      Assert.AreEqual(expected, actual);
    }
  }
}
