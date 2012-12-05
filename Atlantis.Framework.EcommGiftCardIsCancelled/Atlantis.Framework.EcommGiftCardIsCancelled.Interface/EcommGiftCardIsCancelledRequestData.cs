using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommGiftCardIsCancelled.Interface
{
  public class EcommGiftCardIsCancelledRequestData : RequestData
  {
    public int ResourceId { get; set; }
    
    public EcommGiftCardIsCancelledRequestData(string shopperId,
                  string sourceUrl,
                  string orderId,
                  string pathway,
                  int pageCount,
                  int resourceId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ResourceId = resourceId;
    }

    public override string ToXML()
    {
      return string.Empty;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
