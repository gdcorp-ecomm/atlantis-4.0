using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Homepage.Widgets
{
  public class HPOpenGraph : IWidgetModel
  {
    public string Title { get; set; }
    public string SiteType { get; set; }
    public string Url { get; set; }
    public string Image { get; set; }
    public string Description { get; set; }
  }
}
