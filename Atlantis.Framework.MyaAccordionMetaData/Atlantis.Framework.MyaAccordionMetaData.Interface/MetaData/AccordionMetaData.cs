using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaAccordionMetaData.Interface.MetaData;

namespace Atlantis.Framework.MyaAccordionMetaData.Interface
{
  public class AccordionMetaData : IComparable<AccordionMetaData>
  {
    #region Private XML Validation Properties & Methods

    #region Accordion XML
    private XDocument _accordionXDoc;

    private bool IsWellFormedAccordionXml
    {
      get { return !_accordionXDoc.Element("accordion").FirstAttribute.Name.Equals("error"); }
    }

    private string ParseAccordionXml(string attribute)
    {
      return IsWellFormedAccordionXml ? (_accordionXDoc.Element("accordion").Attribute(attribute) != null ? _accordionXDoc.Element("accordion").Attribute(attribute).Value : string.Empty) : string.Empty;
    }
    #endregion

    #region Content XML
    private XDocument _contentXDoc;

    private bool IsWellFormedContentXml
    {
      get { return !_contentXDoc.Element("content").HasAttributes; }
    }

    private string ParseContentXml(string attribute)
    {
      string attrib = string.Empty;

      if (IsWellFormedContentXml && _contentXDoc.Element("content").HasElements)
      {
        attrib = _contentXDoc.Element("content").Element("data").Attribute(attribute) != null ? _contentXDoc.Element("content").Element("data").Attribute(attribute).Value : string.Empty;
      }

      return attrib;
    }

    private LinkUrlData ParseContentBuyLinkXml(XElement link)
    {
      return IsWellFormedContentXml ? ParseLinkUrlXml(link) : null;
    }
    #endregion

    #region ControlPanel XML
    private XDocument _controlPanelXDoc;

    private bool IsWellFormedControlPanelXml
    {
      get { return !_controlPanelXDoc.Element("controlpanels").HasAttributes; }
    }

    private List<LinkUrlData> ParseControlPanelXml(XElement controlpanels)
    {
      List<LinkUrlData> links = new List<LinkUrlData>();
      if (IsWellFormedControlPanelXml)
      {
        foreach (XElement link in controlpanels.Elements("linkurl"))
        {
          links.Add(ParseLinkUrlXml(link));
        }
      }
      return links;
    }
    #endregion

    #region WorkspaceLogin XML
    private XDocument _workspaceLoginXDoc;

    private bool IsWellFormedWorkspaceLoginXml
    {
      get { return (_workspaceLoginXDoc.Element("workspace").FirstAttribute == null || !_workspaceLoginXDoc.Element("workspace").FirstAttribute.Name.Equals("error")); }
    }

    private LinkUrlData ParseWorkspaceLoginXml(XElement workspaceLoginXml, out string buttonText)
    {
      LinkUrlData linkUrlData = null;
      buttonText = string.Empty;

      if (IsWellFormedWorkspaceLoginXml && workspaceLoginXml.HasElements)
      {
        buttonText = workspaceLoginXml.Attribute("buttontext").Value;
        linkUrlData = ParseLinkUrlXml(workspaceLoginXml.Element("linkurl"));
      }
      return linkUrlData;
    }
    #endregion

    #region LinkUrl XML

    private LinkUrlData ParseLinkUrlXml(XElement linkUrlXml)
    {
      return new LinkUrlData(linkUrlXml);
    }
    #endregion

    #endregion

    public bool? IsAllInnerXmlValid { get; private set; }

    #region Stored Database Properties
    private readonly int _accordionId;
    public int AccordionId
    {
      get { return _accordionId; }
    }
    private readonly string _accordionTitle;
    public string AccordionTitle
    {
      get { return _accordionTitle; }
    }
    private string AccordionXml { get; set; }
    private string ContentXml { get; set; }
    private string ControlPanelXml { get; set; }
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
    private string WorkspaceLoginXml { get; set; }
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

    private WorkspaceLoginData _workspaceLogin;
    public WorkspaceLoginData WorkspaceLogin
    {
      get { return _workspaceLogin; }
    }

    #endregion

    #region Constructor
    public AccordionMetaData(XElement accordion)
    {
      _accordionId = Convert.ToInt32(accordion.Attribute("accordionId").Value);
      _accordionTitle = accordion.Attribute("accordionTitle").Value;
      AccordionXml = accordion.Attribute("accordionXml").Value;
      ContentXml = accordion.Attribute("contentXml").Value;
      ControlPanelXml = accordion.Attribute("controlPanelXml").Value;
      _defaultSortOrder = Convert.ToInt32(accordion.Attribute("defaultSortOrder").Value);
      _namespaces = new HashSet<string>(Convert.ToString(accordion.Attribute("namespaces").Value).ToLowerInvariant().Replace(" ", string.Empty).Split(','), StringComparer.InvariantCultureIgnoreCase);
      WorkspaceLoginXml = accordion.Attribute("workspaceLoginXml").Value;

      #region SetAccordionXmlAttributes
      string msg = string.Empty;
      _accordionXDoc = XDocument.Parse(AccordionXml);

      try
      {
        if (XmlValidator.ValidateAccordionXml(_accordionXDoc, out msg))
        {
          _ciExpansion = ParseAccordionXml("ciexpansion");
          _ciRenewNow = ParseAccordionXml("cirenewnow");
          _ciSetup = ParseAccordionXml("cisetup");
          _cmsDisplayGroups = SetCmsDisplayGroups(ParseAccordionXml("cmsdisplaygroupidlist"));
          _iconCssCoordinates = SetCoordinates(ParseAccordionXml("iconcsscoordinates"));
          _productGroup = Convert.ToInt32(ParseAccordionXml("productgroup"));
          _showControlPanel = string.Compare(ParseAccordionXml("controlpanelrequiresaccount"), "false", true) == 0;
          _showSetupForManagerOnly = string.Compare(ParseAccordionXml("showsetupformanageronly"), "true", true) == 0;
          _orionProductName = ParseAccordionXml("orionproductname");
          _isBundleProduct = string.Compare(ParseAccordionXml("isbundle"), "true", true) == 0;
        }
        else
        {
          IsAllInnerXmlValid = false;
          AtlantisException aex = new AtlantisException("AccordionMetaData::AccordionXDoc", "0", msg, string.Empty, null, null);
          _accordionXDoc = XDocument.Parse("<accordion error='AccordionXml Malformed'/>");
          Engine.Engine.LogAtlantisException(aex);
        }
      }
      catch (Exception ex)
      {
        IsAllInnerXmlValid = false;
        AtlantisException aex = new AtlantisException("AccordionMetaData::AccordionXDoc", "0", ex.Message, string.Empty, null, null);
        _accordionXDoc = XDocument.Parse("<accordion error='AccordionXml Malformed'/>");
        Engine.Engine.LogAtlantisException(aex);
      }
      #endregion
      
      _content = SetContentProperty();
      _controlPanels = SetControlPanelProperty();
      _workspaceLogin = SetWorkspaceLoginProperty();
    }

    private ContentData SetContentProperty()
    {
      bool isValid = false;
      string msg = string.Empty;
      ContentData content = null;
      XElement link = null;

      try
      {
        _contentXDoc = XDocument.Parse(ContentXml);
        isValid = XmlValidator.ValidateContentXml(_contentXDoc, out msg);
        if (isValid)
        {
          try
          {
            link = _contentXDoc.Element("content").Element("data").Element("linkurl");
          }
          catch
          {
            link = null;
          }
        }
        else
        {
          IsAllInnerXmlValid = false;
          AtlantisException aex = new AtlantisException("AccordionMetaData::ContentXDoc", "0", msg, string.Empty, null, null);
          _contentXDoc = XDocument.Parse("<content error='ContentXml Malformed'/>");
          Engine.Engine.LogAtlantisException(aex);
        }
      }
      catch (Exception ex)
      {
        link = null;
        IsAllInnerXmlValid = false;
        AtlantisException aex = new AtlantisException("AccordionMetaData::ContentXDoc", "0", ex.Message, string.Empty, null, null);
        _contentXDoc = XDocument.Parse("<content error='ContentXml Malformed'/>");
        Engine.Engine.LogAtlantisException(aex);
      }
      if (isValid)
      {
        if (link == null)
        {
          content = new ContentData(ParseContentXml("accountlist"), ParseContentXml("jsonpage"), ParseContentXml("cioptions"));
        }
        else
        {
          content = new ContentData(ParseContentXml("accountlist"), ParseContentXml("jsonpage"), ParseContentXml("cioptions"), ParseContentBuyLinkXml(link));
        }
      }

      return content;
    }
    
    private ControlPanelData SetControlPanelProperty()
    {
      ControlPanelData cpd = null;
      string msg = string.Empty;

      _controlPanelXDoc = XDocument.Parse(ControlPanelXml);
      try
      {
        if (XmlValidator.ValidateControlPanelXml(_controlPanelXDoc, out msg))
        {
          cpd = new ControlPanelData(ParseControlPanelXml(_controlPanelXDoc.Element("controlpanels")));
        }
        else
        {
          AtlantisException aex = new AtlantisException("AccordionMetaData::ControlPanelXDoc", "0", msg, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(aex);
          _controlPanelXDoc = XDocument.Parse("<controlpanels error='ControlPanelXml Malformed'/>");
          IsAllInnerXmlValid = false;
        }
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException("AccordionMetaData::ControlPanelXDoc", "0", ex.Message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(aex);
        _controlPanelXDoc = XDocument.Parse("<controlpanels error='ControlPanelXml Malformed'/>");
        IsAllInnerXmlValid = false;
      }
      return cpd;
    }

    private WorkspaceLoginData SetWorkspaceLoginProperty()
    {
      string buttonText = string.Empty;
      string msg = string.Empty;
      LinkUrlData linkUrl = null;

      try
      {
        _workspaceLoginXDoc = XDocument.Parse(WorkspaceLoginXml);

        if (XmlValidator.ValidateWorkspaceLoginXml(_workspaceLoginXDoc, out msg))
        {
          linkUrl = ParseWorkspaceLoginXml(_workspaceLoginXDoc.Element("workspace"), out buttonText);
        }
        else
        {
          AtlantisException aex = new AtlantisException("AccordionMetaData::WorkspaceLoginXDoc", "0", msg, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(aex);
          _workspaceLoginXDoc = XDocument.Parse("<workspace error='WorkspaceLoginXml Malformed'/>");
          IsAllInnerXmlValid = false;
        }
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException("AccordionMetaData::WorkspaceLoginXDoc", "0", ex.Message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(aex);
        _workspaceLoginXDoc = XDocument.Parse("<workspace error='WorkspaceLoginXml Malformed'/>");
        IsAllInnerXmlValid = false;
      }

      return new WorkspaceLoginData(linkUrl, buttonText);
    }
    #endregion

    #region Private Methods
    private CssSpriteCoordinate SetCoordinates(string iconnCssCoordinates)
    {
      string[] coordintatePoints = iconnCssCoordinates.Split(',');

      try
      {
        return new CssSpriteCoordinate(coordintatePoints[0], coordintatePoints[1], coordintatePoints[2], coordintatePoints[3]);
      }
      catch
      {
        return new CssSpriteCoordinate("0px", "0px", "0px", "0px");
      }
    }

    private List<int> SetCmsDisplayGroups(string cmsDisplayGroups)
    {
      List<int> cmsDisplayGroupList = new List<int>();

      if (!string.IsNullOrEmpty(cmsDisplayGroups))
      {
        try
        {
          string[] listArr = cmsDisplayGroups.Split(',');
          foreach (string cmsGroup in listArr)
          {
            cmsDisplayGroupList.Add(Convert.ToInt32(cmsGroup));
          }
        }
        catch
        {
          cmsDisplayGroupList = new List<int>();
        }
      }
      return cmsDisplayGroupList;
    }
    #endregion

    #region Public Methods

    public bool HasProductList()
    {
      bool hasProductList;

      if (string.IsNullOrEmpty(ContentXml))
      {
        hasProductList = false;
      }
      else
      {
        try
        {
          XDocument root = XDocument.Parse(ContentXml);
          XElement content = root.Element("content");
          hasProductList = content.HasElements;
        }
        catch
        {
          hasProductList = false;
        }        
      }

      return hasProductList;
    }

    public bool ShowWorkspaceLogin()
    {
      bool show;
      if (string.IsNullOrEmpty(WorkspaceLoginXml))
      {
        show = false;
      }
      else
      {
        try
        {
          XDocument root = XDocument.Parse(WorkspaceLoginXml);
          XElement workspace = root.Element("workspace");
          show = workspace.HasElements;
        }
        catch
        {
          show = false;
        }
      }
      return show;
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
