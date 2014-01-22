using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.AppSettings.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.PlaceHolder.Tests
{
  public class MockAppSettingsProvider : ProviderBase, IAppSettingsProvider
  {

    public MockAppSettingsProvider(IProviderContainer container) : base(container) { }

    public string ReturnValue { get; set; }

    public string GetAppSetting(string appSettingName)
    {
      return ReturnValue ?? "true";
    }
  }
}
