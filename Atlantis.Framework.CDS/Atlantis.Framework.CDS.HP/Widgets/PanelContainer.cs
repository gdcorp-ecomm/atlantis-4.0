using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CDS.HP.Widgets
{
  public class PanelContainer
  {
    public IList<Panel> PanelList { get; set; }
    public PanelContainer()
    {
      PanelList = new List<Panel>();
    }
  }

  public class Panel
  {
    public string PanelId { get; set; }
    public string PanelZoneId { get; set; }
    public bool IsDefault { get; set; }
    public string OnLoadQuerystring { get; set; }
  }
}
