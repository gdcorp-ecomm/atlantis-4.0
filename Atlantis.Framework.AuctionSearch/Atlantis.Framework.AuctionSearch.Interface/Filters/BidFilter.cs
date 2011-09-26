using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Atlantis.Framework.AuctionSearch.Interface
{
  [DataContract]
  public class BidFilter
  {
    [DataMember]
    public string FilterType { get; private set; }

    [DataMember]
    public int BidCount { get; private set; }

    internal BidFilter(XElement bidsElement)
    {
      int bids;
      string bidsValue = bidsElement.Value;
      if (!string.IsNullOrEmpty(bidsValue) && int.TryParse(bidsValue, out bids))
      {
        XAttribute conditionAttribute = bidsElement.Attribute("conditioner");
        string filterValue = conditionAttribute != null ? conditionAttribute.Value : string.Empty;
        if (string.IsNullOrEmpty(filterValue))
        {
          filterValue = NumericFilterType.GreaterThan;
        }

        FilterType = filterValue;
        BidCount = bids;
      }
    }

    /// <summary>
    /// DO NOT use this contructor, meant for serialization only
    /// </summary>
    public BidFilter()
    {
    }

    public BidFilter(string filterType, int bidCount)
    {
      FilterType = filterType;
      BidCount = bidCount;
    }

    internal XElement GetElement()
    {
      XElement bidsElement = new XElement("Bids");
      bidsElement.SetAttributeValue("conditioner", FilterType);
      bidsElement.SetValue(BidCount);

      return bidsElement;
    }
  }
}
