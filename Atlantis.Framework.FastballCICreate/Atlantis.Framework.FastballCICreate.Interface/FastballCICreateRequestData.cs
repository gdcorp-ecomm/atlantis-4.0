using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballCICreate.Interface
{
  public class FastballCICreateRequestData : RequestData
  {
    public string CICodeXML { get; set; }

    public FastballCICreateRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string ciCodeXML)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ShopperID = shopperId;
      RequestTimeout = TimeSpan.FromSeconds(6);
      CICodeXML = ciCodeXML;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
