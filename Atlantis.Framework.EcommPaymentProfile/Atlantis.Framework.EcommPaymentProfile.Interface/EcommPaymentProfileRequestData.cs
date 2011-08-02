using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPaymentProfile.Interface
{
  public class EcommPaymentProfileRequestData : RequestData
  {
    public int ProfileId { get; set; }

    public EcommPaymentProfileRequestData(string shopperId,
                            string sourceUrl,
                            string orderId,
                            string pathway,
                            int pageCount, int profileId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ProfileId = profileId;
      RequestTimeout = TimeSpan.FromSeconds(2d);
    }

    public override string GetCacheMD5()
    {
      throw new Exception("EcommPaymentProfile is not a cacheable request.");
    }
  }
}
