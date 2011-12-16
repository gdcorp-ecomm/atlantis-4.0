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
        new XAttribute("count", "7"));

      XElement accordiondata1 = new XElement("item"
        , new XAttribute("accordionId", 1)
        , new XAttribute("accordionTitle", "Web Hosting")
        , new XAttribute("accordionXml", "<accordion ciexpansion='11111' cirenewnow='22222' cisetup='33333' controlpanelrequiresaccount='false' iconcsscoordinates='-34px,-82px,14px,12px' showsetupformanageronly='false' productgroup='1' cmsdisplaygroupidlist='191' orionproductname='sharedhosting'/>")
        , new XAttribute("contentXml", "<content><data accountlist='mya_accountListGetHosting_sp' jsonpage='GetProductsContainer.aspx' cioptions='97531'/></content>")
        , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='HCCURL' page='accountlist.aspx' type='Standard'><qskey name='ci' value='54043'/><qskey name='account_uid' value='%ERID%'/><qskey name='common_name' value='%CN%'/><qskey name='auth_guid' value='%AUTHGUID%'/></linkurl></controlpanels>")
        , new XAttribute("defaultSortOrder", 3)
        , new XAttribute("namespaces", "pg|1,Hosting,hostingfree")
        , new XAttribute("workspaceLoginXml", "<workspace/>"));

      XElement accordiondata2 = new XElement("item"
        , new XAttribute("accordionId", 10)
        , new XAttribute("accordionTitle", "Express Email Marketing")
        , new XAttribute("accordionXml", "<accordion ciexpansion='54112' cirenewnow='54114' cisetup='54115' controlpanelrequiresaccount='true' iconcsscoordinates='186px,31px,30px,30px' showsetupformanageronly='false' productgroup='21' cmsdisplaygroupidlist='199'/>")
        , new XAttribute("contentXml", "<content><data accountlist='mya_accountListGetCampaignBlazer_sp' jsonpage='GetProductsContainer.aspx' cioptions='54113'/></content>")
        , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='EEMURL' type='Standard'><qskey name='ci' value='54111'/><qskey name='rID' value='%RID%'/><qskey name='pID' value='%PID%'/><qskey name='erID' value='%ERID%'/><qskey name='common_name' value='%CN%'/><qskey name='start' value='%START%'/><qskey name='recurring' value='%RECUR%'/><qskey name='id' value='%ID%'/><qskey name='pbid' value='%PBID%'/><qskey name='pbtype' value='%PBTYPE%'/></linkurl></controlpanels>")
        , new XAttribute("defaultSortOrder", 21)
        , new XAttribute("namespaces", "pg|21,campblazer")
        , new XAttribute("workspaceLoginXml", "<workspace/>"));

      XElement accordiondata3 = new XElement("item"
        , new XAttribute("accordionId", 3)
        , new XAttribute("accordionTitle", "Email")
        , new XAttribute("accordionXml", "<accordion ciexpansion='55555' cirenewnow='66666' cisetup='77777' controlpanelrequiresaccount='false' iconcsscoordinates='-34px,-42px,14px,12px' showsetupformanageronly='false' productgroup='4' cmsdisplaygroupidlist='187,193'/>")
        , new XAttribute("contentXml", "<content><data accountlist='mya_accountListGetEmailAllTypes_sp' jsonpage='GetProductsContainer.aspx' cioptions='97535'/></content>")
        , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='ECCURL' page='ecc.php' type='Standard' identificationrule='ProductTypeId' identificationvalue='16'><qskey name='ci' value='54048'/><qskey name='cmd' value='planlistemail'/><qskey name='erid' value='%ERID%'/><qskey name='prog_id' value='%PROGID%'/></linkurl><linkurl link='ECCURL' page='manager/redirect.php' type='Manager' identificationrule='ProductTypeId' identificationvalue='16'><qskey name='ci' value='54048'/><qskey name='cmd' value='planlistemail'/><qskey name='erid' value='%ERID%'/><qskey name='prog_id' value='%PROGID%'/><qskey name='mstk' value='%MSTK%'/></linkurl><linkurl link='ECCURL' page='ecc.php' type='Standard' identificationrule='ProductTypeId' identificationvalue='69'><qskey name='ci' value='54048'/><qskey name='cmd' value='planlistforward'/><qskey name='erid' value='%ERID%'/><qskey name='prog_id' value='%PROGID%'/></linkurl><linkurl link='ECCURL' page='manager/redirect.php' type='Manager' identificationrule='ProductTypeId' identificationvalue='69'><qskey name='ci' value='54048'/><qskey name='cmd' value='planlistforward'/><qskey name='erid' value='%ERID%'/><qskey name='prog_id' value='%PROGID%'/><qskey name='mstk' value='%MSTK%'/></linkurl></controlpanels>")
        , new XAttribute("defaultSortOrder", 4)
        , new XAttribute("namespaces", "pg|4,email,emailfree,smtprelay,pg|16,emailfwd")
        , new XAttribute("workspaceLoginXml", "<workspace buttontext='Webmail Login'><linkurl link='SECURESERVERLOGINURL' type='Standard'><qskey name='ci' value='54053'/><qskey name='app' value='wbe'/></linkurl></workspace>"));

      XElement accordiondata4 = new XElement("item"
        , new XAttribute("accordionId", 2)
        , new XAttribute("accordionTitle", "Domains")
        , new XAttribute("accordionXml", "<accordion ciexpansion='98765' cirenewnow='98766' controlpanelrequiresaccount='false' iconcsscoordinates='-154px,-92px,14px,12px' showsetupformanageronly='false' productgroup='30' cmsdisplaygroupidlist='186,189,190'/>")
        , new XAttribute("contentXml", "<content><data jsonpage='GetDomainsContainer.aspx'><linkurl link='SITEURL' page='blah.aspx' type='Standard'><qskey name='ci' value='99999'/></linkurl></data></content>")
        , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='DCCURL' page='domaindetails.aspx' type='Standard'><qskey name='ci' value='54036'/><qskey name='domain' value='%DOMAINID%'/></linkurl><linkurl link='MANAGERDCCURL' page='domaindetails.aspx' type='Manager'><qskey name='ci' value='54036'/><qskey name='domain' value='%DOMAINID%'/></linkurl></controlpanels>")
        , new XAttribute("defaultSortOrder", 1)
        , new XAttribute("namespaces", "pg|30,domain")
        , new XAttribute("workspaceLoginXml", "<workspace/>"));

      XElement accordiondata5 = new XElement("item"
        , new XAttribute("accordionId", 5)
        , new XAttribute("accordionTitle", "Domains By Proxy")
        , new XAttribute("accordionXml", "<accordion cirenewnow='58766' controlpanelrequiresaccount='false' iconcsscoordinates='-104px,-22px,14px,12px' showsetupformanageronly='false' productgroup='9'/>")
        , new XAttribute("contentXml", "<content/>")
        , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='DBPURL' page='Login.aspx' type='Standard' isenvsecure='Test,Prod'><qskey name='ci' value='54105'/></linkurl></controlpanels>")
        , new XAttribute("defaultSortOrder", 19)
        , new XAttribute("namespaces", "pg|9, dbp")
        , new XAttribute("workspaceLoginXml", "<workspace/>"));

      XElement accordiondata6 = new XElement("item"
        , new XAttribute("accordionId", 6)
        , new XAttribute("accordionTitle", "Website Tonight&#174; / InstantPage&#8482;")
        , new XAttribute("accordionXml", "<accordion ciexpansion='54059' cirenewnow='54061' cisetup='54062' controlpanelrequiresaccount='true' iconcsscoordinates='155px,93px,30px,30px' showsetupformanageronly='false' productgroup='15' cmsdisplaygroupidlist='192'/>")
        , new XAttribute("contentXml", "<content><data accountlist='mya_accountListGetHostingWST_sp' jsonpage='GetProductsContainer.aspx' cioptions='54060'/></content>")
        , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='WSTURL' page='launch.aspx' type='Standard' identificationrule='UnifiedProductId' identificationvalue=''><qskey name='ci' value='54058'/><qskey name='account_Uid' value='%ERID%'/><qskey name='common_name' value='%CN%'/></linkurl><linkurl link='IPURL' page='sso/mya' type='Standard' identificationrule='UnifiedProductId' identificationvalue='70761'><qskey name='ci' value='54058' /><qskey name='account_Uid' value='%ERID%'/><qskey name='common_name' value='%CN%'/></linkurl><linkurl link='IPMGRURL' page='manager/login.ashx' type='Manager' identificationrule='UnifiedProductId' identificationvalue='70761'><qskey name='ci' value='54058'/><qskey name='uid' value='%ERID%'/></linkurl></controlpanels>")
        , new XAttribute("defaultSortOrder", 6 )
        , new XAttribute("namespaces", "pg|15,wst")
        , new XAttribute("workspaceLoginXml", "<workspace/>"));

      XElement accordiondata7 = new XElement("item"
        , new XAttribute("accordionId", 8)
        , new XAttribute("accordionTitle", "Business Accelerator")
        , new XAttribute("accordionXml", "<accordion ciexpansion='54068' cirenewnow='54070' controlpanelrequiresaccount='true' iconcsscoordinates='31px,0px,30px,30px' showsetupformanageronly='false' productgroup='81' isbundle='true'/>")
        , new XAttribute("contentXml", "<content><data accountlist='mya_accountListGetBusinessAcceleratorBundles_sp' jsonpage='GetProductsContainer.aspx' cioptions='54069'/></content>")
        , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='LOGOURL' type='Standard' identificationrule='ProductTypeId' identificationvalue='187'><qskey name='ci' value='54095'/></linkurl><linkurl link='HCCURL' page='accountlist.aspx' type='Standard' identificationrule='ProductTypeId' identificationvalue='307'><qskey name='ci' value='54095'/><qskey name='account_uid' value='%ERID%'/><qskey name='common_name' value='%CN%'/><qskey name='auth_guid' value='%AUTHGUID%'/></linkurl><linkurl link='WSTURL' page='launch.aspx' type='Standard' identificationrule='ProductTypeId' identificationvalue='130'><qskey name='ci' value='54058'/><qskey name='account_Uid' value='%ERID%'/><qskey name='common_name' value='%CN%'/></linkurl></controlpanels>")
        , new XAttribute("defaultSortOrder", 8)
        , new XAttribute("namespaces", "busaccl,free_busaccl,pg|81")
        , new XAttribute("workspaceLoginXml", "<workspace/>"));

      root.Add(accordiondata1);
      root.Add(accordiondata2);
      root.Add(accordiondata3);
      root.Add(accordiondata4);
      root.Add(accordiondata5);
      root.Add(accordiondata6);
      root.Add(accordiondata7);

      return root.ToString();
    }
    #endregion
  }
}
