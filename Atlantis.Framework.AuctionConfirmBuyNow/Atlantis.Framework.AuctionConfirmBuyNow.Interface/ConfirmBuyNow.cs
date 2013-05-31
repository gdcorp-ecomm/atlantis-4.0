using System.IO;
using System.Text;
using System.Xml;

namespace Atlantis.Framework.AuctionConfirmBuyNow.Interface
{
  internal class ConfirmBuyNow
  {
    public string AuctionId { get; set; }
    public string DomainName { get; set; }
    public string BidderShopperId { get; set; }
    public string Comments { get; set; }
    public string Isc { get; set; }
    public string Itc { get; set; }

    public ConfirmBuyNow()
    {
    }

    public ConfirmBuyNow(string auctionId,
                         string domainName,
                         string bidderShopperId,
                         string comments,
                         string isc,
                         string itc)
    {
      AuctionId = auctionId;
      DomainName = domainName;
      BidderShopperId = bidderShopperId;
      Comments = comments;
      Isc = isc;
      Itc = itc;
    }
  }
}
