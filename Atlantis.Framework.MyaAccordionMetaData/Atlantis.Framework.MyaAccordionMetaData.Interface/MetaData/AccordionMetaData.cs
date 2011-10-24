using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.MyaAccordionMetaData.Interface
{
  public class AccordionMetaData
  {
    #region Properties
    public int AccordionId { get; private set; }
    public string AccordionTitle { get; private set; }
    public string CiExpansion { get; private set; }
    public string CiRenewNow { get; private set; }
    public string CiSetup { get; private set; }
    public string ContentXml { get; private set; }
    public string ControlPanelXml { get; private set; }
    public bool ControlPanelRequiresAccount { get; private set; }
    public int DefaultSortOrder { get; private set; }
    public bool IsProductOfferedFree { get; private set; }
    public string MyaUserControl { get; private set; }
    public List<string> Namespaces { get; private set; }
    public bool ShowSetupForManagerOnly { get; private set; }
    public string WorkspaceLoginXml { get; private set; }
   
    #endregion

    #region Constructor
    public AccordionMetaData(DataRow dr)
    {
      AccordionId = Convert.ToInt32(dr["accordionId"]);
      AccordionTitle = Convert.ToString(dr["accordionTitle"]);
      CiExpansion = Convert.ToString(dr["ciExpansion"]);
      CiRenewNow = Convert.ToString(dr["ciRenewNow"]);
      CiSetup = Convert.ToString(dr["ciSetup"]);
      ContentXml = Convert.ToString(dr["contentXml"]);
      ControlPanelXml = Convert.ToString(dr["controlPanelXml"]);
      ControlPanelRequiresAccount = string.Compare(Convert.ToString(dr["controlPanelRequiresAccount"]), "1") == 0;
      DefaultSortOrder = Convert.ToInt32(dr["defaultSortOrder"]);
      IsProductOfferedFree = string.Compare(Convert.ToString(dr["isProductOfferedFree"]), "1") == 0;
      MyaUserControl = Convert.ToString(dr["myaUserControl"]);
      Namespaces = Convert.ToString(dr["namespaces"]).ToLowerInvariant().Replace(" ", "").Split(',').ToList<string>();
      ShowSetupForManagerOnly = string.Compare(Convert.ToString(dr["showSetupForManagerOnly"]), "1") == 0;
      WorkspaceLoginXml = Convert.ToString(dr["workspaceLoginXml"]);
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
