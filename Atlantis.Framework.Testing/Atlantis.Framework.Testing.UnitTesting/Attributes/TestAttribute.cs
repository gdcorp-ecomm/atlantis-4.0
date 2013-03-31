using System;

namespace Atlantis.Framework.Testing.UnitTesting
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
  public class TestAttribute : Attribute
  {
    private string _name;
    public string Name
    {
      get { return _name ?? this.GetType().DeclaringMethod.ToString(); }
      set { _name = value; }
    }

    public bool Ignore { get; set; }
    
    public TestOptions Options { get; set; }

    public TestAttribute() { }

  }
}