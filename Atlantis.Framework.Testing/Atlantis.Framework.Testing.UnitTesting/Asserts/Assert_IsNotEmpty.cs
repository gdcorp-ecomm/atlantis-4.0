using System;
using System.Collections;

namespace Atlantis.Framework.Testing.UnitTesting
{
  /// <summary>
  /// Summary description for Assert
  /// </summary>
  public static partial class Assert
  {
    #region IsNotEmpty
    public static void IsNotEmpty(string aString)
    {
      if (String.IsNullOrEmpty(aString))
        throw new AssertionFailedException(String.Format("Assertion Failed. String is null or empty."));
    }
    public static void IsNotEmpty(string aString, string message)
    {
      if (String.IsNullOrEmpty(aString))
        throw new AssertionFailedException(String.Format("Assertion Failed. String is null or empty. {0}", message));
    }
    public static void IsNotEmpty(ICollection collection)
    {
      try
      {
        if (collection.Count == 0)
          throw new AssertionFailedException(String.Format("Assertion Failed. Collection is empty."));
      }
      catch (AssertionFailedException)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. {0}", ex.Message), ex);
      }
    }
    public static void IsNotEmpty(ICollection collection, string message)
    {
      try
      {
        if (collection.Count == 0)
          throw new AssertionFailedException(String.Format("Assertion Failed. Collection is empty. {0}", message));
      }
      catch (AssertionFailedException)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new AssertionFailedException(String.Format("Assertion Failed. {0}", ex.Message), ex);
      }
    }
    #endregion

  }
}
