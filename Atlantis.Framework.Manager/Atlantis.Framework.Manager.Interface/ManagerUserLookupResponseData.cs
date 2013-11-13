using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Manager.Interface
{
  public class ManagerUserLookupResponseData : ResponseData
  {
    public static ManagerUserLookupResponseData Invalid { get; private set; }

    static ManagerUserLookupResponseData()
    {
      Invalid = new ManagerUserLookupResponseData(); 
    }

    public static ManagerUserLookupResponseData FromCacheDataXml(string cacheDataXml)
    {
      if (string.IsNullOrEmpty(cacheDataXml))
      {
        return Invalid;
      }

      int managerUserId;
      string loginName;

      var dataElement = XElement.Parse(cacheDataXml);
      var item = dataElement.Descendants("item").FirstOrDefault();

      if (item == null)
      {
        return Invalid;
      }

      string managerUserIdText = item.Attribute("ManagerUserID") != null ? item.Attribute("ManagerUserID").Value : string.Empty;
      if (!int.TryParse(managerUserIdText, out managerUserId))
      {
        return Invalid;
      }

      string firstName = item.Attribute("FirstName") != null ? item.Attribute("FirstName").Value : string.Empty;
      string lastName = item.Attribute("LastName") != null ? item.Attribute("LastName").Value : string.Empty;

      return new ManagerUserLookupResponseData(managerUserId, firstName, lastName);
    }

    public int ManagerUserId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public bool IsValid { get; private set; }

    public string FullName
    {
      get
      {
        return string.Concat(FirstName, string.IsNullOrEmpty(FirstName) ? string.Empty : " ", LastName);
      }
    }

    private ManagerUserLookupResponseData()
    {
      ManagerUserId = 0;
      FirstName = string.Empty;
      LastName = string.Empty;
      IsValid = false;
    }

    private ManagerUserLookupResponseData(int managerUserId, string firstName, string lastName)
    {
      ManagerUserId = managerUserId;
      FirstName = firstName;
      LastName = lastName;
      IsValid = ManagerUserId != 0;
    }
  }
}
