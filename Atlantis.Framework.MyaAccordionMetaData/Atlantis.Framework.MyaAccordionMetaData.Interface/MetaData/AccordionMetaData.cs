using System;
using System.Collections.Generic;
using Atlantis.Framework.BasePages;
using Atlantis.Framework.MyaAccordionMetaData.Interface.MetaData;

namespace Atlantis.Framework.MyaAccordionMetaData.Interface
{
  public class AccordionMetaData : IComparable<AccordionMetaData>
  {
 
    #region Stored Database Properties
    private readonly int _accordionId;
    public int AccordionId
    {
      get { return _accordionId; }
    }
    private readonly string _accordionTitleDefault;
    private readonly string _accordionTitleGoDaddy;
    public string GetAccordionTitle(int contextId)
    {
      if (contextId == ContextIds.GoDaddy && !string.IsNullOrEmpty(_accordionTitleGoDaddy))
      {
        return _accordionTitleGoDaddy;
      }
      return _accordionTitleDefault;
    }
    private readonly int _defaultSortOrder;
    public int DefaultSortOrder
    {
      get { return _defaultSortOrder; }
    }
    private readonly HashSet<string> _namespaces;
    public HashSet<string> Namespaces
    {
      get { return _namespaces; }
    }

    #endregion

    #region Derived Accordion Xml Properties

    private readonly string _ciExpansion;
    public string CiExpansion
    {
      get { return _ciExpansion; }
    }
    private readonly string _ciRenewNow;
    public string CiRenewNow
    {
      get { return _ciRenewNow; }
    }
    private readonly string _ciSetup;
    public string CiSetup
    {
      get { return _ciSetup; }
    }
    private readonly List<int> _cmsDisplayGroups;
    public List<int> CmsDisplayGroups
    {
      get { return _cmsDisplayGroups; }
    }
    private readonly CssSpriteCoordinate _iconCssCoordinates;
    public CssSpriteCoordinate IconCssCoordinates
    {
      get { return _iconCssCoordinates; }
    }
    private readonly int _productGroup;
    public int ProductGroup
    {
      get { return _productGroup; }
    }
    private HashSet<int> _productTypes;
    public HashSet<int> ProductTypes
    {
      get { return _productTypes; }
    }
    private readonly bool _showControlPanel;
    public bool ShowControlPanel
    {
      get { return _showControlPanel; }
    }
    private readonly bool _showSetupForManagerOnly;
    public bool ShowSetupForManagerOnly
    {
      get { return _showSetupForManagerOnly; }
    }
    private readonly string _orionProductName;
    public string OrionProductName
    {
      get { return _orionProductName; }
    }
    private readonly bool _isBundleProduct;
    public bool IsBundleProduct
    {
      get { return _isBundleProduct; }
    }

    #endregion

    #region Derived Content, Control Panel & WorkspaceLogin Accessors

    private readonly ContentData _content;
    public ContentData Content
    {
      get { return _content; }
    }

    private readonly ControlPanelData _controlPanels;
    public ControlPanelData ControlPanels
    {
      get { return _controlPanels; }
    }

    private readonly WorkspaceLoginData _workspaceLogin;
    public WorkspaceLoginData WorkspaceLogin
    {
      get { return _workspaceLogin; }
    }

    #endregion

    internal AccordionMetaData(int accordionId, string accordionTitleDefault, string accordionTitleGoDaddy, int defaultSortOrder, HashSet<string> namespaces, 
          string ciExpansion, string ciRenewNow, string ciSetup, List<int> cmsDisplayGroups, CssSpriteCoordinate iconCssCoordinates, 
          int productGroup, HashSet<int> productTypes, bool showControlPanel, bool showSetupForManagerOnly, string orionProductName, bool isBundleProduct,
          ContentData contentData, ControlPanelData controlPanelData, WorkspaceLoginData workspaceLoginData)
    {
      _accordionId = accordionId;
      _accordionTitleDefault = accordionTitleDefault;
      _accordionTitleGoDaddy = accordionTitleGoDaddy;
      _defaultSortOrder = defaultSortOrder;
      _namespaces = namespaces;
      _ciExpansion = ciExpansion;
      _ciRenewNow = ciRenewNow;
      _ciSetup = ciSetup;
      _cmsDisplayGroups = cmsDisplayGroups;
      _iconCssCoordinates = iconCssCoordinates;
      _productGroup = productGroup;
      _productTypes = productTypes;
      _showControlPanel = showControlPanel;
      _showSetupForManagerOnly = showSetupForManagerOnly;
      _orionProductName = orionProductName;
      _isBundleProduct = isBundleProduct;
      _content = contentData;
      _controlPanels = controlPanelData;
      _workspaceLogin = workspaceLoginData;
    }

    #region Public Methods

    public bool HasProductList()
    {
      return (AccordionId == 1 || !string.IsNullOrEmpty(Content.AccountList));
    }

    public bool ShowWorkspaceLogin()
    {
      return !string.IsNullOrEmpty(WorkspaceLogin.ButtonText);
    }

    #endregion

    public int CompareTo(AccordionMetaData other)
    {
      if (other == null)
      {
        return -1;
      }
      else
      {
        return this.DefaultSortOrder.CompareTo(other.DefaultSortOrder);
      }
    }
  }
}
