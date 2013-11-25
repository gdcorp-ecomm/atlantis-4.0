using System.Globalization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Manager.Interface
{
  public class ManagerCategoriesRequestData : RequestData
  {
    public int ManagerUserId { get; private set; }

    public ManagerCategoriesRequestData(int managerUserId)
    {
      ManagerUserId = managerUserId;
    }

    public override string GetCacheMD5()
    {
      // The "\v2" is to have the MD5 be different so it doesn't namespace clash with the previous ManagerCategories DLL.
      return ManagerUserId.ToString(CultureInfo.InvariantCulture) + "\v2";
    }
  }
}
