using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MarketingPublication.Interface
{
  public class MktgSetShopperInterestPrefRequestData : RequestData
  {
    public int CommPreferenceTypeId { get; set; }
    public int InterestTypeId { get; set; }
    public bool OptIn { get; set; }

    // Sample constructor
    // Do not use the base constructor any more, create a constructor like this that requires only what you need
    public MktgSetShopperInterestPrefRequestData(string shopperId, int intTypeId, int comPreferenceTypeId, bool optIn)
    {
      // store input data into member properties
      // that will be accessible by the IRequest handler
      ShopperID = shopperId;
      InterestTypeId = intTypeId;
      CommPreferenceTypeId = comPreferenceTypeId;
      OptIn = optIn;
      RequestTimeout = TimeSpan.FromSeconds(6);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("MktgSetShopperInterestPrefRequestData is not a cacheable request.");
    }
  }
}
