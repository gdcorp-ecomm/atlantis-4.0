using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BuyerProfileGetById.Interface
{
  public class BuyerProfileGetByIdRequestData : RequestData
  {
    private string _profileId;

    public BuyerProfileGetByIdRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string profileId)
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
      throw new Exception("GetBuyerProfileByID is not a cacheable request.");
    }
  }
}
