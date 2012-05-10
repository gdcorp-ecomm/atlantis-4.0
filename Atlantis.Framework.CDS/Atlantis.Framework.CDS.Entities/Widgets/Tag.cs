using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class Tag
  {
    public Tag()
    {
      Attributes = new Dictionary<string, string>();
    }
    public Dictionary<string, string> Attributes { get; set; }
    public string Name { get; set; }
  }
}
