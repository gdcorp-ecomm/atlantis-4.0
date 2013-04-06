using System.Data;
using Atlantis.Framework.Manager.Interface.ManagerUser;

namespace Atlantis.Framework.Providers.Manager.Interface
{
  public interface IManagerProvider
  {
    #region ManagerCategories

    bool IsCurrentUserInAnyRole(params int[] managerCategories);
    bool IsCurrentUserInAllRoles(params int[] managerCategories);
    bool HasManagerCategory(int category);
    bool HasManagerAttribute(string key);
    bool TryGetManagerAttribute(string key, out string value);

    #endregion

    #region ManagerUserLookup

    ManagerUserData GetManagerUserData();
    bool HasManagerUserData();

    #endregion

    #region ManagerProductDetail

    DataTable GetProductCatalogDetails(decimal pfid, int adminFlag);

    #endregion
  }
}
