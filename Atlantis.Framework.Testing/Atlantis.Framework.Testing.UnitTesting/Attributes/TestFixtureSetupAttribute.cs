using System;


namespace Atlantis.Framework.Testing.UnitTesting
{
  /// <summary>
  /// Summary description for TestFixtureSetup
  /// </summary>
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
  public class TestFixtureSetupAttribute : Attribute
  {
    public TestFixtureSetupAttribute() { }

  }
}