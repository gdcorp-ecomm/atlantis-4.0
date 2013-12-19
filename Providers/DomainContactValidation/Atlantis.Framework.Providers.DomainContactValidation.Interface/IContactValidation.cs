namespace Atlantis.Framework.Providers.DomainContactValidation.Interface
{
  public interface IContactValidation
  {
    string Address1 { get; }
    string Address2 { get; }
    string CanadianPresence { get; }
    string City { get; }
    string Company { get; }
    string Country { get; }
    string Email { get; }
    string Fax { get; }
    string FirstName { get; }
    string LastName { get; }
    string Phone { get; }
    string State { get; }
    string Zip { get; }
  }
}
