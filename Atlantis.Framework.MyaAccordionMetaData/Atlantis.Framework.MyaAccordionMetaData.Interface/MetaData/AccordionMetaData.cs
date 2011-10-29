using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.MyaAccordionMetaData.Interface
{
  public class AccordionMetaData
  {
    #region Private Properties
    private XDocument _xDoc;
    private XDocument AccordionXDoc
    {
      get
      {
        if (_xDoc == null)
        {
          try
          {
            _xDoc = XDocument.Parse(AccordionXml);
          }
          catch
          {
            _xDoc = XDocument.Parse("<accordion error='AccordionXml Malformed'/>");
          }
        }
        return _xDoc;
      }
    }
    private bool IsWellFormedAccordionXml
    {
      get { return !AccordionXDoc.Element("accordion").FirstAttribute.Name.Equals("error"); }
    }

    #endregion

    #region Public Properties
    public class CssSpriteCoordinate
    {
      public string X { get; set; }
      public string Y { get; set; }
      public string Width { get; set; }
      public string Height { get; set; }

      public CssSpriteCoordinate(string x, string y, string width, string height)
      {
        X = x;
        Y = y;
        Width = width;
        Height = height;
      }
    }

    #region Stored Database Properties
    public int AccordionId { get; set; }
    public string AccordionTitle { get; set; }
    public string AccordionXml { get; set; }
    public string ContentXml { get; set; }
    public string ControlPanelXml { get; set; }
    public int DefaultSortOrder { get; set; }
    public List<string> Namespaces { get; set; }
    public string WorkspaceLoginXml { get; set; }
    #endregion

    #region Derived Xml Properties
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
    public bool ShowControlPanel
    {
      get { return string.Compare(ParseAccordionXml("controlpanelrequiresaccount"), "false", true) == 0; }
    }
    public bool ShowSetupForManagerOnly
    {
      get { return string.Compare(ParseAccordionXml("showsetupformanageronly"), "true", true) == 0; }
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

    private string ParseAccordionXml(string attribute)
    {
      return IsWellFormedAccordionXml ? AccordionXDoc.Element("accordion").Attribute(attribute).Value : string.Empty;
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
  }
}
