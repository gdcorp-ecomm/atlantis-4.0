using System.Web;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BasePages.Providers
{
  public class PrivateLabelAwareSiteContextProvider : SiteContextProviderBase
  {
    private static bool _enableWWDStyleId = false;
    public static bool EnableWWDStyleId
    {
      get
      {
        return _enableWWDStyleId;
      }
      set
      { 
        _enableWWDStyleId = value;
      }
    }

    private int? _contextId;
    public override int ContextId
    {
      get
      {
        if (_contextId == null)
        {
          _contextId = ContextIds.Unknown;

          if (Manager.IsManager)
          {
            _contextId = Manager.ManagerContextId;
          }
          else
          {
            string sHost = HttpContext.Current.Request.Url.Host.ToLowerInvariant();
            IProxyContext proxy = RequestHelper.GetProxyContext();
            if ((proxy != null) && (proxy.IsLocalARR))
            {
              sHost = proxy.ARRHost.ToLowerInvariant();
            }

            if (sHost.Contains("godaddy.com") || sHost.Contains("godaddy-com.ide") || sHost.Contains("godaddymobile.com") || sHost.Contains("godaddymobile-com.ide"))
              _contextId = ContextIds.GoDaddy;
            else if (sHost.Contains("bluerazor.com") || sHost.Contains("bluerazor-com.ide"))
              _contextId = ContextIds.BlueRazor;
            else if (sHost.Contains("wildwestdomains.com") || sHost.Contains("wildwestdomains-com.ide"))
              _contextId = ContextIds.WildWestDomains;
            else if (sHost.Contains("securepaynet.net") || sHost.Contains("securepaynet-net.ide"))
              _contextId = ContextIds.Reseller;
          }
        }

        return _contextId.Value;
      }
    }

    private string _styleId;
    public override string StyleId
    {
      get
      {
        if (_styleId == null)
        {
          _styleId = "0";
          if (ContextId == ContextIds.GoDaddy)
            _styleId = "1";
          else if (ContextId == ContextIds.BlueRazor)
            _styleId = "2";
          else if (EnableWWDStyleId && ContextId == ContextIds.WildWestDomains)
            _styleId = "1387";
        }

        return _styleId;
      }
    }

    private const int WWD_PLID = 1387;
    private const string PARAM_PLID = "pl_id";
    private int? _privateLabelId;
    public override int PrivateLabelId
    {
      get
      {
        if (_privateLabelId == null)
        {
          _privateLabelId = 0;

          if (Manager.IsManager)
          {
            _privateLabelId = Manager.ManagerPrivateLabelId;
          }
          else
          {
            if (ContextId == ContextIds.GoDaddy)
              _privateLabelId = 1;
            else if (ContextId == ContextIds.BlueRazor)
              _privateLabelId = 2;
            else if (ContextId == ContextIds.WildWestDomains)
              _privateLabelId = WWD_PLID;
            else
            {
              if (ProgId.Length > 0)
              {
                _privateLabelId = DataCache.DataCache.GetPrivateLabelId(ProgId);
              }

              if (_privateLabelId == 0 && !string.IsNullOrEmpty(HttpContext.Current.Request[PARAM_PLID]))
              {
                int iPrivateLabelId;
                if (int.TryParse(HttpContext.Current.Request[PARAM_PLID], out iPrivateLabelId))
                  _privateLabelId = iPrivateLabelId;
              }
            }
          }
        }

        return _privateLabelId.Value;
      }
    }

    private const string PARAM_PROGID = "prog_id";
    private string _progId;
    public override string ProgId
    {
      get
      {
        if (_progId == null)
        {
          if ((ContextId == ContextIds.Reseller) && (!Manager.IsManager))
            _progId = HttpContext.Current.Request[PARAM_PROGID] ?? string.Empty;
          else
            _progId = DataCache.DataCache.GetProgID(PrivateLabelId);
        }

        return _progId;
      }
    }

    public PrivateLabelAwareSiteContextProvider(IProviderContainer providerContainer)
      : base(providerContainer)
    {
    }

  }
}
