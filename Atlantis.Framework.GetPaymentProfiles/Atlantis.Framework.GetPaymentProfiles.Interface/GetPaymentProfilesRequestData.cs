using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetPaymentProfiles.Interface
{
  public class GetPaymentProfilesRequestData : RequestData
  {
    public GetPaymentProfilesRequestData(string shopperID,
                            string sourceURL,
                            string orderID,
                            string pathway,
                            int pageCount)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {

    }

    public override string GetCacheMD5()
    {
      throw new Exception("GetPaymentProfiles is not a cacheable request.");
    }
  }
}
