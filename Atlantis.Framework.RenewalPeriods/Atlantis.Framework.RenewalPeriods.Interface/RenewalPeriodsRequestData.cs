using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RenewalPeriods.Interface
{
  public class RenewalPeriodsRequestData : RequestData
  {

    private List<int> _resourceIds = new List<int>();
    public List<int> ResourceIds
    {
      get
      {
        return _resourceIds;
      }
    }
    
    public RenewalPeriodsRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  List<int> resourceIds)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _resourceIds = resourceIds;
    }

    public override string GetCacheMD5()
    {
      throw new InvalidOperationException("Not a cacheable request");
    }


  }
}
