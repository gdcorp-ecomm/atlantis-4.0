using System;
using System.Collections;

namespace Atlantis.Framework.Testing.UnitTesting
{
  /// <summary>
  /// Summary description for Assert
  /// </summary>
  public static partial class Assert
  {
    #region IsEmpty
    public static void IsEmpty(string aString)
    {
      if (!String.IsNullOrEmpty(aString))
        throw new AssertionFailedException(String.Format("Assertion Failed. String is not empty."));
    }
    public static void IsEmpty(string aString, string message)
    {
      if (!String.IsNullOrEmpty(aString))
        throw new AssertionFailedException(String.Format("Assertion Failed. String is not empty. {0}", message));
    }
    public static void IsEmpty(ICollection collection)
    {
      try
      {
        if (collection.Count > 0)
          throw new AssertionFailedException(String.Format("Assertion Failed. Collection is not empty."));
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
    public static void IsEmpty(ICollection collection, string message)
    {
      try
      {
        if (collection.Count > 0)
          throw new AssertionFailedException(String.Format("Assertion Failed. Collection is not empty. {0}", message));
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
