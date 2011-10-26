using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MYAResellerUpgrades.Interface
{
  public class MYAResellerUpgradesRequestData : RequestData
  {
    #region Properties

    private int _billingResourceId;
    private TimeSpan _requestTimeout = new TimeSpan(0, 0, 2);

    public int BillingResourceId
    {
      get { return _billingResourceId; }
    }

    public TimeSpan RequestTimeout
    {
      get { return _requestTimeout; }
      set { _requestTimeout = value; }
    }
    #endregion
    
    public MYAResellerUpgradesRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderIo,
                                  string pathway,
                                  int pageCount,
                                  int billingResourceId)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      _billingResourceId = billingResourceId;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in MYAResellerUpgradesRequestData");     
    }
  }
}
