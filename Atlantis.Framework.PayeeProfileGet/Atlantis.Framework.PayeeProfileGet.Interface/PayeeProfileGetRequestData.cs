using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PayeeProfileGet.Interface
{
  public class PayeeProfileGetRequestData : RequestData
  {
    public int CapId { get; private set; }

    public PayeeProfileGetRequestData(string shopperID,
                            string sourceURL,
                            string orderID,
                            string pathway,
                            int pageCount, int capId)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      CapId = capId;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public override string GetCacheMD5()
    {
      throw new Exception("PayeeProfileGet is not a cacheable request.");
    }

  }
}
