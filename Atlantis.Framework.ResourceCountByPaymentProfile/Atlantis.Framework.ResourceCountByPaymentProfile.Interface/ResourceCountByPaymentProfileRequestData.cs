using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ResourceCountByPaymentProfile.Interface
{
  public class ResourceCountByPaymentProfileRequestData : RequestData
  {

    public int ProfileId { get; set; }

    public ResourceCountByPaymentProfileRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount, int profileId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ProfileId = profileId;
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("ResourceCountByPaymentProfileRequestData is not a cachable request.");
    }

    #endregion


  }
}
