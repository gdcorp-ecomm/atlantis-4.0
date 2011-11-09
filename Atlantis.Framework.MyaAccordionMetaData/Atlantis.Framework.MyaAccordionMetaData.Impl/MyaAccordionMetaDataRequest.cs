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
        string metaDataXml = DataCache.DataCache.GetCacheData("<MyaAccordionMetadata/>");   // = BuildDebugMetaDataXml();
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
    private string BuildDebugMetaDataXml()
    {
      XElement root = new XElement("data",
        new XAttribute("count", "5"));

      XElement accordiondata1 = new XElement("item"
        , new XAttribute("accordionId", 1)
        , new XAttribute("accordionTitle", "Web Hosting")
        , new XAttribute("accordionXml", "<accordion ciexpansion='11111' cirenewnow='22222' cisetup='33333' controlpanelrequiresaccount='false' iconcsscoordinates='-34px,-82px,14px,12px' isproductofferedfree='true' showsetupformanageronly='false' />")
        , new XAttribute("contentXml", "<content><data accountlist='mya_accountListGetHosting_sp' jsonpage='GetProductsContainer.aspx' cioptions='97531'/></content>")
        , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='HCCURL' ci='44444' type='Standard'><qskey name='rID' value='%RID%'/><qskey name='pID' value='%PID%'/><qskey name='erID' value='%ERID%'/><qskey name='common_name' value='%CN%'/></linkurl></controlpanels>")
        , new XAttribute("defaultSortOrder", 3)
        , new XAttribute("namespaces", "pg|1,Hosting,hostingfree")
        , new XAttribute("workspaceLoginXml", "<workspace/>"));

      XElement accordiondata2 = new XElement("item"
        , new XAttribute("accordionId", 10)
        , new XAttribute("accordionTitle", "Express Email Marketing")
        , new XAttribute("accordionXml", "<accordion ciexpansion='12345' cirenewnow='12346' cisetup='' controlpanelrequiresaccount='true' iconcsscoordinates='-54px,-102px,14px,12px' isproductofferedfree='false' showsetupformanageronly='false' />")
        , new XAttribute("contentXml", "<content><data accountlist='mya_accountListGetEEM_sp' jsonpage='GetProductsContainer.aspx' cioptions='97533'/></content>")
        , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='EEMURL' ci='12348' type='Standard'><qskey name='rID' value='%RID%'/><qskey name='pID' value='%PID%'/><qskey name='erID' value='-1'/><qskey name='common_name' value='%CN%'/><qskey name='start' value='%START%'/><qskey name='recurring' value='%RECUR%'/><qskey name='id' value='%ID%'/><qskey name='pbid' value='%PBID%'/><qskey name='pbtype' value='%PBTYPE%'/></linkurl></controlpanels>")
        , new XAttribute("defaultSortOrder", 21)
        , new XAttribute("namespaces", "pg|21,campblazer")
        , new XAttribute("workspaceLoginXml", "<workspace/>"));

      XElement accordiondata3 = new XElement("item"
        , new XAttribute("accordionId", 3)
        , new XAttribute("accordionTitle", "Email")
        , new XAttribute("accordionXml", "<accordion ciexpansion='55555' cirenewnow='66666' cisetup='77777' controlpanelrequiresaccount='false' iconcsscoordinates='-34px,-42px,14px,12px' isproductofferedfree='true' showsetupformanageronly='false' />")
        , new XAttribute("contentXml", "<content><data accountlist='mya_accountListGetEmailAllTypes_sp' jsonpage='GetProductsContainer.aspx' cioptions='97535'/></content>")
        , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='ECCURL' ci='88888' type='Standard'><qskey name='rID' value='%RID%'/><qskey name='pID' value='%PID%'/><qskey name='erID' value='%ERID%'/><qskey name='common_name' value='%CN%'/></linkurl></controlpanels>")
        , new XAttribute("defaultSortOrder", 4)
        , new XAttribute("namespaces", "pg|4,email,emailfree,smtprelay,pg|16,emailfwd")
        , new XAttribute("workspaceLoginXml", "<workspace><linkurl link='SECURESERVERLOGINURL' ci='99999' type='Standard'><qskey name='apptag' value='wbe'/></linkurl></workspace>"));

      XElement accordiondata4 = new XElement("item"
        , new XAttribute("accordionId", 2)
        , new XAttribute("accordionTitle", "Domains")
        , new XAttribute("accordionXml", "<accordion ciexpansion='98765' cirenewnow='98766' cisetup='98767' controlpanelrequiresaccount='false' iconcsscoordinates='-154px,-92px,14px,12px' isproductofferedfree='false' showsetupformanageronly='false' />")
        , new XAttribute("contentXml", "<content><data accountlist='' jsonpage='domains.aspx' cioptions=''/></content>")
        , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='DCCURL' ci='98768' type='Standard'><qskey name='rID' value='%RID%'/><qskey name='pID' value='%PID%'/><qskey name='erID' value='-1'/><qskey name='common_name' value='%CN%'/></linkurl><linkurl link='DCCMGRURL' ci='98768' type='Manager'><qskey name='rID' value='%RID%'/><qskey name='pID' value='%PID%'/><qskey name='erID' value='-1'/><qskey name='common_name' value='%CN%'/></linkurl></controlpanels>")
        , new XAttribute("defaultSortOrder", 1)
        , new XAttribute("namespaces", "pg|30,domain")
        , new XAttribute("workspaceLoginXml", "<workspace/>"));

      XElement accordiondata5 = new XElement("item"
        , new XAttribute("accordionId", 5)
        , new XAttribute("accordionTitle", "Domains By Proxy")
        , new XAttribute("accordionXml", "<accordion ciexpansion='' cirenewnow='58766' cisetup='' controlpanelrequiresaccount='false' iconcsscoordinates='-104px,-22px,14px,12px' isproductofferedfree='false' showsetupformanageronly='false' />")
        , new XAttribute("contentXml", "<content/>")
        , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='DBPURL' ci='58768' type='Standard'><qskey name='rID' value='%RID%'/><qskey name='pID' value='%PID%'/><qskey name='erID' value='-1'/><qskey name='common_name' value='%CN%'/></linkurl></controlpanels>")
        , new XAttribute("defaultSortOrder", 19)
        , new XAttribute("namespaces", "pg|9, dbp")
        , new XAttribute("workspaceLoginXml", "<workspace/>"));

      root.Add(accordiondata1);
      root.Add(accordiondata2);
      root.Add(accordiondata3);
      root.Add(accordiondata4);
      root.Add(accordiondata5);

      return root.ToString();
    }
    #endregion
  }
}
