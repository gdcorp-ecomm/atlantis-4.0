using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommHasLineOfCredit.Interface
{
  public class EcommHasLineOfCreditRequestData: RequestData
  {
    public EcommHasLineOfCreditRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {       
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
