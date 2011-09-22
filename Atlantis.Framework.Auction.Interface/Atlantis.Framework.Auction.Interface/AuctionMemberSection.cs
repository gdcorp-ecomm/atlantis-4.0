using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Auction.Interface
{

  [DataContract(Name = "Section"), Serializable]
  public class AuctionMemberSection
  {
    [DataMember(Name = "SectionName")]
    public string SectionName { get; set; }

    private List<Auction> _auctions;

    [DataMember(Name = "Auctions")]
    public List<Auction> Auctions
    {
      get
      {
        if (_auctions == null)
        {
          _auctions = new List<Auction>();
        }
        return _auctions;
      }
      set { _auctions = value; }
    }
  }
}

