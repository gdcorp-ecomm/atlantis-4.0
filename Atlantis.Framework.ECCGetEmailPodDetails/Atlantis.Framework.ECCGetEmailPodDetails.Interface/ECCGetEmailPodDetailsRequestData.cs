using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetEmailPodDetails.Interface
{
  public class ECCGetEmailPodDetailsRequestData : RequestData
  {
    public string EmailAddress { get; set; }

    public ECCGetEmailPodDetailsRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string emailAddresss) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      EmailAddress = emailAddresss;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("ECCGetEmailPodDetails is not a cacheable request.");
    }
  }
}
