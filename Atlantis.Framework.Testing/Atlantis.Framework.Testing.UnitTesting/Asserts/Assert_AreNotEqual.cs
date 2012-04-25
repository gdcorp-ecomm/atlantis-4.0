using System;

namespace Atlantis.Framework.Testing.UnitTesting
{
  /// <summary>
  /// Summary description for Assert
  /// </summary>
  public static partial class Assert
  {
    #region AreNotEqual
    public static void AreNotEqual(int expected, int actual)
    {
      if (expected.Equals(actual))
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. Expected ({0})  matches Actual ({1}).", expected, actual));
      }
    }
    public static void AreNotEqual(int expected, int actual, string message)
    {
      if (expected.Equals(actual))
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. Expected ({0})  matches Actual ({1}). {2}", expected, actual, message));
      }
    }
    public static void AreNotEqual(uint expected, uint actual)
    {
      if (expected.Equals(actual))
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. Expected ({0})  matches Actual ({1}).", expected, actual));
      }
    }
    public static void AreNotEqual(uint expected, uint actual, string message)
    {
      if (expected.Equals(actual))
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. Expected ({0})  matches Actual ({1}). {2}", expected, actual, message));
      }
    }
    public static void AreNotEqual(decimal expected, decimal actual)
    {
      if (expected.Equals(actual))
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. Expected ({0})  matches Actual ({1}). {2}", expected, actual));
      }
    }
    public static void AreNotEqual(decimal expected, decimal actual, string message)
    {
      if (expected.Equals(actual))
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. Expected ({0})  matches Actual ({1}). {2}", expected, actual, message));
      }
    }
    public static void AreNotEqual(float expected, float actual)
    {
      if (expected.Equals(actual))
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. Expected ({0})  matches Actual ({1}).", expected, actual));
      }
    }
    public static void AreNotEqual(float expected, float actual, string message)
    {
      if (expected.Equals(actual))
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. Expected ({0})  matches Actual ({1}). {2}", expected, actual, message));
      }
    }
    public static void AreNotEqual(double expected, double actual)
    {
      if (expected.Equals(actual))
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. Expected ({0})  matches Actual ({1}).", expected, actual));
      }
    }
    public static void AreNotEqual(double expected, double actual, string message)
    {
      if (expected.Equals(actual))
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. Expected ({0})  matches Actual ({1}). {2}", expected, actual, message));
      }
    }
    public static void AreNotEqual(object expected, object actual)
    {
      if (expected.Equals(actual))
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. Expected ({0})  matches Actual ({1}).", expected, actual));
      }
    }
    public static void AreNotEqual(object expected, object actual, string message)
    {
      if (expected.Equals(actual))
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. Expected ({0})  matches Actual ({1}). {2}", expected, actual, message));
      }
    }
    #endregion

  }
}
