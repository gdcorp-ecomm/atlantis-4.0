using System;


namespace Atlantis.Framework.Testing.UnitTesting
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
  public class TestSetupAttribute : Attribute
  {
    private string _name;
    public string Name
    {
      get { return _name ?? this.GetType().DeclaringMethod.ToString(); }
      set { _name = value; }
    }

    public TestSetupAttribute()
    {
    }
  }
}