using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BuyerProfileSetDefault.Interface
{
  public class BuyerProfileSetDefaultRequestData : RequestData
  {
    private string _profileId;
    private int _defaultFlag = 0;

    public BuyerProfileSetDefaultRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string profileId, bool isdefault)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      _profileId = profileId;
      _defaultFlag = Convert.ToInt32(isdefault);
    }

    public string ProfileID
    {
      get
      {
        return _profileId;
      }
    }

    public int DefaultFlag
    {
      get
      {
        return _defaultFlag;
      }
    } 

    public override string GetCacheMD5()
    {
      throw new Exception("BuyerProfileSetDefault is not a cacheable request.");
    }
  }
}
