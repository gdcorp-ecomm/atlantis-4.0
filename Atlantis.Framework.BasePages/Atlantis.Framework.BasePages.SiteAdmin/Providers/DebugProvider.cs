using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;
using System.Web;

namespace Atlantis.Framework.BasePages.SiteAdmin.Providers
{
  public class DebugProvider : ProviderBase, IDebugContext
  {
    private List<KeyValuePair<string, string>> _debugData;
    private ISiteContext _siteContext;

    public DebugProvider(IProviderContainer container) : base(container)
    {
      _debugData = new List<KeyValuePair<string, string>>();
    }

    private ISiteContext SiteContext
    {
      get
      {
        if (_siteContext == null)
        {
          _siteContext = Container.Resolve<ISiteContext>();
        }
        return _siteContext;
      }
    }

    #region IDebugContext Members

    public List<KeyValuePair<string, string>> GetDebugTrackingData()
    {
      return _debugData;
    }

    public void LogDebugTrackingData(string key, string data)
    {
      if (SiteContext.IsRequestInternal)
      {
        _debugData.Add(new KeyValuePair<string, string>(key, data));
      }
    }

    public string GetQaSpoofQueryValue(string spoofParamName)
    {
      string result = null;
      if (SiteContext.IsRequestInternal)
      {
        return HttpContext.Current.Request[spoofParamName];
      }
      return result;
    }

    #endregion
  }
}
