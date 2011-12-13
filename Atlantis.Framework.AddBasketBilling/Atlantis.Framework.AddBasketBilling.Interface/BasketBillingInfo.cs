using System.Collections.Generic;

namespace Atlantis.Framework.AddBasketBilling.Interface
{
  public class BasketBillingInfo : Dictionary<string, string>
  {

    private string GetStringProperty(string key, string defaultValue)
    {
      string result;
      if (!TryGetValue(key, out result))
      {
        result = defaultValue;
      }
      return result;
    }

    public string FirstName
    {
      get
      {
        return GetStringProperty(BillingFields.BillTo_Firstname, string.Empty);
      }
      set
      {
        this[BillingFields.BillTo_Firstname] = value;
      }
    }

    public string LastName
    {
      get
      {
        return GetStringProperty(BillingFields.BillTo_LastName, string.Empty);
      }
      set
      {
        this[BillingFields.BillTo_LastName] = value;
      }
    }

    public string Company
    {
      get
      {
        return GetStringProperty(BillingFields.BillTo_Company, string.Empty);
      }
      set
      {
        this[BillingFields.BillTo_Company] = value;
      }
    }

    public string Address1
    {
      get
      {
        return GetStringProperty(BillingFields.BillTo_Street1, string.Empty);
      }
      set
      {
        this[BillingFields.BillTo_Street1] = value;
      }
    }

    public string Address2
    {
      get
      {
        return GetStringProperty(BillingFields.BillTo_Street2, string.Empty);
      }
      set
      {
        this[BillingFields.BillTo_Street2] = value;
      }
    }

    public string City
    {
      get
      {
        return GetStringProperty(BillingFields.BillTo_City, string.Empty);
      }
      set
      {
        this[BillingFields.BillTo_City] = value;
      }
    }

    public string State
    {
      get
      {
        return GetStringProperty(BillingFields.BillTo_State, string.Empty);
      }
      set
      {
        this[BillingFields.BillTo_State] = value;
      }
    }

    public string ZipCode
    {
      get
      {
        return GetStringProperty(BillingFields.BillTo_Zip, string.Empty);
      }
      set
      {
        this[BillingFields.BillTo_Zip] = value;
      }
    }

    public string Country
    {
      get
      {
        return GetStringProperty(BillingFields.BillTo_Country, string.Empty);
      }
      set
      {
        this[BillingFields.BillTo_Country] = value;
      }
    }

    public string WorkPhone
    {
      get
      {
        return GetStringProperty(BillingFields.BillTo_Phone1, string.Empty);
      }
      set
      {
        this[BillingFields.BillTo_Phone1] = value;
      }
    }

    public string HomePhone
    {
      get
      {
        return GetStringProperty(BillingFields.BillTo_Phone2, string.Empty);
      }
      set
      {
        this[BillingFields.BillTo_Phone2] = value;
      }
    }

    public string Email
    {
      get
      {
        return GetStringProperty(BillingFields.BillTo_Email, string.Empty);
      }
      set
      {
        this[BillingFields.BillTo_Email] = value;
      }
    }

    public string Fax
    {
      get
      {
        return GetStringProperty(BillingFields.BillTo_Fax, string.Empty);
      }
      set
      {
        this[BillingFields.BillTo_Fax] = value;
      }
    }

    public string MiddleName
    {
      get
      {
        return GetStringProperty(BillingFields.BillTo_MiddleName, string.Empty);
      }
      set
      {
        this[BillingFields.BillTo_MiddleName] = value;
      }
    }

    private class BillingFields
    {
      public const string BillTo_Firstname = "bill_to_first_name";
      public const string BillTo_LastName = "bill_to_last_name";
      public const string BillTo_State = "bill_to_state";
      public const string BillTo_Phone1 = "bill_to_phone1";
      public const string BillTo_Phone2 = "bill_to_phone2";
      public const string BillTo_Company = "bill_to_company";
      public const string BillTo_Street1 = "bill_to_street1";
      public const string BillTo_Street2 = "bill_to_street2";
      public const string BillTo_Zip = "bill_to_zip";
      public const string BillTo_Country = "bill_to_country";
      public const string BillTo_Email = "bill_to_email";
      public const string BillTo_City = "bill_to_city";
      public const string BillTo_Fax = "bill_to_fax";
      public const string BillTo_MiddleName = "bill_to_middle_name";
    }
  }
}
