using Atlantis.Framework.Providers.DomainContactValidation.Interface;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public interface IDotTypeFormSchemaLookup
  {
    string FormType { get; set; }
    string Tld { get; set; }
    string Placement { get; set; }
    string Phase { get; set; }
    IDomainContactGroup DomainContactGroup { get; set; }
  }
}
