using System;
using System.Collections.Generic;
using System.Data;
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
        var request = (MyaAccordionMetaDataRequestData)requestData;
        IList<AccordionMetaData> metaDataList;

        GetAccordionMetaData(request, config, out metaDataList);

        responseData = new MyaAccordionMetaDataResponseData(metaDataList);
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

    private void GetAccordionMetaData(MyaAccordionMetaDataRequestData request, ConfigElement config, out IList<AccordionMetaData> metaDataList)
    {
      metaDataList = new List<AccordionMetaData>();

      //DataTable accordionDT = DataCache.DataCache.GetMyaAccordionMetadataList();      
      DataTable accordionDT = BuildDebugMetaDataTable();

      foreach (DataRow dr in accordionDT.Rows)
      {
        AccordionMetaData amd = new AccordionMetaData(dr);
        if (amd != null)
        {
          metaDataList.Add(amd);
        }
      }
    }

    #region Debug Data
    private DataTable BuildDebugMetaDataTable()
    {
      DataTable tbl = new DataTable("accordiondata");
      tbl.Columns.Add("accordionId");
      tbl.Columns.Add("accordionTitle");
      tbl.Columns.Add("ciExpansion");
      tbl.Columns.Add("ciRenewNow");
      tbl.Columns.Add("ciSetup");
      tbl.Columns.Add("contentXml");
      tbl.Columns.Add("controlPanelXml");
      tbl.Columns.Add("controlPanelRequiresAccount");
      tbl.Columns.Add("defaultSortOrder");
      tbl.Columns.Add("isProductOfferedFree");
      tbl.Columns.Add("myaUserControl");
      tbl.Columns.Add("namespaces");
      tbl.Columns.Add("showSetupForManagerOnly");
      tbl.Columns.Add("workspaceLoginXml");

      tbl.Rows.Add("1"
        , "Web Hosting"
        , "11111"
        , "22222"
        , "33333"
        , "<content procname='mya_accountListGetHosting_sp'/>"
        , "<controlpanels><linkurl link='HCCURL' ci='44444' type='std'><qskey name='rID' value='%RID%'/><qskey name='pID' value='%PID%'/><qskey name='erID' value='%ERID%'/><qskey name='common_name' value='%CN%'/></linkurl></controlpanels>"
        , "0"
        , "3"
        , "1"
        , "defaultUserControl"
        , "pg|1, Hosting"
        , "0"
        , "<workspace/>");

      tbl.Rows.Add("10"
        , "Express Email Marketing"
        , "12345"
        , "12346"
        , "12347"
        , "<content procname='mya_accountListGetHosting_sp'/>"
        , "<controlpanels><linkurl link='EEMURL' ci='12348' type='std'><qskey name='rID' value='%RID%'/><qskey name='pID' value'%PID%'/><qskey name='erID' value='-1'/><qskey name='common_name' value='%CN%'/><qskey name='start' value='%START%'/><qskey name='recurring' value='%RECUR%'/><qskey name='id' value='%ID%'/><qskey name='pbid' value='%PBID%'/><qskey name='pbtype' value='%PBTYPE%'/></linkurl></controlpanels>"
        , "1"
        , "21"
        , "0"
        , "defaultUserControl"
        , "pg|21, campblazer"
        , "0"
        , "<workspace/>");

      tbl.Rows.Add("3"
        , "Email"
        , "55555"
        , "66666"
        , "77777"
        , "<content procname='mya_accountListGetHosting_sp'/>"
        , "<controlpanels><linkurl link='HCCURL' ci='88888' type='std'><qskey name='rID' value='%RID%'/><qskey name='pID' value='%PID%'/><qskey name='erID' value='%ERID%'/><qskey name='common_name' value='%CN%'/></linkurl></controlpanels>"
        , "0"
        , "4"
        , "1"
        , "defaultUserControl"
        , "pg|4, email"
        , "0"
        , "<workspace><linkurl link='SECURESERVERLOGINURL' ci='99999' type='std'><qskey name='apptag' value='wbe'/></linkurl></workspace>");

      return tbl;
    }
    #endregion
  }
}
