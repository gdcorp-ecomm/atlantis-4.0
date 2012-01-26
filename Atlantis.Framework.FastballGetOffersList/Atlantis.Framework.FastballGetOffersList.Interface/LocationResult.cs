using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.FastballGetOffersList.Interface
{
  public class LocationResult
  {
    public int ResultCode { get; set; }
    public string CandidateAttributeXml { get; set; }
    public string RequestUID { get; set; }
    public string Header1 { get; set; }
    public string Header2 { get; set; }
    public List<SubLocation> SubLocations { get; set; }
  }
}
