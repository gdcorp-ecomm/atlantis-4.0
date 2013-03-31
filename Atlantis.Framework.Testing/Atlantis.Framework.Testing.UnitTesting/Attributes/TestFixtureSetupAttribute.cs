using System;


namespace Atlantis.Framework.Testing.UnitTesting
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
  public class TestFixtureSetupAttribute : Attribute
  {
    public TestFixtureSetupAttribute() { }

  }
}