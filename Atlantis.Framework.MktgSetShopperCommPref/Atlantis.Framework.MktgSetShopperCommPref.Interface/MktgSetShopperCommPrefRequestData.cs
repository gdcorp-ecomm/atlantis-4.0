using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MktgSetShopperCommPref.Interface
{
  public class MktgSetShopperCommPrefRequestData : RequestData
  {
    public int CommPreferenceTypeId { get; set; }
    public bool OptIn { get; set; }

    public MktgSetShopperCommPrefRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      int comPreferenceTypeId,bool optIn)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      CommPreferenceTypeId=comPreferenceTypeId;
      OptIn=optIn;
      RequestTimeout = TimeSpan.FromSeconds(6);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("MktgSetShopperCommPrefRequestData is not a cacheable request.");
    }
  }
}
