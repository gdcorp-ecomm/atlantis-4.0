using System;
using Atlantis.Framework.Interface;
using System.Collections.Generic;
using System.Web;

namespace Atlantis.Framework.BasePages.Providers
{
  public class DebugProvider : ProviderBase, IDebugContext
  {
    private readonly List<KeyValuePair<string, string>> _debugData;
    private readonly Lazy<ISiteContext> _siteContext;

    public DebugProvider(IProviderContainer container) : base(container)
    {
      _debugData = new List<KeyValuePair<string, string>>();
      _siteContext = new Lazy<ISiteContext>(LoadSiteContext);
    }

    private ISiteContext LoadSiteContext()
    {
      ISiteContext result;
      Container.TryResolve(out result);
      return result;
    }

    #region IDebugContext Members

    public List<KeyValuePair<string, string>> GetDebugTrackingData()
    {
      return _debugData;
    }

    public void LogDebugTrackingData(string key, string data)
    {
      _debugData.Add(new KeyValuePair<string, string>(key, data));
    }

    public string GetQaSpoofQueryValue(string spoofParamName)
    {
      if ((HttpContext.Current != null) && (_siteContext.Value != null) && (_siteContext.Value.IsRequestInternal))
      {
        return HttpContext.Current.Request[spoofParamName];
      }

      return null;
    }

    #endregion

  }
}
