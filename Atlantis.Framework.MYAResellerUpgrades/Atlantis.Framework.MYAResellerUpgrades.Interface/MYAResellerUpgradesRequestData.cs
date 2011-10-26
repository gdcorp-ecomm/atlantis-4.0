using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MYAResellerUpgrades.Interface
{
  public class MYAResellerUpgradesRequestData : RequestData
  {
    #region Properties

    public int BillingResourceId { get; set; }
   
    #endregion
    
    public MYAResellerUpgradesRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderIo,
                                  string pathway,
                                  int pageCount,
                                  int billingResourceId)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5d);
      BillingResourceId = billingResourceId;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in MYAResellerUpgradesRequestData");     
    }
  }
}
