using System;
using System.Collections.Generic;

namespace Atlantis.Framework.CMSCreditAccounts.Interface
{
  public class CreditAccountInfo:Dictionary<string,string>
  {
    public string DomainName
    {
      get
      {
        return GetStringProperty("name", string.Empty);
      }
      set
      {
        this["name"] = value;
      }
    }

    public int ParentResourceID
    {
      get
      {
        return GetIntProperty("parentresourceid", -1);
      }
      set
      {
        this["parentresourceid"] = value.ToString();
      }
    }
    public int ParentResourceType
    {
      get
      {
        return GetIntProperty("parentresourcetype", -1);
      }
      set
      {
        this["parentresourcetype"] = value.ToString();
      }
    }
    public int ChildUnifiedPFID
    {
      get
      {
        return GetIntProperty("childunifiedproductid", -1);
      }
      set
      {
        this["childunifiedproductid"] = value.ToString();
      }
    }
    public CreditAccountInfo()
    {

    }

    public CreditAccountInfo(string domainName, int parentResourceID, int parentResourceType, int childUnifiedPfid)
    {
      DomainName = domainName;
      ParentResourceID = parentResourceID;
      ParentResourceType = parentResourceType;
      ChildUnifiedPFID = childUnifiedPfid;
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
