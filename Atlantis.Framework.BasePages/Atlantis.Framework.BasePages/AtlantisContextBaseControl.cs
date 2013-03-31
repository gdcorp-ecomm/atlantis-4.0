﻿using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using System.Web.UI;

namespace Atlantis.Framework.BasePages
{
  public abstract class AtlantisContextBaseControl : UserControl
  {
    private ISiteContext _siteContext;
    protected virtual ISiteContext SiteContext
    {
      get
      {
        if (_siteContext == null)
        {
          _siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
        }
        return _siteContext;
      }
    }

    private IShopperContext _shopperContext;
    protected virtual IShopperContext ShopperContext
    {
      get
      {
        if (_shopperContext == null)
        {
          _shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
        }
        return _shopperContext;
      }
    }
  }
}
