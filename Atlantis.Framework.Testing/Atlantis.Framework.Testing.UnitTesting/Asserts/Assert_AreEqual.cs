using System;

namespace Atlantis.Framework.Testing.UnitTesting
{
  /// <summary>
  /// Summary description for Assert
  /// </summary>
  public static partial class Assert
  {
    #region AreEqual
    public static void AreEqual(int expected, int actual)
    {
      if (!expected.Equals(actual))
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. Expected {0}, Actual was {1}.", expected, actual));
      }
    }
    public static void AreEqual(int expected, int actual, string message)
    {
      if (!expected.Equals(actual))
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. Expected {0}, Actual was {1}. {2}", expected, actual, message));
      }
    }
    public static void AreEqual(uint expected, uint actual)
    {
      if (!expected.Equals(actual))
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. Expected {0}, Actual was {1}.", expected, actual));
      }
    }
    public static void AreEqual(uint expected, uint actual, string message)
    {
      if (!expected.Equals(actual))
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. Expected {0}, Actual was {1}. {2}", expected, actual, message));
      }
    }
    public static void AreEqual(decimal expected, decimal actual)
    {
      if (!expected.Equals(actual))
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. Expected {0}, Actual was {1}.", expected, actual));
      }
    }
    public static void AreEqual(decimal expected, decimal actual, string message)
    {
      if (!expected.Equals(actual))
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. Expected {0}, Actual was {1}. {2}", expected, actual, message));
      }
    }
    public static void AreEqual(object expected, object actual)
    {
      if (!expected.Equals(actual))
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. Expected {0}, Actual was {1}.", expected, actual));
      }
    }
    public static void AreEqual(object expected, object actual, string message)
    {
      if (!expected.Equals(actual))
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. Expected {0}, Actual was {1}. {2}", expected, actual, message));
      }
    }
    #endregion

  }
}