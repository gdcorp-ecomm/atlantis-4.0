using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Mobile.Widgets
{
  public class MobileDomainSearchWidget : IWidgetModel
  {
    public bool HideBulkDomainSearch { get; set; }
    public bool SectionGrooveTop { get; set; }
    public bool ExtraPadding { get; set; }
    public string IscCode { get; set; }
    public string SearchActionName { get; set; }
    public int RegistrationLength { get; set; }
  }
}