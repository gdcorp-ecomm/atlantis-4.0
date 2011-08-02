using System.Collections.Generic;

namespace Atlantis.Framework.EcommPaymentProfiles.Interface
{
  public class PaymentProfile : Dictionary<string, string>
  {
    public string GetStringProperty(string key, string defaultValue)
    {
      string result;
      if (!TryGetValue(key, out result))
      {
        result = defaultValue;
      }
      return result;
    }

    public string ProfileID
    {
      get { return GetStringProperty(PaymentProfileFields.ProfileID, string.Empty); }
      set { this[PaymentProfileFields.ProfileID] = value; }
    }

    public string ProfileType
    {
      get { return GetStringProperty(PaymentProfileFields.ProfileType, string.Empty); }
      set { this[PaymentProfileFields.ProfileType] = value; }
    }

    public string CreditCardType
    {
      get { return GetStringProperty(PaymentProfileFields.CreditCardType, string.Empty); }
      set { this[PaymentProfileFields.CreditCardType] = value; }
    }

    public string HolderName
    {
      get { return GetStringProperty(PaymentProfileFields.HolderName, string.Empty); }
      set { this[PaymentProfileFields.HolderName] = value; }
    }

    public string CreditCardNumber
    {
      get { return GetStringProperty(PaymentProfileFields.CCNumber, string.Empty); }
      set { this[PaymentProfileFields.CCNumber] = value; }
    }

    public string AccountNumber
    {
      get { return GetStringProperty(PaymentProfileFields.AccountNumber, string.Empty); }
      set { this[PaymentProfileFields.AccountNumber] = value; }
    }

    public string ExpirationMonth
    {
      get { return GetStringProperty(PaymentProfileFields.ExpirationMonth, string.Empty); }
      set { this[PaymentProfileFields.ExpirationMonth] = value; }
    }

    public string ExpirationYear
    {
      get { return GetStringProperty(PaymentProfileFields.ExpirationYear, string.Empty); }
      set { this[PaymentProfileFields.ExpirationYear] = value; }
    }

    public string Company
    {
      get { return GetStringProperty(PaymentProfileFields.Company, string.Empty); }
      set { this[PaymentProfileFields.Company] = value; }
    }

    public string FirstName
    {
      get { return GetStringProperty(PaymentProfileFields.FirstName, string.Empty); }
      set { this[PaymentProfileFields.FirstName] = value; }
    }

    public string Address1
    {
      get { return GetStringProperty(PaymentProfileFields.Address1, string.Empty); }
      set { this[PaymentProfileFields.Address1] = value; }
    }

    public string LastName
    {
      get { return GetStringProperty(PaymentProfileFields.LastName, string.Empty); }
      set { this[PaymentProfileFields.LastName] = value; }
    }

    public string Address2
    {
      get { return GetStringProperty(PaymentProfileFields.Address2, string.Empty); }
      set { this[PaymentProfileFields.Address2] = value; }
    }

    public string City
    {
      get { return GetStringProperty(PaymentProfileFields.City, string.Empty); }
      set { this[PaymentProfileFields.City] = value; }
    }

    public string State
    {
      get { return GetStringProperty(PaymentProfileFields.State, string.Empty); }
      set { this[PaymentProfileFields.State] = value; }
    }

    public string Country
    {
      get { return GetStringProperty(PaymentProfileFields.Country, string.Empty); }
      set { this[PaymentProfileFields.Country] = value; }
    }

    public string PostalCode
    {
      get { return GetStringProperty(PaymentProfileFields.PostalCode, string.Empty); }
      set { this[PaymentProfileFields.PostalCode] = value; }
    }

    public string Phone1
    {
      get { return GetStringProperty(PaymentProfileFields.Phone1, string.Empty); }
      set { this[PaymentProfileFields.Phone1] = value; }
    }

    public string Phone2
    {
      get { return GetStringProperty(PaymentProfileFields.Phone2, string.Empty); }
      set { this[PaymentProfileFields.Phone2] = value; }
    }

    public string Fax
    {
      get { return GetStringProperty(PaymentProfileFields.Fax, string.Empty); }
      set { this[PaymentProfileFields.Fax] = value; }
    }

    public string Email
    {
      get { return GetStringProperty(PaymentProfileFields.Email, string.Empty); }
      set { this[PaymentProfileFields.Email] = value; }
    }

    public string AccountName
    {
      get { return GetStringProperty(PaymentProfileFields.AccountName, string.Empty); }
      set { this[PaymentProfileFields.AccountName] = value; }
    }

    public string RoutingNumber
    {
      get { return GetStringProperty(PaymentProfileFields.RoutingNumber, string.Empty); }
      set { this[PaymentProfileFields.RoutingNumber] = value; }
    }

    public string AccountType
    {
      get { return GetStringProperty(PaymentProfileFields.AccountType, string.Empty); }
      set { this[PaymentProfileFields.AccountType] = value; }
    }

    public string LicenseNo
    {
      get { return GetStringProperty(PaymentProfileFields.LicenseNo, string.Empty); }
      set { this[PaymentProfileFields.LicenseNo] = value; }
    }

    public string LicenseState
    {
      get { return GetStringProperty(PaymentProfileFields.LicenseState, string.Empty); }
      set { this[PaymentProfileFields.LicenseState] = value; }
    }

    public string DatOfBirth
    {
      get { return GetStringProperty(PaymentProfileFields.DatOfBirth, string.Empty); }
      set { this[PaymentProfileFields.DatOfBirth] = value; }
    }

    private class PaymentProfileFields
    {
      public const string ProfileID = "profile_id";
      public const string ProfileType = "type";
      public const string CreditCardType = "cc_type";
      public const string HolderName = "cc_name";
      public const string CCNumber = "cc_number";
      public const string ExpirationMonth = "cc_expmonth";
      public const string ExpirationYear = "cc_expyear";
      public const string Company = "company";
      public const string FirstName = "firstName";
      public const string LastName = "lastName";
      public const string Address1 = "street1";
      public const string Address2 = "street2";
      public const string City = "city";
      public const string State = "state";
      public const string Country = "country";
      public const string PostalCode = "postal_code";
      public const string Phone1 = "phone1";
      public const string Phone2 = "phone2";
      public const string Fax = "fax";
      public const string Email = "email";
      public const string AccountName = "account_name";
      public const string RoutingNumber = "routing_number";
      public const string AccountNumber = "account_number";
      public const string AccountType = "account_type";
      public const string LicenseNo = "license_no";
      public const string LicenseState = "license_state";
      public const string DatOfBirth = "dob";

      public const string ShopperID = "shopper_id";
    }
  }
}
