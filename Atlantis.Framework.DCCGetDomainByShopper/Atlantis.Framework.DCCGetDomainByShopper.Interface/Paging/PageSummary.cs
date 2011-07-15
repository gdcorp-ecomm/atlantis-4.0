using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.DCCGetDomainByShopper.Interface.Paging
{
  [DataContract]
  public class PageSummary
  {
    private const string DELIM = " | ";

    [DataMember]
    public string FirstDomainName { get; set; }

    [DataMember]
    public DateTime FirstExpirationDate { get; set; }

    [DataMember]
    public string LastDomainName { get; set; }
    
    [DataMember]
    public DateTime LastExpirationDate { get; set; }

    public PageSummary()
    {
    }

    public override string ToString()
    {
      return string.Concat(FirstDomainName, DELIM, FirstExpirationDate, DELIM, LastDomainName, DELIM, LastExpirationDate);
    }
  }
}
