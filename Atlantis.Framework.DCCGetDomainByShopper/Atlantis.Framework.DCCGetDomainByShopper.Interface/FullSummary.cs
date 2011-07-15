using System.Runtime.Serialization;

namespace Atlantis.Framework.DCCGetDomainByShopper.Interface
{
  [DataContract]
  public class FullSummary
  {
    private const string DELIM = "|";

    [DataMember]
    public int ResultCount { get; set; }

    [DataMember]
    public string FirstDomain { get; set; }

    [DataMember]
    public string LastDomain { get; set; }

    public FullSummary()
    {
    }

    public override string ToString()
    {
      return string.Concat(ResultCount, DELIM, FirstDomain, DELIM, LastDomain);
    }
  }
}
