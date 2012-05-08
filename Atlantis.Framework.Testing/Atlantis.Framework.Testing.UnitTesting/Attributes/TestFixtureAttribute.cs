using System;

namespace Atlantis.Framework.Testing.UnitTesting
{
  /// <summary>
  /// Summary description for TestFixture
  /// </summary>
 
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public class TestFixtureAttribute : Attribute
  {
    public TestFixtureAttribute() { }
    public bool AllowDestructiveTests { get; set; }

  }
}
