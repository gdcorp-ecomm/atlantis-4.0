using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.MyaAccordionMetaData.Interface
{
  public class AccordionMetaData
  {
    #region Properties
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

    public int AccordionId { get; set; }
    public string AccordionTitle { get; set; }
    public string CiExpansion { get; set; }
    public string CiRenewNow { get; set; }
    public string CiSetup { get; set; }
    public string ContentXml { get; set; }
    public string ControlPanelXml { get; set; }
    public bool ControlPanelRequiresAccount { get; set; }
    public int DefaultSortOrder { get; set; }
    public CssSpriteCoordinate IconnCssCoordinates { get; set; }
    public bool IsProductOfferedFree { get; set; }
    public List<string> Namespaces { get; set; }
    public bool ShowSetupForManagerOnly { get; set; }
    public string WorkspaceLoginXml { get; set; }
   
    #endregion

    #region Constructor
    public AccordionMetaData()
    { }

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

    #region Convenience Methods
    public bool ShowControlPanel()
    {
      return !ControlPanelRequiresAccount;
    }

    public bool HasProductList()
    {
      bool hasProductList;

      if (string.IsNullOrWhiteSpace(ContentXml))
      {
        hasProductList = false;
      }
      else
      {
        XDocument content = XDocument.Parse(ContentXml);
        XElement root = content.Element("groups");
        hasProductList = root.HasElements;
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
        XDocument content = XDocument.Parse(WorkspaceLoginXml);
        XElement workspace = content.Element("workspace");
        show = workspace.HasAttributes && !string.IsNullOrWhiteSpace(workspace.Attribute("apptag").Value);       
      }
      return show;
    }
    #endregion
  }
}
