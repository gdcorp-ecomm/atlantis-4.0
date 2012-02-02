using System.Collections.Generic;

namespace Atlantis.Framework.PayeeProfileClass.Interface
{
  public class PayeeProfile : Dictionary<string, string>
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

    #region Account
    public string CapID
    {
      get { return GetStringProperty(PayeeProfileFields.CapID, string.Empty); }
      set { this[PayeeProfileFields.CapID] = value; }
    }

    public string FriendlyName
    {
      get { return GetStringProperty(PayeeProfileFields.FriendlyName, string.Empty); }
      set { this[PayeeProfileFields.FriendlyName] = value; }
    }

    public string TaxDeclarationTypeID
    {
      get { return GetStringProperty(PayeeProfileFields.TaxDeclarationTypeID, string.Empty); }
      set { this[PayeeProfileFields.TaxDeclarationTypeID] = value; }
    }

    public string TaxStatusTypeID
    {
      get { return GetStringProperty(PayeeProfileFields.TaxStatusTypeID, string.Empty); }
      set { this[PayeeProfileFields.TaxStatusTypeID] = value; }
    }

    public string TaxStatusText
    {
      get { return GetStringProperty(PayeeProfileFields.TaxStatusText, string.Empty); }
      set { this[PayeeProfileFields.TaxStatusText] = value; }     
    }

    public string TaxID
    {
      get { return GetStringProperty(PayeeProfileFields.TaxID, string.Empty); }
      set { this[PayeeProfileFields.TaxID] = value; }
    }

    public string TaxIDTypeID
    {
      get { return GetStringProperty(PayeeProfileFields.TaxIDTypeID, string.Empty); }
      set { this[PayeeProfileFields.TaxIDTypeID] = value; }
    }

    public string TaxExemptTypeID
    {
      get { return GetStringProperty(PayeeProfileFields.TaxExemptTypeID, string.Empty); }
      set { this[PayeeProfileFields.TaxExemptTypeID] = value; }
    }

    public string TaxCertificationTypeID
    {
      get { return GetStringProperty(PayeeProfileFields.TaxCertificationTypeID, string.Empty); }
      set { this[PayeeProfileFields.TaxCertificationTypeID] = value; }
    }

    public string SubmitterName
    {
      get { return GetStringProperty(PayeeProfileFields.SubmitterName, string.Empty); }
      set { this[PayeeProfileFields.SubmitterName] = value; }
    }

    public string SubmitterTitle
    {
      get { return GetStringProperty(PayeeProfileFields.SubmitterTitle, string.Empty); }
      set { this[PayeeProfileFields.SubmitterTitle] = value; }
    }

    public string TaxUpdateDate
    {
      get { return GetStringProperty(PayeeProfileFields.TaxUpdateDate, string.Empty); }
      set { this[PayeeProfileFields.TaxUpdateDate] = value; }
    }

    public string PaymentMethodTypeID
    {
      get { return GetStringProperty(PayeeProfileFields.PaymentMethodTypeID, string.Empty); }
      set { this[PayeeProfileFields.PaymentMethodTypeID] = value; }
    }

    public string TaxIDContactNameChangeDate
    {
      get { return GetStringProperty(PayeeProfileFields.TaxIDContactNameChangeDate, string.Empty); }
      set { this[PayeeProfileFields.TaxIDContactNameChangeDate] = value; }
    }

    public string CreateDate
    {
      get { return GetStringProperty(PayeeProfileFields.CreateDate, string.Empty); }
      set { this[PayeeProfileFields.CreateDate] = value; }
    }

    private ACHClass _ach = new ACHClass();
    public ACHClass ACH
    {
      get { return _ach; }
      set { _ach = value; }
    }

    private List<AddressClass> _address = new List<AddressClass>();
    public List<AddressClass> Address
    {
      get { return _address; }
      set { _address = value; }
    }

    private PayPalClass _paypal = new PayPalClass();
    public PayPalClass PayPal
    {
      get { return _paypal; }
      set { _paypal = value; }
    }

    private GAGClass _gag = new GAGClass();
    public GAGClass GAG
    {
      get { return _gag; }
      set { _gag = value; }
    }

    private class PayeeProfileFields
    {
      public const string CapID = "capid";
      public const string FriendlyName = "friendlyname";
      public const string TaxDeclarationTypeID = "taxdeclarationtypeid";
      public const string TaxStatusTypeID = "taxstatustypeid";
      public const string TaxStatusText = "taxstatustext";
      public const string TaxID = "taxid";
      public const string TaxIDTypeID = "taxidtypeid";
      public const string TaxExemptTypeID = "taxexempttypeid";
      public const string TaxCertificationTypeID = "taxcertificationtypeid";
      public const string SubmitterName = "submittername";
      public const string SubmitterTitle = "submittertitle";
      public const string TaxUpdateDate = "taxupdatedate";
      public const string PaymentMethodTypeID = "paymentmethodtypeid";
      public const string TaxIDContactNameChangeDate = "taxidcontactnamechangedate";
      public const string CreateDate = "createdate";    
      public const string ShopperID = "shopperid";
    }
    #endregion

    #region ACH
    public class ACHClass : Dictionary<string, string>
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

      public string AchPrenoteDate
      {
        get { return GetStringProperty(ACHFields.AchPrenoteDate, string.Empty); }
        set { this[ACHFields.AchPrenoteDate] = value; }
      }

      public string AchBankName
      {
        get { return GetStringProperty(ACHFields.AchBankName, string.Empty); }
        set { this[ACHFields.AchBankName] = value; }
      }

      public string AccountNumber
      {
        get { return GetStringProperty(ACHFields.AccountNumber, string.Empty); }
        set { this[ACHFields.AccountNumber] = value; }
      }

      public string AccountOrganizationTypeID
      {
        get { return GetStringProperty(ACHFields.AccountOrganizationTypeID, string.Empty); }
        set { this[ACHFields.AccountOrganizationTypeID] = value; }
      }

      public string AccountTypeID
      {
        get { return GetStringProperty(ACHFields.AccountTypeID, string.Empty); }
        set { this[ACHFields.AccountTypeID] = value; }
      }

      public string PaymentUpdateDate
      {
        get { return GetStringProperty(ACHFields.PaymentUpdateDate, string.Empty); }
        set { this[ACHFields.PaymentUpdateDate] = value; }
      }


      public string AchPrenoteStatusID
      {
        get { return GetStringProperty(ACHFields.AchPrenoteStatusID, string.Empty); }
        set { this[ACHFields.AchPrenoteStatusID] = value; }
      }

      public string AchRTN
      {
        get { return GetStringProperty(ACHFields.AchRTN, string.Empty); }
        set { this[ACHFields.AchRTN] = value; }
      }
      
      private class ACHFields
      {
        public const string AchPrenoteDate = "achprenotedate";
        public const string AchBankName = "achbankname";
        public const string AccountNumber = "accountnumber";
        public const string AccountOrganizationTypeID = "accountorganizationtypeid";
        public const string AccountTypeID = "accounttypeid";
        public const string PaymentUpdateDate = "paymentupdatedate";
        public const string AchPrenoteStatusID = "achprenotestatusid";
        public const string AchRTN = "achrtn";        
      }

    }
    #endregion

    #region Address
    public class AddressClass : Dictionary<string, string>
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

      public string AddressType
      {
        get { return GetStringProperty(AddressFields.AddressType, string.Empty); }
        set { this[AddressFields.AddressType] = value; }
      }

      public string ContactName
      {
        get { return GetStringProperty(AddressFields.ContactName, string.Empty); }
        set { this[AddressFields.ContactName] = value; }
      }
      public string Address1
      {
        get { return GetStringProperty(AddressFields.Address1, string.Empty); }
        set { this[AddressFields.Address1] = value; }
      }

      public string Address2
      {
        get { return GetStringProperty(AddressFields.Address2, string.Empty); }
        set { this[AddressFields.Address2] = value; }
      }

      public string City
      {
        get { return GetStringProperty(AddressFields.City, string.Empty); }
        set { this[AddressFields.City] = value; }
      }

      public string StateOrProvince
      {
        get { return GetStringProperty(AddressFields.StateOrProvince, string.Empty); }
        set { this[AddressFields.StateOrProvince] = value; }
      }

      public string PostalCode
      {
        get { return GetStringProperty(AddressFields.PostalCode, string.Empty); }
        set { this[AddressFields.PostalCode] = value; }
      }

      public string Country
      {
        get { return GetStringProperty(AddressFields.Country, string.Empty); }
        set { this[AddressFields.Country] = value; }
      }

      public string Phone1
      {
        get { return GetStringProperty(AddressFields.Phone1, string.Empty); }
        set { this[AddressFields.Phone1] = value; }
      }

      public string Phone2
      {
        get { return GetStringProperty(AddressFields.Phone2, string.Empty); }
        set { this[AddressFields.Phone2] = value; }
      }

      public string Fax
      {
        get { return GetStringProperty(AddressFields.Fax, string.Empty); }
        set { this[AddressFields.Fax] = value; }
      }

      private class AddressFields
      {
        public const string AddressType = "type";
        public const string ContactName = "contactname";
        public const string Address1 = "address1";
        public const string Address2 = "address2";
        public const string City = "city";
        public const string StateOrProvince = "stateorprovince";
        public const string PostalCode = "postalcode";
        public const string Country = "country";
        public const string Phone1 = "phone1";
        public const string Phone2 = "phone2";
        public const string Fax = "fax";
      }
    }
    #endregion

    #region PayPal
    public class PayPalClass : Dictionary<string, string>
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

      public string Email
      {
        get { return GetStringProperty(PayPalFields.Email, string.Empty); }
        set { this[PayPalFields.Email] = value; }
      }

      private class PayPalFields
      {
        public const string Email = "email";
      }
    }
    #endregion

    #region GoodAsGold
    public class GAGClass : Dictionary<string, string>
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

      public string AchPrenoteDate
      {
        get { return GetStringProperty(GAGFields.ShopperID, string.Empty); }
        set { this[GAGFields.ShopperID] = value; }
      }

      private class GAGFields
      {
        public const string ShopperID = "shopperid";
      }
    }
    #endregion
  }
}
