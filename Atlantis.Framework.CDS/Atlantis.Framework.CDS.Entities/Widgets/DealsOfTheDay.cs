using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class DealsOfTheDay : IWidgetModel
  {
    public string BorderColor { get; set; }
    public string BackgroundColor { get; set; }
    public string Title { get; set; }
    public string MoreLinkText { get; set; }
    public string MoreLink { get; set; }
  }
}
