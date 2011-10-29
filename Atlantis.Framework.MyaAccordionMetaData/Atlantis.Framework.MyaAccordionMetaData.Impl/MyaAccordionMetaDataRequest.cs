using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaAccordionMetaData.Interface;

namespace Atlantis.Framework.MyaAccordionMetaData.Impl
{
  public class MyaAccordionMetaDataRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MyaAccordionMetaDataResponseData responseData = null;

      try
      {
        //string metaDataXml = DataCache.DataCache.GetCacheData("<AccordionMetaData/>");
        string metaDataXml = BuildDebugMetaDataTable();

        responseData = new MyaAccordionMetaDataResponseData(metaDataXml);
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new MyaAccordionMetaDataResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new MyaAccordionMetaDataResponseData(requestData, ex);
      }

      return responseData;
    }

    #region Debug Data
    private string BuildDebugMetaDataTable()
    {
      XElement root = new XElement("data",
        new XAttribute("count", "4"));

      XElement accordiondata1 = new XElement("accordiondata"
        , new XAttribute("accordionId", 1)
        , new XAttribute("accordionTitle", "Web Hosting")
        , new XAttribute("ciExpansion", "11111")
        , new XAttribute("ciRenewNow", "22222")
        , new XAttribute("ciSetup", "33333")
        , new XAttribute("contentXml", "<content accountlist='mya_accountListGetHosting_sp' usercontrol='GetProductList.ascx'/>")
        , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='HCCURL' ci='44444' type='std'><qskey name='rID' value='%RID%'/><qskey name='pID' value='%PID%'/><qskey name='erID' value='%ERID%'/><qskey name='common_name' value='%CN%'/></linkurl></controlpanels>")
        , new XAttribute("controlPanelRequiresAccount", 0)
        , new XAttribute("defaultSortOrder", 3)
        , new XAttribute("iconcsscoordinate", "-34px,-82px,14px,14px")
        , new XAttribute("isProductOfferedFree", 1)
        , new XAttribute("namespaces", "pg|1,Hosting,hostingfree")
        , new XAttribute("showSetupForManagerOnly", 0)
        , new XAttribute("workspaceLoginXml", "<workspace/>"));

      XElement accordiondata2 = new XElement("accordiondata"
        , new XAttribute("accordionId", 10)
        , new XAttribute("accordionTitle", "Express Email Marketing")
        , new XAttribute("ciExpansion", "12345")
        , new XAttribute("ciRenewNow", "12346")
        , new XAttribute("ciSetup", "12347")
        , new XAttribute("contentXml", "<content accountlist='mya_accountListGetEEM_sp' usercontrol='GetProductList.ascx'/>")
        , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='EEMURL' ci='12348' type='std'><qskey name='rID' value='%RID%'/><qskey name='pID' value'%PID%'/><qskey name='erID' value='-1'/><qskey name='common_name' value='%CN%'/><qskey name='start' value='%START%'/><qskey name='recurring' value='%RECUR%'/><qskey name='id' value='%ID%'/><qskey name='pbid' value='%PBID%'/><qskey name='pbtype' value='%PBTYPE%'/></linkurl></controlpanels>")
        , new XAttribute("controlPanelRequiresAccount", "1")
        , new XAttribute("defaultSortOrder", "21")
        , new XAttribute("iconcsscoordinate", "-54px,-102px,14px,14px")
        , new XAttribute("isProductOfferedFree", "0")
        , new XAttribute("namespaces", "pg|21,campblazer")
        , new XAttribute("showSetupForManagerOnly", "0")
        , new XAttribute("workspaceLoginXml", "<workspace/>"));

      XElement accordiondata3 = new XElement("accordiondata"
        , new XAttribute("accordionId", 3)
        , new XAttribute("accordionTitle", "Email")
        , new XAttribute("ciExpansion", "55555")
        , new XAttribute("ciRenewNow", "66666")
        , new XAttribute("ciSetup", "77777")
        , new XAttribute("contentXml", "<content accountlist='mya_accountListGetEmail_sp' usercontrol='GetProductList.ascx'/>")
        , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='ECCURL' ci='88888' type='std'><qskey name='rID' value='%RID%'/><qskey name='pID' value='%PID%'/><qskey name='erID' value='%ERID%'/><qskey name='common_name' value='%CN%'/></linkurl></controlpanels>")
        , new XAttribute("controlPanelRequiresAccount", "0")
        , new XAttribute("defaultSortOrder", "4")
        , new XAttribute("iconcsscoordinate", "-34px,-42px,14px,14px")
        , new XAttribute("isProductOfferedFree", "1")
        , new XAttribute("namespaces", "pg|4,email,emailfree,smtprelay,pg|16,emailfwd")
        , new XAttribute("showSetupForManagerOnly", "0")
        , new XAttribute("workspaceLoginXml", "<workspace><linkurl link='SECURESERVERLOGINURL' ci='99999' type='std'><qskey name='apptag' value='wbe'/></linkurl></workspace>"));

      XElement accordiondata4 = new XElement("accordiondata"
        , new XAttribute("accordionId", 2)
        , new XAttribute("accordionTitle", "Domains")
        , new XAttribute("ciExpansion", "98765")
        , new XAttribute("ciRenewNow", "98766")
        , new XAttribute("ciSetup", "98767")
        , new XAttribute("contentXml", "<content accountlist='' usercontrol='domains.ascx'/>")
        , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='DCCURL' ci='98768' type='std'><qskey name='rID' value='%RID%'/><qskey name='pID' value'%PID%'/><qskey name='erID' value='-1'/><qskey name='common_name' value='%CN%'/><qskey name='start' value='%START%'/><qskey name='recurring' value='%RECUR%'/><qskey name='id' value='%ID%'/><qskey name='pbid' value='%PBID%'/><qskey name='pbtype' value='%PBTYPE%'/></linkurl></controlpanels>")
        , new XAttribute("controlPanelRequiresAccount", "0")
        , new XAttribute("defaultSortOrder", "1")
        , new XAttribute("iconcsscoordinate", "-154px,-92px,14px,14px")
        , new XAttribute("isProductOfferedFree", "0")
        , new XAttribute("namespaces", "pg|30,domain")
        , new XAttribute("showSetupForManagerOnly", "0")
        , new XAttribute("workspaceLoginXml", "<workspace/>"));

      root.Add(accordiondata1);
      root.Add(accordiondata2);
      root.Add(accordiondata3);
      root.Add(accordiondata4);

      return root.ToString();
    }
    #endregion
  }
}
