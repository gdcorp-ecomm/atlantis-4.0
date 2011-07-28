using System;
using System.Web;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Split.Tests
{
  internal class TestContexts : ProviderBase, ISiteContext, IShopperContext
  {
    int _privateLabelId = 1;
    string _shopperId = string.Empty;

    public TestContexts(IProviderContainer container)
      : base(container)
    {
    }

    public void SetContextInfo(int privateLabelId, string shopperId)
    {
      _privateLabelId = privateLabelId;
      _shopperId = shopperId;
    }

    #region ISiteContext Members

    public string StyleId
    {
      get { return "0"; }
    }

    public System.Web.HttpCookie NewCrossDomainCookie(string cookieName, DateTime expiration)
    {
      throw new NotImplementedException();
    }

    public System.Web.HttpCookie NewCrossDomainMemCookie(string cookieName)
    {
      return new HttpCookie(cookieName);
    }

    public int PageCount
    {
      get { return 0; }
    }

    public string Pathway
    {
      get { return "UnitTest"; }
    }

    public string CI
    {
      get { return string.Empty; }
    }

    public string CommissionJunctionStartDate
    {
      get
      {
        return string.Empty;
      }
      set
      {
        return;
      }
    }

    public string ISC
    {
      get
      {
        string result = string.Empty;
        if (HttpContext.Current != null)
        {
          if (HttpContext.Current.Request.QueryString["ISC"] != null)
          {
            result = HttpContext.Current.Request.QueryString["ISC"];
          }
        }
        return result;
      }
    }

    public string CurrencyType
    {
      get
      {
        return "USD";
      }
    }

    public void SetCurrencyType(string currencyType)
    {
      return;
    }

    public bool IsRequestInternal
    {
      get
      {
        return true;
      }
    }

    public ServerLocationType ServerLocation
    {
      get { return ServerLocationType.Dev; }
    }


    #endregion

    public int ContextId
    {
      get
      {
        int result = 6;
        if (_privateLabelId == 2)
        {
          result = 5;
        }
        else if (_privateLabelId == 1)
        {
          result = 1;
        }
        else if (_privateLabelId == 1387)
        {
          result = 2;
        }
        return result;
      }
    }

    public int PrivateLabelId
    {
      get { return _privateLabelId; }
    }

    public string ProgId
    {
      get { throw new NotImplementedException(); }
    }

    public IManagerContext Manager
    {
      get { throw new NotImplementedException(); }
    }

    public string ShopperId
    {
      get { return _shopperId; }
    }

    public ShopperStatusType ShopperStatus
    {
      get { throw new NotImplementedException(); }
    }

    public int ShopperPriceType
    {
      get { throw new NotImplementedException(); }
    }

    public void ClearShopper()
    {
      _shopperId = string.Empty;
    }

    public void SetNewShopper(string shopperId)
    {
      _shopperId = shopperId;
    }

    public bool SetLoggedInShopperWithCookieOverride(string shopperId)
    {
      throw new NotImplementedException();
    }

    public bool SetLoggedInShopper(string shopperId)
    {
      _shopperId = shopperId;
      return true;
    }
  }
}
