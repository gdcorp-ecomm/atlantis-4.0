using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OrderShippingXml.Interface
{
  public class OrderShippingXmlRequestData : RequestData
  {
    #region Properties

    public string RecentOrderId { get; set; }

    #endregion
    public OrderShippingXmlRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount, string recentOrderId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RecentOrderId = recentOrderId;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }

}
