using System.Security.Principal;
using Atlantis.Framework.SiteAdminRoles.Interface;
using System.Web;
using Atlantis.Framework.Interface;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;

namespace Atlantis.Framework.BasePages.SiteAdmin.Security
{
  public static class SiteAdminSecurity
  {
    private static bool IsUserInRole(ISiteContext siteContext, WindowsIdentity user, int siteAdminRoleId)
    {
      bool result = false;

      if (user != null)
      {
        SiteAdminRolesRequestData request = new SiteAdminRolesRequestData(
          user.Name, RequestUrl, string.Empty, siteContext.Pathway, siteContext.PageCount,
          siteAdminRoleId);
        SiteAdminRolesResponseData response = (SiteAdminRolesResponseData)DataCache.DataCache.GetProcessRequest(request, SiteAdminBaseEngineRequests.SiteAdminRoles);
        result = (response.IsSuccess && response.IsUserAllowed(user));
      }

      return result;
    }

    public static bool IsCurrentUserInRole(ISiteContext siteContext, int siteAdminRoleId)
    {
      WindowsIdentity user = GetCurrentUser();
      return IsUserInRole(siteContext, user, siteAdminRoleId);
    }

    public static bool IsCurrentUserInRole(ISiteContext siteContext, params int[] siteAdminRoleIds)
    {
      bool result = false;
      WindowsIdentity user = GetCurrentUser();

      if ((user != null) && (siteAdminRoleIds != null))
      {
        foreach (int siteAdminRoldId in siteAdminRoleIds)
        {
          if (IsUserInRole(siteContext, user, siteAdminRoldId))
          {
            result = true;
            break;
          }
        }
      }

      return result;
    }

    public static bool IsCurrentUserInRole(ISiteContext siteContext, IEnumerable<int> siteAdminRoleIds)
    {
      bool result = false;
      WindowsIdentity user = GetCurrentUser();

      if ((user != null) && (siteAdminRoleIds != null))
      {
        foreach (int siteAdminRoldId in siteAdminRoleIds)
        {
          if (IsUserInRole(siteContext, user, siteAdminRoldId))
          {
            result = true;
            break;
          }
        }
      }

      return result;
    }

    private static WindowsIdentity GetCurrentUser()
    {
      WindowsIdentity result = null;
      if (HttpContext.Current != null)
      {
        result = HttpContext.Current.User.Identity as WindowsIdentity;
      }
      return result;
    }

    private static string RequestUrl
    {
      get
      {
        string result = string.Empty;
        if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
        {
          result = HttpContext.Current.Request.RawUrl;
        }
        return result;
      }
    }



  }
}
