using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PaymentProfileLastChargeDate.Interface
{
  public class PaymentProfileLastChargeDateRequestData : RequestData
  {

    public int ProfileId { get; set; }
    
    public PaymentProfileLastChargeDateRequestData(string shopperId, 
                                            string sourceURL, 
                                            string orderId, 
                                            string pathway, 
                                            int pageCount, int profileId) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      ProfileId = profileId;
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("PaymentProfileLastChargeDateRequestData is not a cacheable request.");
    }

    #endregion
  }
}
