using System;
using System.Collections;

namespace Atlantis.Framework.Testing.UnitTesting
{
  /// <summary>
  /// Summary description for Assert
  /// </summary>
  public static partial class Assert
  {
    #region Contains
    public static void Contains(object anObject, IList collection)
    {
      try
      {
        if (!collection.Contains(anObject))
          throw new AssertionFailedException(String.Format("Assertion Failed. Item not found in collection."));
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
    public static void Contains(object anObject, IList collection, string message)
    {
      try
      {
        if (!collection.Contains(anObject))
          throw new AssertionFailedException(String.Format("Assertion Failed. Item not found in collection. {0}", message));
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
