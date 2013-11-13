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
      return WindowsUserName.ToLowerInvariant();
    }
  }
}
