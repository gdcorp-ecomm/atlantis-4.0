using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.RegVendorDomainSearch.Interface
{
  public class DPPDomainNameMatch
  {
    public RegVendorDomainSearchVendor VendorId { get; set; }
    public int AuctionId { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int CommissionPct { get; set; }
    public string AuctionType { get; set; }
    public DateTime AuctionEndTime { get; set; }

    public DPPDomainNameMatch()
    {
    }

    public DPPDomainNameMatch(RegVendorDomainSearchVendor vendorId, int id, string name, 
      int price, int pct, string auctionType, string auctionEndTime)
    {
      VendorId = vendorId;
      AuctionId = id;
      Name = name;
      Price = price;
      CommissionPct = pct;
      AuctionType = auctionType;
      DateTime endTime = DateTime.Now;
      DateTime.TryParse(auctionEndTime, out endTime);
      AuctionEndTime = endTime;
    }
  }

  public class RegVendorDomainSearchResult
  {
    public string Server { get; set; }
    public string DateTime { get; set; }

    public DPPDomainNameMatch ExactMatch { get; private set; }

    private List<DPPDomainNameMatch> _premiumMatch = new List<DPPDomainNameMatch>();
    public List<DPPDomainNameMatch> PremiumMatch
    {
      get
      {
        return _premiumMatch;
      }
    }

    public void AddExactMatch(RegVendorDomainSearchVendor vendorId, int id, string name, int price, 
      int pct, string auctionType, string auctionEndTime)
    {
      ExactMatch = new DPPDomainNameMatch(vendorId, id, name, price, pct, auctionType, auctionEndTime);
    }

    public void AddPremiumMatch(RegVendorDomainSearchVendor vendorId, int id, string name, 
      int price, int pct, string auctionType, string auctionEndTime)
    {
      PremiumMatch.Add(new DPPDomainNameMatch(vendorId, id, name, price, pct, auctionType, auctionEndTime));
    }

    public void AddExactMatch(DPPDomainNameMatch match)
    {
      ExactMatch = match;
    }

    public void AddPremiumMatch(DPPDomainNameMatch match)
    {
      PremiumMatch.Add(match);
    }

  }
}
