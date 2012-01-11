using System;
using System.Collections.Generic;

namespace Atlantis.Framework.MyaAccordionMetaData.Interface.MetaData
{
  public class ControlPanelData
  {
    #region ReadOnly Property
    private readonly List<LinkUrlData> _linkUrls;
    #if DEBUG
    public List<LinkUrlData> LinkUrls
    {
      get { return _linkUrls; }
    }
    #endif
    #endregion

    #region Convenience Accessor Properties & Methods

    public bool HasManagerLink(string identificationValue)
    {
      return _linkUrls.Exists(url => url.Type.Equals(LinkUrlData.TypeOfLink.Manager) && url.IdentificationValue.Equals(identificationValue));
    }

    public bool DoLinkUrlsContainIdentificationRules
    {
      get { return _linkUrls.Exists(url => !string.IsNullOrEmpty(url.IdentificationRule)); }
    }

    public LinkUrlData FindLinkUrl(Predicate<LinkUrlData> p)
    {
      return _linkUrls.Find(p);
    }

    public LinkUrlData FirstLinkUrl()
    {
      return _linkUrls[0];
    }
    #endregion

    internal ControlPanelData(List<LinkUrlData> linkUrls)
    {
      _linkUrls = linkUrls;
    }
  }
}
