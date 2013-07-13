
using Atlantis.Framework.ShopperValidator.Interface.LanguageResources;

namespace Atlantis.Framework.ShopperValidator.Interface.RuleConstants
{
  internal class FieldNames
  {
    private readonly string _culture = "en";

    private FetchResource _fetchResource;
    private FetchResource FetchResource
    {
      get
      {
        if (_fetchResource == null)
        {
          _fetchResource = new FetchResource(ResourceNamespace.ShopperValidator, _culture);
        }

        return _fetchResource;
      }
    }

    public FieldNames(string culture)
    {
      _culture = culture;
    }

    public string BirthDay { get { return FetchResource.GetString("birthday"); } }
    public string Address1 { get { return FetchResource.GetString("address1"); } }
    public string Address2 { get { return FetchResource.GetString("address2"); } }
    public string Phone { get { return FetchResource.GetString("phone"); } }
    public string CallInPin { get { return FetchResource.GetString("callInPin"); } }
    public string City { get { return FetchResource.GetString("city"); } }
    public string Country { get { return FetchResource.GetString("country"); } }
    public string Email { get { return FetchResource.GetString("email"); } }
    public string FirstName { get { return FetchResource.GetString("firstName"); } }
    public string LastName { get { return FetchResource.GetString("lastName"); } }
    public string Password { get { return FetchResource.GetString("password"); } }
    public string PasswordConfirm { get { return FetchResource.GetString("confirmPassword"); } }
    public string PasswordHint { get { return FetchResource.GetString("passwordHint"); } }
    public string PhoneExtension { get { return FetchResource.GetString("ext"); } }
    public string State { get { return FetchResource.GetString("state"); } }
    public string Username { get { return FetchResource.GetString("username"); } }
    public string Zip { get { return FetchResource.GetString("zip"); } }
  }

  internal enum FieldName
  {
    BirthDay,
    Address1,
    Phone,
    CallInPin,
    City,
    Country,
    Email,
    FirstName,
    LastName,
    Password,
    PasswordConfirm,
    PasswordHint,
    PhoneExtension,
    State,
    Username,
    Zip
  }
}
