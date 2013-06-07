namespace Atlantis.Framework.Warmup.Fixtures
{
  using System;

  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public class WarmupFixtureAttribute : Attribute
  {
    private string _name;
    public string Name
    {
      get { return this._name ?? this.GetType().DeclaringType.ToString(); }
      set { this._name = value; }
    }

    public bool Ignore { get; set; }
  }
}
