
namespace Atlantis.Framework.PurchaseBasket.Interface
{
  public class PurchaseBillingInfo : PurchaseElement
  {
    public override string ElementName
    {
      get { return "BillingInfo"; }
    }

    public string City
    {
      get { return GetStringProperty("city", string.Empty); }
      set { this["city"] = value; }
    }

    public string Company
    {
      get { return GetStringProperty("company", string.Empty); }
      set { this["company"] = value; }
    }

    public string Country
    {
      get { return GetStringProperty("country", string.Empty); }
      set { this["country"] = value; }
    }

    public string Email
    {
      get { return GetStringProperty("email", string.Empty); }
      set { this["email"] = value; }
    }

    public string FAX
    {
      get { return GetStringProperty("fax", string.Empty); }
      set { this["fax"] = value; }
    }

    public string FirstName
    {
      get { return GetStringProperty("first_name", string.Empty); }
      set { this["first_name"] = value; }
    }

    public string MiddleName
    {
      get { return GetStringProperty("middle_name", string.Empty); }
      set { this["middle_name"] = value; }
    }

    public string LastName
    {
      get { return GetStringProperty("last_name", string.Empty); }
      set { this["last_name"] = value; }
    }

    public string Phone1
    {
      get { return GetStringProperty("phone1", string.Empty); }
      set { this["phone1"] = value; }
    }

    public string Phone2
    {
      get { return GetStringProperty("phone2", string.Empty); }
      set { this["phone2"] = value; }
    }

    public string State
    {
      get { return GetStringProperty("state", string.Empty); }
      set { this["state"] = value; }
    }

    public string Street1
    {
      get { return GetStringProperty("street1", string.Empty); }
      set { this["street1"] = value; }
    }

    public string Street2
    {
      get { return GetStringProperty("street2", string.Empty); }
      set { this["street2"] = value; }
    }

    public string Zip
    {
      get { return GetStringProperty("zip", string.Empty); }
      set { this["zip"] = value; }
    }

  }
}
