namespace Atlantis.Framework.Testing.UnitTesting
{
  /// <summary>
  /// Summary description for Assert
  /// </summary>
  public static partial class Assert
  {
    #region Fail
    public static void Fail()
    {
      throw new AssertionFailedException("Assertion Failed.");
    }
    public static void Fail(string message)
    {
      throw new AssertionFailedException(string.Format("Assertion Failed. {0}", message));
    }
    #endregion
  }
}