using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.DPP.Widgets
{
    public class DPPSearchHeader : IWidgetModel
  {
    public DPPSearchHeader()
    {
    }

    public string HeaderText { get; set; }
    public string CSS { get; set; }
    public string JavaScript { get; set; }
    public string Markup { get; set; }
    public string DPPUserControl { get; set; }
    public string QueryStringParameters { get; set; }
  }
}
