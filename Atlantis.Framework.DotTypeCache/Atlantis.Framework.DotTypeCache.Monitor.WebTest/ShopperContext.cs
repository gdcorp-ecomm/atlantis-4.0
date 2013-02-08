using System;
using System.Web;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeCache.Monitor.WebTest
{
  public class ShopperContext : ProviderBase, IShopperContext
  {
    public ShopperContext(IProviderContainer container) : base(container)
    {
    }

    public string ShopperId 
    {
      get { return "861126"; }
    }
    public ShopperStatusType ShopperStatus { get; private set; }
    public int ShopperPriceType { get; private set; }
    public void ClearShopper()
    {
      throw new NotImplementedException();
    }

    public bool SetLoggedInShopper(string shopperId)
    {
      throw new NotImplementedException();
    }

    public bool SetLoggedInShopperWithCookieOverride(string shopperId)
    {
      throw new NotImplementedException();
    }

    public void SetNewShopper(string shopperId)
    {
      throw new NotImplementedException();
    }
  }
}