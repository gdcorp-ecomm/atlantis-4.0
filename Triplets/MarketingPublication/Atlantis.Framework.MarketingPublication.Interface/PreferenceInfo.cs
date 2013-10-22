using System;
using System.Collections.Generic;

namespace Atlantis.Framework.MarketingPublication.Interface
{
  public class PreferenceInfo : Dictionary<string, string>
  {

    public string DateAdded
    {
      get
      {
        return GetStringProperty("Date", string.Empty);
      }
      set
      {
        this["date"] = value;
      }
    }

    public int CommTypeID
    {
      get
      {
         return GetIntProperty("CommTypeID", -1);
      }
      set
      {
        this["CommTypeID"] = value.ToString();
      }
    }

    public int InterestTypeID
    {
      get
      {
        return GetIntProperty("InterestTypeID", -1);
      }
      set
      {
        this["InterestTypeID"] = value.ToString();
      }
    }

    public string DoubleOptinDate
    {
      get
      {
        return GetStringProperty("DoubleOptInDate", string.Empty);
      }
      set
      {
        this["DoubleOptInDate"] = value;
      }
    }

    public int GetIntProperty(string key, int defaultValue)
    {
      int result;
      string stringValue;
      if (!TryGetValue(key, out stringValue))
      {
        result = defaultValue;
      }
      else
      {
        if (!Int32.TryParse(stringValue, out result))
        {
          decimal testValue = 0;
          if (!decimal.TryParse(stringValue, out testValue))
          {
            result = defaultValue;
          }
          else
          {
            result = (int)testValue;
          }
        }
      }
      return result;
    }

    public string GetStringProperty(string key, string defaultValue)
    {
      string result = defaultValue;
      if (!TryGetValue(key, out result))
      {
        result = defaultValue;
      }
      return result;
    }

  }
}
