using Atlantis.Framework.Interface;
using System;
using Atlantis.Framework.Providers.AppSettings.Interface;

namespace Atlantis.Framework.Providers.DomainSearch.Tests
{
  public class MockAppSettingsProvider : ProviderBase, IAppSettingsProvider // Framework providers should implement a corresponding interface
  {
    public MockAppSettingsProvider(IProviderContainer container)
      : base(container)
    {
    }

    public string GetAppSetting(string appSettingName)
    {
      string key = "appsetting." + appSettingName;
      return Container.GetData(key, string.Empty);
    }
  }
}
