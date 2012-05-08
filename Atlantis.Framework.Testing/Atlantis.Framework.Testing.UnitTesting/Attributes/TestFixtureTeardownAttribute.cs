using System;


namespace Atlantis.Framework.Testing.UnitTesting
{
  /// <summary>
  /// Summary description for TestFixtureTeardownAttribute
  /// </summary>
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
  public class TestFixtureTeardownAttribute : Attribute
  {
    public TestFixtureTeardownAttribute()
    {
      //
      // TODO: Add constructor logic here
      //
    }
  }
}