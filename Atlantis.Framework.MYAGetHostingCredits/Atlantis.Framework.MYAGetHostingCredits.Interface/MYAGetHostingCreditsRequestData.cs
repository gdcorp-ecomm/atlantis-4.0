using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MYAGetHostingCredits.Interface
{
  public class MYAGetHostingCreditsRequestData : RequestData
  {
    public List<int> ProductTypeIds { get; set; }

    public MYAGetHostingCreditsRequestData(string shopperId,
                                           string sourceUrl,
                                           string orderIo,
                                           string pathway,
                                           int pageCount,
                                           List<int> productTypeIds)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      ProductTypeIds = productTypeIds;
      RequestTimeout = TimeSpan.FromSeconds(2d);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in MYAGetHostingCreditsRequestData");     
    }
  }
}
