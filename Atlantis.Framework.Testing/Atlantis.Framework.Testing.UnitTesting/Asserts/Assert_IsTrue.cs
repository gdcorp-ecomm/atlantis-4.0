using System;
namespace Atlantis.Framework.Testing.UnitTesting
{
  /// <summary>
  /// Summary description for Assert
  /// </summary>
  public static partial class Assert
  {

    #region IsTrue
    public static void IsTrue(bool condition)
    {
      if (!condition)
        throw new AssertionFailedException(String.Format("Assertion Failed. Condition evaluates to False."));
    }
    public static void IsTrue(bool condition, string message)
    {
      if (!condition)
        throw new AssertionFailedException(String.Format("Assertion Failed. Condition evaluates to False. {0}", message));
    }
    #endregion

  }
}
