using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.FastballGetOffersDetail.Interface
{
  public class PackageDetailLite
  {
    public string Id { get; set; }
    public string Type { get; set; }
    public DateTime TargetDate { get; set; }
    public DateTime EndDate { get; set; }

    public bool IsActive
    {
      get { return DateTime.Now >= TargetDate && DateTime.Now <= EndDate; }
    }
  }
}
