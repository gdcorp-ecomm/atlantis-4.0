using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class Meta : IWidgetModel
  {
    public Meta()
    {
    }
    public string Title { get; set; }
    public string Canonical { get; set; }
    public string ShortCutIcon { get; set; }
    public string Description { get; set; }
    public string Keywords { get; set; }
    public bool Robots { get; set; }
  }
}
