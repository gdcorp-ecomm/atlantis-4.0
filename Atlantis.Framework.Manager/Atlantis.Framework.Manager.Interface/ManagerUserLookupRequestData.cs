using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Manager.Interface
{
  public class ManagerUserLookupRequestData : RequestData
  {
    public string WindowsUserName { get; private set; }

    public ManagerUserLookupRequestData(string windowsUserName)
    {
      WindowsUserName = windowsUserName ?? string.Empty;
    }

    public override string GetCacheMD5()
    {
      // The "\v2" is to have the MD5 be different so it doesn't namespace clash with the previous ManagerUser DLL.
      return WindowsUserName.ToLowerInvariant() + "\v2";
    }
  }
}
