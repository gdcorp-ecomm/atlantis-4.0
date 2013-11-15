using Atlantis.Framework.Providers.DotTypeRegistration.Interface;

namespace Atlantis.Framework.Providers.DotTypeRegistration
{
  public class DotTypeFormLookup : IDotTypeFormLookup
  {
    public string FormType { get; set; }
    public string Domain { get; set; }
    public string Tld { get; set; }
    public string Placement { get; set; }
    public string Phase { get; set; }

    private DotTypeFormLookup(string formType, string tld, string placement, string phase, string domain)
    {
      FormType = formType;
      Domain = domain;
      Tld = tld;
      Placement = placement;
      Phase = phase;
    }

    public static IDotTypeFormLookup Create(string formType, string tld, string placement, string phase, string domain = "")
    {
      return new DotTypeFormLookup(formType, tld, placement, phase, domain);
    }
  }
}
