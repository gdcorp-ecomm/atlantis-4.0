using System.Runtime.Serialization;

namespace Atlantis.Framework.AuctionSearch.Interface
{
  [DataContract]
  public class DomainAgeFilter
  {
    private int _minDomainAgeYears = -1;
    [DataMember]
    public int MinDomainAgeYears
    {
      get { return _minDomainAgeYears; }
      set { _minDomainAgeYears = value; }
    }

    private int _maxDomainAgeYears = -1;
    [DataMember]
    public int MaxDomainAgeYears
    {
      get { return _maxDomainAgeYears; }
      set { _maxDomainAgeYears = value; }
    }
  }
}
