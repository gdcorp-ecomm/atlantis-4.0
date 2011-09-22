using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Auction.Interface
{
  public class AuctionStatusCode
  {
    public int StatusCodeId { get; set; }
    public string StatusType { get; set; }
    public string StatusDescription { get; set; }
    public string StatusGroup { get; set; }
    public bool IsListable { get; set; }
    public bool IsMemberArea { get; set; }
    public bool AllowDuplicateListings { get; set; }

    public AuctionStatusCode() { }
  }
}
