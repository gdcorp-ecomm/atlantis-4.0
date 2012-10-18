using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Mobile
{
  class MobileDomainSearchWidget : IWidgetModel
  {
    public MobileDomainSearchWidget()
    {
      HideBulkDomainSearch = false;
      SectionGrooveTop = false;
      ExtraPadding = false;
      IscCode = string.Empty;
      SearchActionName = string.Empty;
      RegistrationLength = 1;
    }

    public bool HideBulkDomainSearch { get; set; }
    public bool SectionGrooveTop { get; set; }
    public bool ExtraPadding { get; set; }
    public string IscCode { get; set; }
    public string SearchActionName { get; set; }
    public int RegistrationLength { get; set; }
  }
}
