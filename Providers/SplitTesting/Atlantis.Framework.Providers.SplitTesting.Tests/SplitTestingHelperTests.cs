using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.SplitTesting.Tests
{
  [TestClass]
  public class SplitTestingHelperTests
  {
    [TestMethod]
    public void GetOverrideSide_OneOverride()
    {

      const string testString = "123.A";
      const string expected = "A";

      string actual;
      Assert.IsTrue(SplitTestingHelper.GetOverrideSide(123, testString, out actual));
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetOverrideSide_TwoOverride()
    {
      const string testString = "123.A|456.B";
      const string expectedA = "A";
      const string expectedB = "B";

      string actual;
      Assert.IsTrue(SplitTestingHelper.GetOverrideSide(123, testString, out actual));
      Assert.AreEqual(expectedA, actual);

      Assert.IsTrue(SplitTestingHelper.GetOverrideSide(456, testString, out actual));
      Assert.AreEqual(expectedB, actual);
    }

    [TestMethod]
    public void GetOverrideSide_EmptyInput()
    {

      string testString = string.Empty;
      string expected = string.Empty;

      string actual;
      Assert.IsFalse(SplitTestingHelper.GetOverrideSide(123, testString, out actual));
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetOverrideSide_MissingSide()
    {

      string testString = "123.";
      string expected = string.Empty;

      string actual;
      Assert.IsFalse(SplitTestingHelper.GetOverrideSide(123, testString, out actual));
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetOverrideSide_MissingTestId()
    {

      const string testString = ".A";
      string expected = string.Empty;

      string actual;
      Assert.IsFalse(SplitTestingHelper.GetOverrideSide(123, testString, out actual));
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetOverrideSide_MalformedInput()
    {

      const string testString = "malformed^input";
      string expected = string.Empty;

      string actual;
      Assert.IsFalse(SplitTestingHelper.GetOverrideSide(123, testString, out actual));
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetOverrideSide_MalformedInput2()
    {

      const string testString = "..123";
      string expected = string.Empty;

      string actual;
      Assert.IsFalse(SplitTestingHelper.GetOverrideSide(123, testString, out actual));
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetOverrideSide_MalformedInput3()
    {

      const string testString = ".123";
      string expected = string.Empty;

      string actual;
      Assert.IsFalse(SplitTestingHelper.GetOverrideSide(123, testString, out actual));
      Assert.AreEqual(expected, actual);
    }


    [TestMethod]
    public void GetOverrideSide_MalformedInput4()
    {

      string testString = "123.B||";
      string expected = string.Empty;

      string actual;
      Assert.IsFalse(SplitTestingHelper.GetOverrideSide(456, testString, out actual));
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetOverrideSide_ExtraDelimiter()
    {

      const string testString = "123.B|";
      string expected = "B";

      string actual;
      Assert.IsTrue(SplitTestingHelper.GetOverrideSide(123, testString, out actual));
      Assert.AreEqual(expected, actual);
    }


  }
}
