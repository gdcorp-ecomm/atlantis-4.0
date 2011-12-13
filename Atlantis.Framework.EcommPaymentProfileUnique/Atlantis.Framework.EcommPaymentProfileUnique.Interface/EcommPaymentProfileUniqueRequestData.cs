using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPaymentProfileUnique.Interface
{
  public class EcommPaymentProfileUniqueRequestData : RequestData
  {
    
    public EcommPaymentProfileUniqueRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
        RequestTimeout = TimeSpan.FromSeconds(3);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in EcommPaymentProfileUniqueRequestData");     
    }
  }
}
