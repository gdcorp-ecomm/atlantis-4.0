using Atlantis.Framework.Providers.DomainContactValidation.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;

namespace Atlantis.Framework.Providers.DotTypeRegistration
{
  public class DotTypeFormSchemaLookup : IDotTypeFormSchemaLookup
  {
    public string FormType { get; set; }
    public string Domain { get; set; }
    public string Tld { get; set; }
    public string Placement { get; set; }
    public string Phase { get; set; }
    public IDomainContactGroup DomainContactGroup { get; set; }

    private DotTypeFormSchemaLookup(string formType, string tld, string placement, string phase)
    {
      FormType = formType;
      Tld = tld;
      Placement = placement;
      Phase = phase;
    }

    public static IDotTypeFormSchemaLookup Create(string formType, string tld, string placement, string phase)
    {
      return new DotTypeFormSchemaLookup(formType, tld, placement, phase);
    }
  }
}
