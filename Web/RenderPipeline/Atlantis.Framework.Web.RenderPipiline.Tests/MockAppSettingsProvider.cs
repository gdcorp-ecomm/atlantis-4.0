using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.AppSettings.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Atlantis.Framework.Web.RenderPipiline.Tests
{
  public class MockAppSettingsProvider : ProviderBase, IAppSettingsProvider
  {
    protected readonly ISiteContext _siteContext;

    public MockAppSettingsProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = container.Resolve<ISiteContext>();
    }


    public string GetAppSetting(string appSettingName)
    {
      string result = Container.GetData<string>("MockAppSettingsProvider.ReturnValue", null);
      if (_siteContext.IsRequestInternal)
      {
        result = _InternalGetSetting(appSettingName, result);
      }
      return result;
    }

    virtual protected string _InternalGetSetting(string appSettingName, string result)
    {
      string spoofParam = FormQueryStringName(appSettingName);
      string spoofValue = HttpContext.Current.Request[spoofParam];
      // check if request has an override
      if (spoofValue != null)
      {
        result = spoofValue;
      }

      return result;
    }

    protected const string QaPrefix = "QA--";
    public static string FormQueryStringName(string appSettingName)
    {
      return QaPrefix + appSettingName;
    }
  }
}
