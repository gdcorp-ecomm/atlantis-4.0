using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BuyerProfileDelete.Interface
{
  public class BuyerProfileDeleteRequestData : RequestData
  {
    private string _profileId;

    public BuyerProfileDeleteRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string profileId)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      _profileId = profileId;
    }

    public string ProfileID
    {
      get
      {
        return _profileId;
      }
    } 


    public override string GetCacheMD5()
    {
      throw new Exception("BuyerProfileDelete is not a cacheable request.");
    }
  }
}
