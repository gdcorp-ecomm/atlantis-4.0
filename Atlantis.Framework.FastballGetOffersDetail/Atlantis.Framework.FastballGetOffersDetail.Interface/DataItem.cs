using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.FastballGetOffersDetail.Interface
{
  public class DataItem
  {
    public string Id { get; set; }
    public string Type { get; set; }
    public Dictionary<string, List<string>> Attributes { get; set; }
  }
}
