using System;


namespace Atlantis.Framework.Testing.UnitTesting
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
  public class TestFixtureTeardownAttribute : Attribute
  {
    public TestFixtureTeardownAttribute()
    {
    }
  }
}