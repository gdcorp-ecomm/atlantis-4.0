﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaAccordionMetaData.Interface.MetaData;

namespace Atlantis.Framework.MyaAccordionMetaData.Interface.MetaDataBuilder
{
  public class XmlBuilder
  {
    private XDocument _xDoc;

    public List<AccordionMetaData> BuildMetaDataList(string metadataXml)
    {
      string metaDataXml = metadataXml;
      if (string.IsNullOrEmpty(metaDataXml))
      {
        throw new ArgumentException("Cannot be null or empty. ", metaDataXml);
      }

      var accordionMetaDataList = new List<AccordionMetaData>();

      _xDoc = XDocument.Parse(metaDataXml);

      var accordionsXml = _xDoc.Element("data").Elements();
      foreach (var accordion in accordionsXml)
      {
        try
        {
          var contentXml = accordion.Attribute("contentXml").Value;
          ContentData contentData = CreateContentData(contentXml);

          var controlPanelXml = accordion.Attribute("controlPanelXml").Value;
          ControlPanelData controlPanelData = CreateControlPanelData(controlPanelXml);

          var workspaceLoginXml = accordion.Attribute("workspaceLoginXml").Value;
          WorkspaceLoginData workspaceLoginData = CreateWorkspaceLoginData(workspaceLoginXml);

          AccordionMetaData amd = CreateAccordionMetaData(accordion, contentData, controlPanelData, workspaceLoginData);

          accordionMetaDataList.Add(amd);
        }
        catch (AtlantisException aex)
        {
          Engine.Engine.LogAtlantisException(aex);
        }
        catch (Exception ex)
        {
          var aex = new AtlantisException("XmlBuilder::BuildMetaDataList", "0", "Exception: " + ex.Message, ex.StackTrace, null, null);
          Engine.Engine.LogAtlantisException(aex);
        }
      }

      accordionMetaDataList.Sort();

      return accordionMetaDataList;
    }

    private AccordionMetaData CreateAccordionMetaData(XElement accordion, ContentData contentData, ControlPanelData controlPanelData, WorkspaceLoginData workspaceLoginData)
    {
      var accordionId = Convert.ToInt32(accordion.Attribute("accordionId").Value);

      string accordionTitleDefault;
      string accordionTitleGoDaddy;
      GetAccordionTitle(accordion, accordionId, out accordionTitleDefault, out accordionTitleGoDaddy);

      var defaultSortOrder = Convert.ToInt32(accordion.Attribute("defaultSortOrder").Value);
      var namespaces =
        new HashSet<string>(
          Convert.ToString(accordion.Attribute("namespaces").Value).Replace(" ", string.Empty).
            Split(','), StringComparer.InvariantCultureIgnoreCase);
      var accordionXml = accordion.Attribute("accordionXml").Value;

      _accordionXDoc = XDocument.Parse(accordionXml);

      string ciExpansion;
      string ciRenewNow;
      string ciSetup;
      List<int> cmsDisplayGroups;
      CssSpriteCoordinate iconCssCoordinates;
      int productGroup;
      HashSet<int> productTypes;
      bool showControlPanel;
      bool showSetupForManagerOnly;
      string orionProductName;
      bool isBundleProduct;
      Dictionary<int, ProductMap> productMaps;
      Dictionary<SurveyData.SurveyTypes, SurveyData> surveyData;
      GetAccordionAttributes(out ciExpansion, out ciRenewNow, out ciSetup, out cmsDisplayGroups,
                             out iconCssCoordinates, out productGroup, out productTypes,
                             out showControlPanel, out showSetupForManagerOnly, out orionProductName,
                             out isBundleProduct, out productMaps, out surveyData);

      return new AccordionMetaData(accordionId, accordionTitleDefault, accordionTitleGoDaddy, defaultSortOrder, namespaces,
                                   ciExpansion, ciRenewNow, ciSetup, cmsDisplayGroups,
                                   iconCssCoordinates,
                                   productGroup, productTypes, showControlPanel, showSetupForManagerOnly,
                                   orionProductName, isBundleProduct, productMaps, surveyData,
                                   contentData, controlPanelData, workspaceLoginData);
    }

    private static void GetAccordionTitle(XElement accordion, int accordionId, out string accordionTitleDefault, out string accordionTitleGoDaddy) {
      var accrdTitleAttr = accordion.Attribute("accordionTitle");
      if (accrdTitleAttr != null && !string.IsNullOrWhiteSpace(accrdTitleAttr.Value))
      {
        var accordionTitleXDoc = XDocument.Parse(accrdTitleAttr.Value);
        var titleElement = accordionTitleXDoc.Element("title");
        accordionTitleDefault = GetAttributeValue(titleElement, "default");
        if (string.IsNullOrWhiteSpace(accordionTitleDefault)) { throw new Exception(string.Format("No default title provided. AccordionId: {0}", accordionId)); }
        accordionTitleGoDaddy = GetAttributeValue(titleElement, "context1");
      }
      else
      {
        throw new Exception(string.Format("AccordionTitle is missing or empty in xml. AccordianId: {0}", accordionId));
      }
    }

    private static string GetAttributeValue(XElement xElem, string attrName)
    {
      string attrValue = null;
      if (xElem != null)
      {
        var attr = xElem.Attribute(attrName);
        if (attr != null)
        {
          attrValue = attr.Value;
        }
      }
      return attrValue;
    }
    
    public bool? IsAllInnerXmlValid { get; private set; }

    #region AccordianMetaData

    private XDocument _accordionXDoc;

    private void GetAccordionAttributes(out string ciExpansion, out string ciRenewNow, out string ciSetup,
                                        out List<int> cmsDisplayGroups, out CssSpriteCoordinate iconCssCoordinates,
                                        out int productGroup, out HashSet<int> productTypes,
                                        out bool showControlPanel, out bool showSetupForManagerOnly,
                                        out string orionProductName, out bool isBundleProduct, out Dictionary<int, ProductMap> productMaps, out Dictionary<SurveyData.SurveyTypes, SurveyData> surveyData)
    {
      ciExpansion = string.Empty;
      ciRenewNow = string.Empty;
      ciSetup = string.Empty;
      cmsDisplayGroups = null;
      iconCssCoordinates = null;
      productGroup = -1;
      showControlPanel = false;
      showSetupForManagerOnly = false;
      orionProductName = string.Empty;
      isBundleProduct = false;

      string msg;
      if (XmlValidator.ValidateAccordionXml(_accordionXDoc, out msg))
      {
        ciExpansion = ParseAccordionXml("ciexpansion");
        ciRenewNow = ParseAccordionXml("cirenewnow");
        ciSetup = ParseAccordionXml("cisetup");
        cmsDisplayGroups = SetCmsDisplayGroups(ParseAccordionXml("cmsdisplaygroupidlist"));
        iconCssCoordinates = SetCoordinates(ParseAccordionXml("iconcsscoordinates"));
        productGroup = Convert.ToInt32(ParseAccordionXml("productgroup"));
        productTypes = SetProductTypes(ParseAccordionXml("producttypes"));
        showControlPanel = string.Compare(ParseAccordionXml("controlpanelrequiresaccount"), "false", true) == 0;
        showSetupForManagerOnly = string.Compare(ParseAccordionXml("showsetupformanageronly"), "true", true) == 0;
        orionProductName = ParseAccordionXml("orionproductname");
        isBundleProduct = string.Compare(ParseAccordionXml("isbundle"), "true", true) == 0;
        productMaps = SetProductMaps(ParseAccordionXmlProductMaps());
        surveyData = SetSurveyData(ParseAccordionXmlSurveyData());
      }
      else
      {
        IsAllInnerXmlValid = false;
        throw new AtlantisException("XmlBuilder::GetAccordionAttributes", "0", "Error validating xml.", msg, null, null);
      }
    }

    private bool IsWellFormedAccordionXml
    {
      get { return !_accordionXDoc.Element("accordion").FirstAttribute.Name.Equals("error"); }
    }

    private string ParseAccordionXml(string attribute)
    {
      return IsWellFormedAccordionXml ? (_accordionXDoc.Element("accordion").Attribute(attribute) != null ? _accordionXDoc.Element("accordion").Attribute(attribute).Value : string.Empty) : string.Empty;
    }

    private XElement ParseAccordionXmlProductMaps()
    {
      return IsWellFormedAccordionXml ? (_accordionXDoc.Element("accordion").Element("productmaps") != null ? _accordionXDoc.Element("accordion").Element("productmaps") : null) : null;
    }

    private XElement ParseAccordionXmlSurveyData()
    {
      return IsWellFormedAccordionXml ? (_accordionXDoc.Element("accordion").Element("surveys") != null ? _accordionXDoc.Element("accordion").Element("surveys") : null) : null;
    }

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

    private Dictionary<SurveyData.SurveyTypes, SurveyData> SetSurveyData(XElement surveyDataColl)
    {
      var surveyDataDict = new Dictionary<SurveyData.SurveyTypes, SurveyData>();

      try
      {
        foreach (var survey in surveyDataColl.Elements("survey"))
        {
          var type = (SurveyData.SurveyTypes)Enum.Parse(typeof(SurveyData.SurveyTypes), survey.Attribute("type").Value);
          var linkText = survey.Attribute("linktext").Value;
          var productid = survey.Attribute("productid") != null ? survey.Attribute("productid").Value : string.Empty;
          LinkUrlData linkUrl = survey.Element("linkurl") != null ? ParseLinkUrlElement(survey.Element("linkurl")) : null;

          var surveyData = new SurveyData(type, linkText, productid, linkUrl);
          surveyDataDict[type] = surveyData;
        }
      }
      catch (Exception ex)
      {
        surveyDataDict = new Dictionary<SurveyData.SurveyTypes, SurveyData>();
      }
      return surveyDataDict;
    }

    private Dictionary<int, ProductMap> SetProductMaps(XElement productMaps)
    {
      var prodMapDict = new Dictionary<int, ProductMap>();

      if (productMaps != null)
      {
        foreach (XElement pm in productMaps.Elements())
        {
          var group = Convert.ToInt32(pm.Attribute("group").Value);
          var types = SetProductTypes(pm.Attribute("typelist").Value);
          var namespaces = SetProductNamespaces(pm.Attribute("ns").Value);
          var description = pm.Attribute("description").Value;

          var productMap = new ProductMap(group, types, namespaces, description);
          prodMapDict.Add(group, productMap);
        }
      }
      return prodMapDict;
    }

    private static HashSet<int> SetProductTypes(string typesStr) 
    { 
      var set = new HashSet<int>();
      if (string.IsNullOrWhiteSpace(typesStr))
      {
        return null;
      }
      var split = typesStr.Split(',');
      foreach (var typeStr in split)
      {
        int type;
        if (int.TryParse(typeStr, out type))
        {
          set.Add(type);
        }
        else
        {
          var data = string.Format("value unable to parse: '{0}', ", typeStr);
          var aex = new AtlantisException("XmlBuilder::SetProductTypes", "0", "Unable to parse an Accordion Metadata Product Type value", data, null, null);
          Engine.Engine.LogAtlantisException(aex);
        }
      }
      return set.Count == 0 ? null : set;
    }

    private static HashSet<string> SetProductNamespaces(string namespacesStr)
    {
      var set = new HashSet<string>();
      if (string.IsNullOrWhiteSpace(namespacesStr))
      {
        return null;
      }
      var split = namespacesStr.Split(',');
      foreach (var namespaceStr in split)
      {
        set.Add(namespaceStr);
      }
      return set.Count == 0 ? null : set;
    }

    #endregion
    
    #region ContentData

    private XDocument _contentXDoc;

    private ContentData CreateContentData(string contentXml)
    {
      string msg = string.Empty;
      ContentData content = null;
      List<LinkUrlData> links = new List<LinkUrlData>();
      string showOptionsForMgrOnly = string.Empty;

      _contentXDoc = XDocument.Parse(contentXml);
      if (XmlValidator.ValidateContentXml(_contentXDoc, out msg))
      {
        try
        {
          links = ParseContentLinks(_contentXDoc.Element("content").Element("data").Element("links"));
        }
        catch
        {
          links = new List<LinkUrlData>();
        }

        try
        {
          showOptionsForMgrOnly = _contentXDoc.Element("content").Element("data").Attribute("optionsmgronly").Value;
        }
        catch
        {
          showOptionsForMgrOnly = string.Empty;
        }

        switch (links.Count.Equals(0))
        {
          case true:
            content = string.IsNullOrEmpty(showOptionsForMgrOnly) ? new ContentData(ParseContentXml("accountlist"), ParseContentXml("jsonpage"), ParseContentXml("cioptions")) : new ContentData(ParseContentXml("accountlist"), ParseContentXml("jsonpage"), ParseContentXml("cioptions"), ParseContentXml("optionsmgronly"));
            break;
          case false:
            content = string.IsNullOrEmpty(showOptionsForMgrOnly) ? new ContentData(ParseContentXml("accountlist"), ParseContentXml("jsonpage"), ParseContentXml("cioptions"), null, links) : new ContentData(ParseContentXml("accountlist"), ParseContentXml("jsonpage"), ParseContentXml("cioptions"), ParseContentXml("optionsmgronly"), links);
            break;        
        }
      }
      else
      {
        IsAllInnerXmlValid = false;
        throw new AtlantisException("XmlBuilder::CreateContentData", "0", "Error validating xml.", msg, null, null);
      }
      return content;
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

    private bool IsWellFormedContentXml
    {
      get { return !_contentXDoc.Element("content").HasAttributes; }
    }

    private List<LinkUrlData> ParseContentLinks(XElement contentLinks)
    {
      List<LinkUrlData> links = new List<LinkUrlData>();
      if (IsWellFormedContentXml)
      {
        foreach (XElement link in contentLinks.Elements("linkurl"))
        {
          links.Add(ParseLinkUrlElement(link));
        }
      }
      return links;
    }
    #endregion

    #region ControlPanel

    private XDocument _controlPanelXDoc;

    private ControlPanelData CreateControlPanelData(string controlPanelXml)
    {
      ControlPanelData cpd = null;
      string msg = string.Empty;

      _controlPanelXDoc = XDocument.Parse(controlPanelXml);
      if (XmlValidator.ValidateControlPanelXml(_controlPanelXDoc, out msg))
      {
        cpd = new ControlPanelData(ParseControlPanelElement(_controlPanelXDoc.Element("controlpanels")));
      }
      else
      {
        IsAllInnerXmlValid = false;
        throw new AtlantisException("XmlBuilder::CreateControlPanelData", "0", "Error validating xml.", msg, null, null);
      }
      return cpd;
    }
    
    private bool IsWellFormedControlPanelXml
    {
      get { return !_controlPanelXDoc.Element("controlpanels").HasAttributes; }
    }

    private List<LinkUrlData> ParseControlPanelElement(XElement controlpanels)
    {
      List<LinkUrlData> links = new List<LinkUrlData>();
      if (IsWellFormedControlPanelXml)
      {
        foreach (XElement link in controlpanels.Elements("linkurl"))
        {
          links.Add(ParseLinkUrlElement(link));
        }
      }
      return links;
    }
    
    #endregion
    
    #region WorkspaceLogin

    private XDocument _workspaceLoginXDoc;

    private WorkspaceLoginData CreateWorkspaceLoginData(string workspaceLoginXml)
    {
      string buttonText;
      string msg = string.Empty;
      LinkUrlData linkUrl = null;

      _workspaceLoginXDoc = XDocument.Parse(workspaceLoginXml);

      if (XmlValidator.ValidateWorkspaceLoginXml(_workspaceLoginXDoc, out msg))
      {
        linkUrl = ParseWorkspaceLoginXml(_workspaceLoginXDoc.Element("workspace"), out buttonText);
      }
      else
      {
        IsAllInnerXmlValid = false;
        throw new AtlantisException("XmlBuilder::CreateWorkspaceLoginData", "0", "Error validating xml.", msg, null, null);
      }

      return new WorkspaceLoginData(linkUrl, buttonText);
    }

    private bool IsWellFormedWorkspaceLoginXml
    {
      get { return (_workspaceLoginXDoc.Element("workspace").FirstAttribute == null || !_workspaceLoginXDoc.Element("workspace").FirstAttribute.Name.Equals("error")); }
    }

    private LinkUrlData ParseWorkspaceLoginXml(XElement workspaceLoginElement, out string buttonText)
    {
      LinkUrlData linkUrlData = null;
      buttonText = string.Empty;

      if (IsWellFormedWorkspaceLoginXml && workspaceLoginElement.HasElements)
      {
        buttonText = workspaceLoginElement.Attribute("buttontext").Value;
        linkUrlData = ParseLinkUrlElement(workspaceLoginElement.Element("linkurl"));
      }
      return linkUrlData;
    }

    #endregion
    
    #region LinkUrl

    private LinkUrlData ParseLinkUrlElement(XElement linkUrlElement)
    {

      string msg = string.Empty;
      var linkUrlXDoc = XDocument.Parse(linkUrlElement.ToString());

      if (XmlValidator.ValidateLinkXml(linkUrlXDoc, out msg))
      {
        var link = linkUrlElement.Attribute("link").Value;
        var page = linkUrlElement.Attribute("page") != null ? linkUrlElement.Attribute("page").Value : string.Empty;
        var type = (LinkUrlData.TypeOfLink) Enum.Parse(typeof (LinkUrlData.TypeOfLink), linkUrlElement.Attribute("type").Value);
        var identificationRule = linkUrlElement.Attribute("identificationrule") != null
                                   ? linkUrlElement.Attribute("identificationrule").Value
                                   : string.Empty;
        var identificationValue = linkUrlElement.Attribute("identificationvalue") != null
                                    ? linkUrlElement.Attribute("identificationvalue").Value
                                    : string.Empty;
        var environmentHttpsRequirements =
          BuildEnvironmentHttpsDictionary(linkUrlElement.Attribute("isenvsecure") != null
                                            ? linkUrlElement.Attribute("isenvsecure").Value
                                            : string.Empty);

        var nvc = new NameValueCollection();
        if (linkUrlElement.HasElements)
        {
          foreach (XElement qsKey in linkUrlElement.Elements("qskey"))
          {
            nvc.Add(qsKey.Attribute("name").Value, qsKey.Attribute("value").Value);
          }
        }
        return new LinkUrlData(link,page,type,identificationRule, identificationValue, environmentHttpsRequirements, nvc);
      }
      else
      {
        IsAllInnerXmlValid = false;
        throw new AtlantisException("XmlBuilder::ParseLinkUrlElement", "0", "Error validating xml.", msg, null, null);
      }
    }

    private Dictionary<int, bool> BuildEnvironmentHttpsDictionary(string secureEnvironmentString)
    {
      Dictionary<int, bool> envDict = new Dictionary<int, bool>();
      List<string> envs = string.IsNullOrEmpty(secureEnvironmentString) ? new List<string>() : secureEnvironmentString.Split(',').ToList<string>();

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

  }
}
