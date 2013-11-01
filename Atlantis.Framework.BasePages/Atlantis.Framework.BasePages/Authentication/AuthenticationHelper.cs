using Atlantis.Framework.Interface;
using Atlantis.Framework.MiniEncrypt;
using System;

namespace Atlantis.Framework.BasePages.Authentication
{
  public static class AuthenticationHelper
  {
    [Obsolete("Please use the miniEncrypt ParseMstk method directly")]
    public static int GetMgrDecryptedValues(string mstk, out string managerUserId, out string managerUserName)
    {
      int result = 1;
      managerUserId = string.Empty;
      managerUserName = string.Empty;

      if (!string.IsNullOrEmpty(mstk))
      {
        try
        {
          using (var mstkAuth = MstkAuthentication.CreateDisposable())
          {
            result = mstkAuth.ParseMstk(mstk, out managerUserId, out managerUserName);
          }
        }
        catch (Exception ex)
        {
          string data = "mstk=" + mstk;
          AtlantisException aex = new AtlantisException(
            "AuthenticationHelper.GetMgrDecryptedValues", string.Empty, "0", ex.Message + ex.StackTrace, data,
            string.Empty, string.Empty, string.Empty, string.Empty, 0);
          Engine.Engine.LogAtlantisException(aex);
        }
      }

      return result;
    }

    [Obsolete("Please use the miniEncrypt CreateMstk method directly")]
    public static string GetMgrEncryptedValue(string managerUserId, string managerUserName)
    {
      string result = string.Empty;

      if (!string.IsNullOrEmpty(managerUserId) || !string.IsNullOrEmpty(managerUserName))
      {
        try
        {
          using (var mstkAuth = MstkAuthentication.CreateDisposable())
          {
            result = mstkAuth.CreateMstk(managerUserId, managerUserName);
          }
        }
        catch (Exception ex)
        {
          string data = "uid=" + managerUserId + " : uname=" + managerUserName;
          AtlantisException aex = new AtlantisException(
            "AuthenticationHelper.GetMgrEncryptedValue", string.Empty, "0", ex.Message + ex.StackTrace, data,
            string.Empty, string.Empty, string.Empty, string.Empty, 0);
          Engine.Engine.LogAtlantisException(aex);
        }
      }

      return result;
    }
  }
}
