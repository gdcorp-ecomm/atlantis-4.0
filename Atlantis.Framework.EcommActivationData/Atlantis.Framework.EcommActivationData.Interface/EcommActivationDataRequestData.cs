using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommActivationData.Interface
{
  public class EcommActivationDataRequestData : RequestData
  {

    public EcommActivationDataRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    { }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("Do not Implement Caching on Activation Data");
    }


  }
}
