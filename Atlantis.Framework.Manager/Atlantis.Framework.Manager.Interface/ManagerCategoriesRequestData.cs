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
      return ManagerUserId.ToString(CultureInfo.InvariantCulture);
    }
  }
}
