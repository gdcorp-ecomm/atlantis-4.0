
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Atlantis.Framework.TH.Domains.Tests
{
  [TestClass]
  [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
  public class GrammaticalDelimiterTokenTests
  {

    /// <summary>
    /// Gets or sets the test context which provides
    /// information about and functionality for the current test run.
    /// </summary>
    public TestContext TestContext
    {
      get;
      set;
    }

    [TestMethod]
    public void GrammaticalDelimiterToken_ConstructorTest()
    {
      const string key = "domains";
      const string data = "<icanntlds />";
      string fullToken = String.Format("[@T[{0}:{1}]@T]", key, data);
      var target = new GrammaticalDelimiterToken(key, data, fullToken);
      Assert.IsNotNull(target);
    }
  }
}
