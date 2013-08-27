using System;

namespace Atlantis.Framework.MiniEncrypt
{
  public class Mstk : MiniEncryptBase
  {
    private gdMiniEncryptLib.IAuthentication _authenticationClass;

    private Mstk ()
    {
      _authenticationClass = new gdMiniEncryptLib.Authentication();
    }

    ~Mstk()
    {
      ReleaseObject(_authenticationClass);
    }

    public override void Dispose()
    {
      ReleaseObject(_authenticationClass);
      GC.SuppressFinalize(this);
    }

    public static Mstk CreateDisposable() 
    {
      return new Mstk();
    }
    
  //todo add try catch, rn
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

        result = _authenticationClass.GetMgrDecryptedValues(mstk, out userIdObject, out userNameObject);
        managerUserId = userIdObject.ToString();
        managerUserName = userNameObject.ToString();
      }

      return result;
    }
  }
}
