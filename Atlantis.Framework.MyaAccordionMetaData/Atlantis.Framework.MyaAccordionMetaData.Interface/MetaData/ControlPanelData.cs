using System;
using System.Collections.Generic;

namespace Atlantis.Framework.MyaAccordionMetaData.Interface.MetaData
{
  internal class ControlPanelData
  {
    #region ReadOnly Property
    private readonly List<LinkUrlData> _linkUrls;
    public List<LinkUrlData> LinkUrls
    {
      get { return _linkUrls; }
    }
    #endregion

    #region Convenience Accessor Properties & Methods
    public bool HasManagerLink(string identificationValue)
    {
      return LinkUrls.Exists(new Predicate<LinkUrlData>(url => url.Type.Equals(LinkUrlData.TypeOfLink.Manager) && url.IdentificationValue.Equals(identificationValue)));
    }

    public bool DoLinkUrlsContainIdentificationRules
    {
      get { return LinkUrls.Exists(new Predicate<LinkUrlData>(url => !string.IsNullOrWhiteSpace(url.IdentificationRule))); }
    }
    #endregion

    public ControlPanelData(List<LinkUrlData> linkUrls)
    {
      _linkUrls = linkUrls;
    }
  }
}
