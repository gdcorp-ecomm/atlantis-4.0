namespace Atlantis.Framework.Warmup.Fixtures
{
  using System;

  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
  public class WarmupAttribute : Attribute
  {
    private string _name;
    public string Name
    {
      get { return this._name ?? this.GetType().DeclaringMethod.ToString(); }
      set { this._name = value; }
    }

    public bool Ignore { get; set; }
    
  }
}