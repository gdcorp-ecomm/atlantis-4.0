using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.GDTV.Widgets
{
  public class VideoMeta : IWidgetModel
  {
    public string Title { get; set; }
    public string Canonical { get; set; }
    public string ShortCutIcon { get; set; }
    public string Description { get; set; }
    public string Keywords { get; set; }
    public bool Robots { get; set; }
  }
}
