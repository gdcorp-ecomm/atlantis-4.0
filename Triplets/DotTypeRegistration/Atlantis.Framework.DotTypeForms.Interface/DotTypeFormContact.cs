namespace Atlantis.Framework.DotTypeForms.Interface
{
  public class DotTypeFormContact
  {
    public DotTypeFormContactTypes ContactType { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Company { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string CountryCode { get; set; }
    public string Phone { get; set; }
    public string Fax { get; set; }
    public string Email { get; set; }

    public DotTypeFormContact(DotTypeFormContactTypes contactType, string firstName, string lastName, string company, string address1, string address2,
                        string city, string state, string zip, string countryCode, string phone, string fax,
                        string email)
    {
      ContactType = contactType;
      FirstName = firstName;
      LastName = lastName;
      Company = company;
      Address1 = address1;
      Address2 = address2;
      City = city;
      State = state;
      Zip = zip;
      CountryCode = countryCode;
      Phone = phone;
      Fax = fax;
      Email = email;
    }
  }
}
