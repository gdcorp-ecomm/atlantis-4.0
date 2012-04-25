using System;
namespace Atlantis.Framework.Testing.UnitTesting
{
  /// <summary>
  /// Summary description for Assert
  /// </summary>
  public static partial class Assert
  {
    #region IsFalse
    public static void IsFalse(bool condition)
    {
      if (condition)
        throw new AssertionFailedException(String.Format("Assertion Failed. Condition evaluates to True."));
    }
    public static void IsFalse(bool condition, string message)
    {
      if (condition)
        throw new AssertionFailedException(String.Format("Assertion Failed. Condition evaluates to True. {0}", message));

    }
    #endregion

  }
}
