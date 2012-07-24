using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.GDTV.Widgets
{
  public class FeaturedProduct : IWidgetModel
  {
    public string PodSize { get; set; }
    public string PodTitle { get; set; }
    public string PodDescription { get; set; }
    public int PodProductId { get; set; }
    public string PodLinkUrl { get; set; }
    public string PodLinkText { get; set; }
    public string PodBackgroundImage { get; set; }
  }
}
