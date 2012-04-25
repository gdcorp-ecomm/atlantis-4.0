using System;
namespace Atlantis.Framework.Testing.UnitTesting
{
  /// <summary>
  /// Summary description for Assert
  /// </summary>
  public static partial class Assert
  {
    #region IsInstanceOfType
    public static void IsInstanceOfType(Type expected, object actual)
    {
      try
      {
        if (!expected.Equals(actual.GetType()))
        {
          throw new AssertionFailedException(String.Format("Assertion Failed. Types are not equal. "));
        }
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
    public static void IsInstanceOfType(Type expected, object actual, string message)
    {
      try
      {
        if (!expected.Equals(actual.GetType()))
        {
          throw new AssertionFailedException(String.Format("Assertion Failed. Types are not equal. {0}", message));
        }
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
