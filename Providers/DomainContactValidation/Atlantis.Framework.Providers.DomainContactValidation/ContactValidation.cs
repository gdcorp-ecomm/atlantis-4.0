using System.Dynamic;
using Atlantis.Framework.Providers.DomainContactValidation.Interface;

namespace Atlantis.Framework.Providers.DomainContactValidation
{
  public class ContactValidation : IContactValidation
  {
    public string Address1 { get; private set; }
    public string Address2 { get; private set; }
    public string CanadianPresence { get; private set; }
    public string City { get; private set; }
    public string Company { get; private set; }
    public string Country { get; private set; }
    public string Email { get; private set; }
    public string Fax { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Phone { get; private set; }
    public string State { get; private set; }
    public string Zip { get; private set; }

    public static ContactValidation Create(string firstName, string lastName, string email, string company, string address1, string address2, string city, string state, string zip, string country,
      string phone, string fax, string canadianPresence)
    {
      var contact = new ContactValidation
      {
        FirstName = firstName,
        LastName = lastName,
        Email = email,
        Company = company,
        Address1 = address1,
        Address2 = address2,
        City = city,
        State = state,
        Zip = zip,
        Country = country,
        Phone = phone,
        Fax = fax,
        CanadianPresence = canadianPresence
      };

      return contact;
    }
  }
}
