using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AddBasketShipping.Interface
{
  public class BasketShippingInfo : Dictionary<string, string>
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

    public string Price
    {
      get
      {
        return GetStringProperty(ShippingFields.ShipTo_Price, string.Empty);
      }
      set
      {
        this[ShippingFields.ShipTo_Price] = value;
      }
    }

    public string FirstName
    {
      get
      {
        return GetStringProperty(ShippingFields.ShipTo_FirstName, string.Empty);
      }
      set
      {
        this[ShippingFields.ShipTo_FirstName] = value;
      }
    }

    public string LastName
    {
      get
      {
        return GetStringProperty(ShippingFields.ShipTo_LastName, string.Empty);
      }
      set
      {
        this[ShippingFields.ShipTo_LastName] = value;
      }
    }

    public string Company
    {
      get
      {
        return GetStringProperty(ShippingFields.ShipTo_Company, string.Empty);
      }
      set
      {
        this[ShippingFields.ShipTo_Company] = value;
      }
    }

    public string Address1
    {
      get
      {
        return GetStringProperty(ShippingFields.ShipTo_Address1, string.Empty);
      }
      set
      {
        this[ShippingFields.ShipTo_Address1] = value;
      }
    }

    public string Address2
    {
      get
      {
        return GetStringProperty(ShippingFields.ShipTo_Address2, string.Empty);
      }
      set
      {
        this[ShippingFields.ShipTo_Address2] = value;
      }
    }

    public string City
    {
      get
      {
        return GetStringProperty(ShippingFields.ShipTo_City, string.Empty);
      }
      set
      {
        this[ShippingFields.ShipTo_City] = value;
      }
    }

    public string State
    {
      get
      {
        return GetStringProperty(ShippingFields.ShipTo_State, string.Empty);
      }
      set
      {
        this[ShippingFields.ShipTo_State] = value;
      }
    }

    public string ZipCode
    {
      get
      {
        return GetStringProperty(ShippingFields.ShipTo_ZipCode, string.Empty);
      }
      set
      {
        this[ShippingFields.ShipTo_ZipCode] = value;
      }
    }

    public string Country
    {
      get
      {
        return GetStringProperty(ShippingFields.ShipTo_Country, string.Empty);
      }
      set
      {
        this[ShippingFields.ShipTo_Country] = value;
      }
    }

    public string WorkPhone
    {
      get
      {
        return GetStringProperty(ShippingFields.ShipTo_WorkPhone, string.Empty);
      }
      set
      {
        this[ShippingFields.ShipTo_WorkPhone] = value;
      }
    }

    public string HomePhone
    {
      get
      {
        return GetStringProperty(ShippingFields.ShipTo_HomePhone, string.Empty);
      }
      set
      {
        this[ShippingFields.ShipTo_HomePhone] = value;
      }
    }

    public string Email
    {
      get
      {
        return GetStringProperty(ShippingFields.ShipTo_Email, string.Empty);
      }
      set
      {
        this[ShippingFields.ShipTo_Email] = value;
      }
    }

    public string Fax
    {
      get
      {
        return GetStringProperty(ShippingFields.ShipTo_Fax, string.Empty);
      }
      set
      {
        this[ShippingFields.ShipTo_Fax] = value;
      }
    }

    public string Method
    {
      get
      {
        return GetStringProperty(ShippingFields.ShipTo_Method, string.Empty);
      }
      set
      {
        this[ShippingFields.ShipTo_Method] = value;
      }
    }    

    public string Province
    {
      get
      {
        return GetStringProperty(ShippingFields.ShipTo_Province, string.Empty);
      }
      set
      {
        this[ShippingFields.ShipTo_Province] = value;
      }
    }

    public string MiddleName
    {
      get
      {
        return GetStringProperty(ShippingFields.shipTo_MiddleName, string.Empty);
      }
      set
      {
        this[ShippingFields.shipTo_MiddleName] = value;
      }
    }

    public string MobilePhone
    {
      get
      {
        return GetStringProperty(ShippingFields.ShipTo_MobilePhone, string.Empty);
      }
      set
      {
        this[ShippingFields.ShipTo_MobilePhone] = value;
      }
    }

    public string ThirdPartyMethod
    {
      get
      {
        return GetStringProperty(ShippingFields.ShipTo_ThirdMethod, string.Empty);
      }
      set
      {
        this[ShippingFields.ShipTo_ThirdMethod] = value;
      }
    }

    public string ThirdPartyAmount
    {
      get
      {
        return GetStringProperty(ShippingFields.ShipTo_ThirdAmount, string.Empty);
      }
      set
      {
        this[ShippingFields.ShipTo_ThirdAmount] = value;
      }
    }

    private class ShippingFields
    {
      public const string ShipTo_Price = "shipping_price";
      public const string ShipTo_FirstName = "ship_to_first_name";
      public const string ShipTo_LastName = "ship_to_last_name";
      public const string shipTo_MiddleName = "ship_to_middle_name";
      public const string ShipTo_Company = "ship_to_company";
      public const string ShipTo_Address1 = "ship_to_street1";
      public const string ShipTo_Address2 = "ship_to_street2";
      public const string ShipTo_City = "ship_to_city";
      public const string ShipTo_State = "ship_to_state";
      public const string ShipTo_ZipCode = "ship_to_zip";
      public const string ShipTo_Country = "ship_to_country";
      public const string ShipTo_WorkPhone = "ship_to_phone1";
      public const string ShipTo_HomePhone = "ship_to_phone2";
      public const string ShipTo_MobilePhone = "ship_to_phone3";
      public const string ShipTo_Email = "ship_to_email";
      public const string ShipTo_Fax = "ship_to_fax";
      public const string ShipTo_Method = "shipping_method";
      public const string ShipTo_ThirdMethod = "third_party_shipping_method";
      public const string ShipTo_ThirdAmount = "third_party_shipping_amount";
      public const string ShipTo_Province = "ship_to_province";
    }
  }
}
