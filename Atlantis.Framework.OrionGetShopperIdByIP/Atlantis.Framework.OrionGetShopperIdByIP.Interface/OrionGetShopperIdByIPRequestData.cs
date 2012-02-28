using System;
using Atlantis.Framework.Interface;


namespace Atlantis.Framework.OrionGetShopperIdByIP.Interface
{
  public class OrionGetShopperIdByIPRequestData : RequestData
  {
    public string IpToSearch { get; set; }

    public OrionGetShopperIdByIPRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string ipToSearch) :
      base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      IpToSearch = ipToSearch;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
