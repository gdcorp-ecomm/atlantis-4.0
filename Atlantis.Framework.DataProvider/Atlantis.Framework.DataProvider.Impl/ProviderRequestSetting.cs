using System.Collections.Generic;

namespace Atlantis.Framework.DataProvider.Impl
{
  internal enum ProviderRequestSettingType
  {
    None = 0,
    StoredProcedure = 1,
    WebService = 2,
    RestService = 3
  }

  internal class ProviderRequestSetting
  {
    public string RequestName;
    public string HostName;
    public string DSN;
    public string AppName;
    public string CertName;
    public string TargetName;
    public List<ProviderParameter> ParamList;
    public ProviderRequestSettingType RequestSettingType;
  }
}
