namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public interface IDotTypeFormLookup
  {
    string FormType { get; set; }
    string Domain { get; set; }
    string Tld { get; set; }
    string Placement { get; set; }
    string Phase { get; set; }
  }
}
