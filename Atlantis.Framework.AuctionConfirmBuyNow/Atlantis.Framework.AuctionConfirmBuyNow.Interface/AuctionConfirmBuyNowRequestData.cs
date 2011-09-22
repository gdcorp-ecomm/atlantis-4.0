using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionConfirmBuyNow.Interface
{
  public class AuctionConfirmBuyNowRequestData : RequestData
  {
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    public string AuctionItemId { get; set; }

    public string Comments { get; set; }

    public int SourceSystemId { get; set; }

    public string Isc { get; set; }

    public string Itc { get; set; }

    public AuctionConfirmBuyNowRequestData(string shopperId, 
                                           string sourceURL, 
                                           string orderId, 
                                           string pathway, 
                                           int pageCount,
                                           string auctionItemId,
                                           string comments,
                                           int sourceSystemId,
                                           string isc,
                                           string itc) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AuctionItemId = auctionItemId;
      Comments = comments;
      SourceSystemId = sourceSystemId;
      Isc = isc;
      Itc = itc;
      RequestTimeout = _requestTimeout;
    }


    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("AuctionConfirmBuyNow is not a cacheable request.");
    }

    #endregion
  }
}
