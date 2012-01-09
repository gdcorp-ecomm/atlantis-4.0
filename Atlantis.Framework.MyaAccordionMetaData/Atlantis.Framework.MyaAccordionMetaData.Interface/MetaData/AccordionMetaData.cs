using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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
    private XDocument AccordionXDoc
    {
      get
      {
        string msg = string.Empty;
        if (_accordionXDoc == null)
        {
          try
          {
            _accordionXDoc = XDocument.Parse(AccordionXml);
            XmlValidator.ValidateAccordionXml(_accordionXDoc, out msg);

            if (!string.IsNullOrWhiteSpace(msg))
            {
              AtlantisException aex = new AtlantisException("AccordionMetaData::AccordionXDoc", "0", msg, string.Empty, null, null);
              Engine.Engine.LogAtlantisException(aex);
              _accordionXDoc = XDocument.Parse("<accordion error='AccordionXml Malformed'/>");
              IsAllInnerXmlValid = false;
            }
          }
          catch (Exception ex)
          {
            AtlantisException aex = new AtlantisException("AccordionMetaData::AccordionXDoc", "0", ex.Message, string.Empty, null, null);
            Engine.Engine.LogAtlantisException(aex);
            _accordionXDoc = XDocument.Parse("<accordion error='AccordionXml Malformed'/>");
            IsAllInnerXmlValid = false;
          }
        }
        return _accordionXDoc;
      }
    }

    private bool IsWellFormedAccordionXml
    {
      get { return !AccordionXDoc.Element("accordion").FirstAttribute.Name.Equals("error"); }
    }

    private string ParseAccordionXml(string attribute)
    {
      return IsWellFormedAccordionXml ? (AccordionXDoc.Element("accordion").Attribute(attribute) != null ? AccordionXDoc.Element("accordion").Attribute(attribute).Value : string.Empty) : string.Empty;
    }
    #endregion

    #region Content XML
    private XDocument _contentXDoc;
    private XDocument ContentXDoc
    {
      get
      {
        string msg = string.Empty;
        if (_contentXDoc == null)
        {
          try
          {
            _contentXDoc = XDocument.Parse(ContentXml);
            XmlValidator.ValidateContentXml(_contentXDoc, out msg);

            if (!string.IsNullOrWhiteSpace(msg))
            {
              AtlantisException aex = new AtlantisException("AccordionMetaData::ContentXDoc", "0", msg, string.Empty, null, null);
              Engine.Engine.LogAtlantisException(aex);
              _contentXDoc = XDocument.Parse("<content error='ContentXml Malformed'/>");
              IsAllInnerXmlValid = false;
            }
          }
          catch (Exception ex)
          {
            AtlantisException aex = new AtlantisException("AccordionMetaData::ContentXDoc", "0", ex.Message, string.Empty, null, null);
            Engine.Engine.LogAtlantisException(aex);
            _contentXDoc = XDocument.Parse("<content error='ContentXml Malformed'/>");
            IsAllInnerXmlValid = false;
          }
        }
        return _contentXDoc;
      }
    }

    private bool IsWellFormedContentXml
    {
      get { return !ContentXDoc.Element("content").HasAttributes; }
    }

    private string ParseContentXml(string attribute)
    {
      string attrib = string.Empty;

      if (IsWellFormedContentXml && ContentXDoc.Element("content").HasElements)
      {
        attrib = ContentXDoc.Element("content").Element("data").Attribute(attribute) != null ? ContentXDoc.Element("content").Element("data").Attribute(attribute).Value : string.Empty;
      }

      return attrib;
    }

    private LinkUrlData ParseContentBuyLinkXml(XElement link)
    {
      return IsWellFormedWorkspaceLoginXml ? ParseLinkUrlXml(link) : null;
    }
    #endregion

    #region ControlPanel XML
    private XDocument _controlPanelXDoc;
    private XDocument ControlPanelXDoc
    {
      get
      {
        string msg = string.Empty;
        if (_controlPanelXDoc == null)
        {
          try
          {
            _controlPanelXDoc = XDocument.Parse(ControlPanelXml);
            XmlValidator.ValidateControlPanelXml(_controlPanelXDoc, out msg);

            if (!string.IsNullOrWhiteSpace(msg))
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
        }
        return _controlPanelXDoc;
      }
    }

    private bool IsWellFormedControlPanelXml
    {
      get { return !ControlPanelXDoc.Element("controlpanels").HasAttributes; }
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
    private XDocument WorkspaceLoginXDoc
    {
      get
      {
        string msg = string.Empty;
        if (_workspaceLoginXDoc == null)
        {
          try
          {
            _workspaceLoginXDoc = XDocument.Parse(WorkspaceLoginXml);
            XmlValidator.ValidateWorkspaceLoginXml(_workspaceLoginXDoc, out msg);

            if (!string.IsNullOrWhiteSpace(msg))
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
        }
        return _workspaceLoginXDoc;
      }
    }

    private bool IsWellFormedWorkspaceLoginXml
    {
      get { return (WorkspaceLoginXDoc.Element("workspace").FirstAttribute == null || !WorkspaceLoginXDoc.Element("workspace").FirstAttribute.Name.Equals("error")); }
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
    private XDocument LinkUrlXDoc(XElement linkUrlXml)
    {
      XDocument linkUrlXDoc;
      string msg = string.Empty;
      try
      {
        linkUrlXDoc = XDocument.Parse(linkUrlXml.ToString());
        XmlValidator.ValidateLinkXml(linkUrlXDoc, out msg);

        if (!string.IsNullOrWhiteSpace(msg))
        {
          AtlantisException aex = new AtlantisException("AccordionMetaData::LinkUrlXDoc", "0", msg, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(aex);
          linkUrlXDoc = XDocument.Parse("<linkurl error='LinkUrl Malformed'/>");
          IsAllInnerXmlValid = false;
        }
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException("AccordionMetaData::LinkUrlXDoc", "0", ex.Message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(aex);
        linkUrlXDoc = XDocument.Parse("<linkurl error='LinkUrl Malformed'/>");
        IsAllInnerXmlValid = false;
      }

      return linkUrlXDoc;
    }

    private bool IsWellFormedLinkUrlXml(XElement linkUrlXml)
    {
      return !LinkUrlXDoc(linkUrlXml).Element("linkurl").FirstAttribute.Name.Equals("error"); 
    }

    private LinkUrlData ParseLinkUrlXml(XElement linkUrlXml)
    {
      LinkUrlData linkUrl = null;
      NameValueCollection nvc = new NameValueCollection();

      if (IsWellFormedLinkUrlXml(linkUrlXml))
      {
        linkUrl = new LinkUrlData();
        linkUrl.Link = linkUrlXml.Attribute("link").Value;
        linkUrl.Page = linkUrlXml.Attribute("page") != null ? linkUrlXml.Attribute("page").Value : string.Empty;
        linkUrl.Type = (LinkUrlData.TypeOfLink)Enum.Parse(typeof(LinkUrlData.TypeOfLink), linkUrlXml.Attribute("type").Value);
        linkUrl.IdentificationRule = linkUrlXml.Attribute("identificationrule") != null ? linkUrlXml.Attribute("identificationrule").Value : string.Empty;
        linkUrl.IdentificationValue = linkUrlXml.Attribute("identificationvalue") != null ? linkUrlXml.Attribute("identificationvalue").Value : string.Empty;
        linkUrl.EnvironmentHttpsRequirements = BuildEnvironmentHttpsDictionary(linkUrlXml.Attribute("isenvsecure") != null ? linkUrlXml.Attribute("isenvsecure").Value : string.Empty);

        if (linkUrlXml.HasElements)
        {
          foreach (XElement qsKey in linkUrlXml.Elements("qskey"))
          {
            nvc.Add(qsKey.Attribute("name").Value, qsKey.Attribute("value").Value);
          }
          linkUrl.QsKeys = nvc;
        }
      }

      return linkUrl;
    }
    #endregion

    #endregion

    #region Public Properties

    public bool? IsAllInnerXmlValid { get; private set; }

    #region Stored Database Properties
    public int AccordionId { get; private set; }
    public string AccordionTitle { get; private set; }
    private string AccordionXml { get; set; }
    private string ContentXml { get; set; }
    private string ControlPanelXml { get; set; }
    public int DefaultSortOrder { get; private set; }
    public HashSet<string> Namespaces { get; private set; }
    private string WorkspaceLoginXml { get; set; }
    #endregion

    #region Derived Accordion Xml Properties
    public class CssSpriteCoordinate
    {
      public string X { get; private set; }
      public string Y { get; private set; }
      public string Width { get; private set; }
      public string Height { get; private set; }

      public CssSpriteCoordinate(string x, string y, string width, string height)
      {
        X = x;
        Y = y;
        Width = width;
        Height = height;
      }
    }
    public string CiExpansion { get; private set; }
    public string CiRenewNow { get; private set; }
    public string CiSetup { get; private set; }
    public List<int> CmsDisplayGroups { get; private set; }
    public CssSpriteCoordinate IconCssCoordinates { get; private set; }
    public int ProductGroup { get; private set; }
    public bool ShowControlPanel { get; private set; }
    public bool ShowSetupForManagerOnly { get; private set; }
    public string OrionProductName { get; private set; }
    public bool IsBundleProduct { get; private set; }
    #endregion

    #region Derived Content Xml Properties
    public class ContentData
    {
      public string AccountList { get; private set; }
      public string JsonPage { get; private set; }
      public string CiOptions { get; private set; }
      public LinkUrlData LinkUrl { get; private set; }

      public bool ShowOptionsLink
      {
        get { return !string.IsNullOrWhiteSpace(CiOptions); }
      }

      public bool ShowBuyLink
      {
        get { return LinkUrl != null; }
      }

      public ContentData(string accountList, string jsonPage, string ciOptions, LinkUrlData linkUrl)
      {
        AccountList = accountList;
        JsonPage = jsonPage;
        CiOptions = ciOptions;
        LinkUrl = linkUrl;
      }
      
      public ContentData(string accountList, string jsonPage, string ciOptions)
      {
        AccountList = accountList;
        JsonPage = jsonPage;
        CiOptions = ciOptions;
        LinkUrl = null;
      }
    }
    public ContentData Content { get; private set; }
    #endregion

    #region Derived ControlPanel Xml Properties
    public class ControlPanelData
    {
      public List<LinkUrlData> LinkUrls { get; private set; }
      public bool HasManagerLink(string identificationValue)
      {
        return LinkUrls.Exists(new Predicate<LinkUrlData>(url => url.Type.Equals(LinkUrlData.TypeOfLink.Manager) && url.IdentificationValue.Equals(identificationValue)));
      }

      public bool DoLinkUrlsContainIdentificationRules
      {
        get { return LinkUrls.Exists(new Predicate<LinkUrlData>(url => !string.IsNullOrWhiteSpace(url.IdentificationRule))); }
      }

      public ControlPanelData(List<LinkUrlData> linkUrls)
      {
        LinkUrls = linkUrls;
      }
    }

    public ControlPanelData ControlPanels { get; private set; }
    #endregion

    #region Derived LinkUrl Xml Properties
    public class LinkUrlData
    {
      public enum TypeOfLink : int
      {
        Standard = 0,
        Manager = 1
      }
      public string Link { get; set; }
      public string Page { get; set; }
      public TypeOfLink Type { get; set; }
      public string IdentificationRule { get; set; }
      public string IdentificationValue { get; set; }
      public Dictionary<int, bool> EnvironmentHttpsRequirements { get; set; }
      public NameValueCollection QsKeys { get; set; }

      public bool DoesEnvironmentRequireSecureLink(int environment)
      {
        bool isSecure = false;
        if (!EnvironmentHttpsRequirements.TryGetValue(environment, out isSecure))
        {
          isSecure = false;
        }

        return isSecure;
      }

      public LinkUrlData()
      { }
    }
    #endregion

    #region Derived WorkspaceLogin Xml Properties
    public class WorkspaceLoginData
    {
      public LinkUrlData LinkUrl { get; private set; }
      public bool HasLink
      {
        get { return LinkUrl != null; }
      }

      public string ButtonText { get; set; }

      public WorkspaceLoginData(LinkUrlData linkUrl, string buttonText)
      {
        LinkUrl = linkUrl;
        ButtonText = buttonText;
      }
    }

    public WorkspaceLoginData WorkspaceLogin { get; private set; }
    #endregion

    #endregion

    #region Constructor
    public AccordionMetaData(XElement accordion)
    {
      AccordionId = Convert.ToInt32(accordion.Attribute("accordionId").Value);
      AccordionTitle = accordion.Attribute("accordionTitle").Value;
      AccordionXml = accordion.Attribute("accordionXml").Value;
      ContentXml = accordion.Attribute("contentXml").Value;
      ControlPanelXml = accordion.Attribute("controlPanelXml").Value;
      DefaultSortOrder = Convert.ToInt32(accordion.Attribute("defaultSortOrder").Value);
      Namespaces = new HashSet<string>(Convert.ToString(accordion.Attribute("namespaces").Value).ToLowerInvariant().Replace(" ", string.Empty).Split(','), StringComparer.InvariantCultureIgnoreCase);
      WorkspaceLoginXml = accordion.Attribute("workspaceLoginXml").Value;
      SetAccordionXmlAttributes();
      Content = SetContentProperty();
      ControlPanels = SetControlPanelProperty();
      WorkspaceLogin = SetWorkspaceLoginProperty();
    }

    private void SetAccordionXmlAttributes()
    {
      CiExpansion = ParseAccordionXml("ciexpansion");
      CiRenewNow = ParseAccordionXml("cirenewnow");
      CiSetup = ParseAccordionXml("cisetup");
      CmsDisplayGroups = SetCmsDisplayGroups(ParseAccordionXml("cmsdisplaygroupidlist"));
      IconCssCoordinates = SetCoordinates(ParseAccordionXml("iconcsscoordinates"));
      ProductGroup = Convert.ToInt32(ParseAccordionXml("productgroup"));
      ShowControlPanel = string.Compare(ParseAccordionXml("controlpanelrequiresaccount"), "false", true) == 0;
      ShowSetupForManagerOnly = string.Compare(ParseAccordionXml("showsetupformanageronly"), "true", true) == 0;
      OrionProductName = ParseAccordionXml("orionproductname");
      IsBundleProduct = string.Compare(ParseAccordionXml("isbundle"), "true", true) == 0;
    }

    private ContentData SetContentProperty()
    {
      ContentData content;
      XElement link;
      try
      {
        link = ContentXDoc.Element("content").Element("data").Element("linkurl");
      }
      catch
      {
        link = null;
      }
      if (link == null)
      {
        content = new ContentData(ParseContentXml("accountlist"), ParseContentXml("jsonpage"), ParseContentXml("cioptions"));
      }
      else
      {
        content = new ContentData(ParseContentXml("accountlist"), ParseContentXml("jsonpage"), ParseContentXml("cioptions"), ParseContentBuyLinkXml(link));
      }

      return content;
    }
    
    private ControlPanelData SetControlPanelProperty()
    {
      return new ControlPanelData(ParseControlPanelXml(ControlPanelXDoc.Element("controlpanels")));
    }

    private WorkspaceLoginData SetWorkspaceLoginProperty()
    {
      string buttonText = string.Empty;
      LinkUrlData linkUrl = ParseWorkspaceLoginXml(WorkspaceLoginXDoc.Element("workspace"), out buttonText);

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

      if (!string.IsNullOrWhiteSpace(cmsDisplayGroups))
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
    private enum ServerLocationType
    {
      Undetermined = 0,
      Dev = 1,
      Test = 2,
      Ote = 3,
      Prod = 4,
    }
    private Dictionary<int,bool> BuildEnvironmentHttpsDictionary(string secureEnvironmentString)
    {
      Dictionary<int, bool> envDict = new Dictionary<int, bool>();
      List<string> envs = string.IsNullOrWhiteSpace(secureEnvironmentString) ? new List<string>() : secureEnvironmentString.Split(',').ToList<string>();

      envDict.Add((int)ServerLocationType.Dev, false);
      envDict.Add((int)ServerLocationType.Ote, false);
      envDict.Add((int)ServerLocationType.Prod, false);
      envDict.Add((int)ServerLocationType.Test, false);

      if (envs.Count > 0)
      {
        foreach (string env in envs)
        {
          envDict[(int)Enum.Parse(typeof(ServerLocationType), env)] = true;
        }
      }
      return envDict;
    }
    #endregion

    #region Public Methods

    public bool HasProductList()
    {
      bool hasProductList;

      if (string.IsNullOrWhiteSpace(ContentXml))
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
      if (string.IsNullOrWhiteSpace(WorkspaceLoginXml))
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
