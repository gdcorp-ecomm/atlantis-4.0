
namespace Atlantis.Framework.BuyerProfileDetails.Interface
{
  public struct AddressList
  {

    #region properties

    private int _contactTypeId;
    private string _contactTypeShort;
    private string _contactTypeDesc;
    private string _firstName;
    private string _middleName;
    private string _lastName;
    private string _organization;
    private string _address1;
    private string _address2;
    private string _city;
    private string _stateOrProvince;
    private string _zipCode;
    private string _country;
    private string _daytimePhone;
    private string _eveningPhone;
    private string _fax;
    private string _email;

    public int ContactTypeId
    {
      get { return _contactTypeId; }
      set { _contactTypeId = value; }
    }

    public string ContactTypeShort
    {
      get { return _contactTypeShort; }
      set { _contactTypeShort = value; }
    }

    public string ContactTypeDesc
    {
      get { return _contactTypeDesc; }
      set { _contactTypeDesc = value; }
    }

    public string FirstName
    {
      get { return _firstName; }
      set { _firstName = value; }
    }

    public string MiddleName
    {
      get { return _middleName; }
      set { _middleName = value; }
    }

    public string LastName
    {
      get { return _lastName; }
      set { _lastName = value; }
    }

    public string Organization
    {
      get { return _organization; }
      set { _organization = value; }
    }

    public string Address1
    {
      get { return _address1; }
      set { _address1 = value; }
    }

    public string Address2
    {
      get { return _address2; }
      set { _address2 = value; }
    }

    public string City
    {
      get { return _city; }
      set { _city = value; }
    }

    public string StateOrProvince
    {
      get { return _stateOrProvince; }
      set { _stateOrProvince = value; }
    }

    public string ZipCode
    {
      get { return _zipCode; }
      set { _zipCode = value; }
    }

    public string Country
    {
      get { return _country; }
      set { _country = value; }
    }

    public string DaytimePhone
    {
      get { return _daytimePhone; }
      set { _daytimePhone = value; }
    }

    public string EveningPhone
    {
      get { return _eveningPhone; }
      set { _eveningPhone = value; }
    }

    public string Fax
    {
      get { return _fax; }
      set { _fax = value; }
    }

    public string Email
    {
      get { return _email; }
      set { _email = value; }
    }

    #endregion

    public AddressList(int contactTypeId, string contactTypeShort, string contactTypeDesc, string firstName,
        string middleName, string lastName, string organization, string address1, string address2, string city,
        string stateOrProvince, string zipCode, string country, string daytimePhone, string eveningPhone,
        string fax, string email)
    {
      _contactTypeId = contactTypeId;
      _contactTypeShort = contactTypeShort;
      _contactTypeDesc = contactTypeDesc;
      _firstName = firstName;
      _middleName = middleName;
      _lastName = lastName;
      _organization = organization;
      _address1 = address1;
      _address2 = address2;
      _city = city;
      _stateOrProvince = stateOrProvince;
      _zipCode = zipCode;
      _country = country;
      _daytimePhone = daytimePhone;
      _eveningPhone = eveningPhone;
      _fax = fax;
      _email = email;

    }

  }
}
