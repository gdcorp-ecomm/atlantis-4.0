using System;
namespace Atlantis.Framework.Testing.UnitTesting
{
  /// <summary>
  /// Summary description for Assert
  /// </summary>
  public static partial class Assert
  {
    #region IsNotNull
    public static void IsNotNull(object anObject)
    {
      if (anObject == null)
        throw new AssertionFailedException(String.Format("Assertion Failed. Object is null."));

    }
    public static void IsNotNull(object anObject, string message)
    {
      if (anObject == null)
        throw new AssertionFailedException(String.Format("Assertion Failed. Object is null. {0}", message));

    }
    #endregion

  }
}
