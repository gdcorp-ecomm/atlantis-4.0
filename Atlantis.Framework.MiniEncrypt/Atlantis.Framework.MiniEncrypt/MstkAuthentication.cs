using System;
using System.Runtime.InteropServices;

namespace Atlantis.Framework.MiniEncrypt
{
  public class MstkAuthentication : IDisposable
  {
    private gdMiniEncryptLib.IAuthentication _authenticationClass;

    private MstkAuthentication()
    {
      _authenticationClass = new gdMiniEncryptLib.Authentication();
    }

    ~MstkAuthentication()
    {
      _authenticationClass.SafeRelease();
    }

    public void Dispose()
    {
      _authenticationClass.SafeRelease();
      GC.SuppressFinalize(this);
    }

    public static MstkAuthentication CreateDisposable()
    {
      return new MstkAuthentication();
    }

    public string CreateMstk(string managerUserId, string managerUserName)
    {
      string result = string.Empty;

      if (!string.IsNullOrEmpty(managerUserId) || !string.IsNullOrEmpty(managerUserName))
      {
        object resultObject = _authenticationClass.GetMgrEncryptedValue(managerUserId, managerUserName);
        result = resultObject.ToString();
      }

      return result;
    }

    public int ParseMstk(string mstk, out string managerUserId, out string managerUserName)
    {
      int result = 1;
      managerUserId = string.Empty;
      managerUserName = string.Empty;

      if (!string.IsNullOrEmpty(mstk))
      {
        object userIdObject;
        object userNameObject;

        try
        {
          result = _authenticationClass.GetMgrDecryptedValues(mstk, out userIdObject, out userNameObject);
          managerUserId = userIdObject.ToString();
          managerUserName = userNameObject.ToString();
        }
        catch (Exception)
        {
          result = 1;
          managerUserId = string.Empty;
          managerUserName = string.Empty;
        }
      }

      return result;
    }
  }
}
