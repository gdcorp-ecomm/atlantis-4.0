using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class Literal : IWidgetModel
  {
    public string Content { get; set; }
    public string Styles { get; set; }
  }
}
