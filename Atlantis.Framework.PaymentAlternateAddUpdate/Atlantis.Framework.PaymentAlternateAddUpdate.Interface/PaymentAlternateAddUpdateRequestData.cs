using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PaymentAlternateAddUpdate.Interface
{
  public class PaymentAlternateAddUpdateRequestData : RequestData
  {
    public int PaymentProfileId
    {
      get;
      set;
    }

    public PaymentAlternateAddUpdateRequestData(string shopperId,
                                            string sourceURL,
                                            string orderId,
                                            string pathway,
                                            int pageCount, int profileId)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      PaymentProfileId = profileId;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in PaymentAlternateAddUpdateRequestData");
    }
  }
}
