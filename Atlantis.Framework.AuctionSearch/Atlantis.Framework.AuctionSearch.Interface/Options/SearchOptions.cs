using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.AuctionSearch.Interface
{
  [DataContract]
  public class SearchOptions
  {
    [DataMember]
    public string PreDefinedSearchType { get; set; }

    [DataMember]
    public KeywordFilter Keywords { get; set; }

    [DataMember]
    public BidFilter Bid { get; set; }

    [DataMember]
    public CharacterFilter Character { get; set; }

    [DataMember]
    public PriceFilter Price { get; set; }

    [DataMember]
    public TrafficFilter Traffic { get; set; }

    [DataMember]
    public DomainAgeFilter DomainAge { get; set; }

    [DataMember]
    public int? DaysEndingIn { get; set; }

    private IList<string> _auctionIdList = new List<string>(32);
    [DataMember]
    public IList<string> AuctionIdList
    {
      get { return _auctionIdList; }
      set { _auctionIdList = value; }
    }

    private IList<string> _auctionTypeList = new List<string>(16);
    [DataMember]
    public IList<string> AuctionTypeList
    {
      get { return _auctionTypeList; }
      set { _auctionTypeList = value; }
    }

    private IList<string> _tldFilter = new List<string>(32);
    [DataMember]
    public IList<string> TldFilter
    {
      get { return _tldFilter; }
      set { _tldFilter = value; }
    }

    [DataMember]
    public PatternFilter PatternFilter { get; set; }

    [DataMember]
    public bool? HasDigits { get; set; }

    [DataMember]
    public bool? HasDashes { get; set; }

    [DataMember]
    public bool? BuyNow { get; set; }

    [DataMember]
    public bool? WebSite { get; set; }

    [DataMember]
    public bool? Featured { get; set; }

    [DataMember]
    public bool? Appraisal { get; set; }

    [DataMember]
    public bool? InviteOnly { get; set; }

    [DataMember]
    public bool? OnSale { get; set; }

    [DataMember]
    public string Category { get; set; }
  }
}
