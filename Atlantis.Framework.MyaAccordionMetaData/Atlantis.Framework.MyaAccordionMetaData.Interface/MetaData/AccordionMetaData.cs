using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
      get { return !WorkspaceLoginXDoc.Element("workspace").HasAttributes; }
    }

    private LinkUrlData ParseWorkspaceLoginXml(XElement workspaceLogin)
    {
      return IsWellFormedWorkspaceLoginXml ? (workspaceLogin.HasElements ? ParseLinkUrlXml(workspaceLogin.Element("linkurl")) : null) : null;
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
    public int AccordionId { get; set; }
    public string AccordionTitle { get; set; }
    public string AccordionXml { get; set; }
    public string ContentXml { get; set; }
    public string ControlPanelXml { get; set; }
    public int DefaultSortOrder { get; set; }
    public HashSet<string> Namespaces { get; set; }
    public string WorkspaceLoginXml { get; set; }
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
    public string CiExpansion 
    {
      get { return ParseAccordionXml("ciexpansion"); }
    }
    public string CiRenewNow
    {
      get { return ParseAccordionXml("cirenewnow"); }
    }
    public string CiSetup
    {
      get { return ParseAccordionXml("cisetup"); }    
    }
    public CssSpriteCoordinate IconCssCoordinates 
    { 
      get { return SetCoordinates(ParseAccordionXml("iconcsscoordinates")); }
    }
    public bool IsProductOfferedFree
    {
      get { return string.Compare(ParseAccordionXml("isproductofferedfree"), "true", true) == 0; }
    }
    public int ProductGroup
    {
      get { return Convert.ToInt32(ParseAccordionXml("productgroup")); }
    }
    public bool ShowControlPanel
    {
      get { return string.Compare(ParseAccordionXml("controlpanelrequiresaccount"), "false", true) == 0; }
    }
    public bool ShowSetupForManagerOnly
    {
      get { return string.Compare(ParseAccordionXml("showsetupformanageronly"), "true", true) == 0; }
    }
    public string OrionProductName
    {
      get { return ParseAccordionXml("orionproductname"); }
    }
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
    public ContentData Content
    {
      get 
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
    }
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

    public ControlPanelData ControlPanels
    {
      get { return new ControlPanelData(ParseControlPanelXml(ControlPanelXDoc.Element("controlpanels"))); }
    }
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
      public NameValueCollection QsKeys { get; set; }

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

      public WorkspaceLoginData(LinkUrlData linkUrl)
      {
        LinkUrl = linkUrl;
      }
    }

    public WorkspaceLoginData WorkspaceLogin
    {
      get { return new WorkspaceLoginData(ParseWorkspaceLoginXml(WorkspaceLoginXDoc.Element("workspace"))); }
    }
    #endregion

    #endregion

    #region Constructor
    public AccordionMetaData()
    { }
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
