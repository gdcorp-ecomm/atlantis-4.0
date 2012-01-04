using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.EEMGetCustomerSummary.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaAccountList.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.SEVGetWebsiteId.Interface;

namespace Atlantis.Framework.MyaAccountList.Impl
{
  public class MyaAccountListRequest : IRequest
  {
    #region Implementation of IRequest
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      MyaAccountListRequestData request = (MyaAccountListRequestData)requestData;

      DataSet ds = null;

      try
      {
        int totalRecords;
        int totalPages;
        ds = GetAccountList(config, request, ds, out totalRecords, out totalPages);

        if (request.AccordionData.AccordionId.Equals(MyaAccountListRequestData.AccordionIds.SearchEngineVisibility))
        {
          ds = UpdateSEVAccountListData(request, config, ds);
        }
        else if (request.AccordionData.AccordionId.Equals(MyaAccountListRequestData.AccordionIds.ExpressEmailMarketing))
        {
          ds = UpdateEEMAccountListData(request, config, ds);
        }

        responseData = new MyaAccountListResponseData(ds, totalPages, totalRecords);
      }
      catch (Exception ex)
      {
        responseData = new MyaAccountListResponseData(request, ex);
      }

      return responseData;
    }

    #region Get Account List Data

    #region Base Account List Data
    private DataSet GetAccountList(ConfigElement config, MyaAccountListRequestData request, DataSet ds, out int totalRecords, out int totalPages)
    {
      string connectionString = NetConnect.LookupConnectInfo(config);

      /*
       dbo.mya_accountListGetHosting_sp  
      ( @shopper_id  varchar(10),  
       @pageno   int = 1,  
       @rowsperpage  int = 10,  
       @sortcol  varchar(132) = 'commonName',  
       @sortdir  varchar(5) = 'ASC',  
       @returnAllFlag  bit = 0,  
       @daysTillExpiration int = NULL,  
       @free   int = NULL,  
       @commonNameFilter nvarchar(600) = NULL,  
       @totalrecords  int OUTPUT,  
       @totalpages  int OUTPUT  
       */
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        using (SqlCommand command = new SqlCommand(request.StoredProcName, connection))
        {
          command.CommandType = CommandType.StoredProcedure;
          command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
          command.Parameters.Add(new SqlParameter("@shopper_id", request.ShopperID));
          command.Parameters.Add(new SqlParameter("@sortdir", request.SortDirection));
          command.Parameters.Add(new SqlParameter("@pageno", request.PageInfo.CurrentPage));
          if (request.PageInfo.PageSize > 0)
          {
            command.Parameters.Add(new SqlParameter("@rowsperpage", request.PageInfo.PageSize));
          }
          if (request.Filter != null)
          {
            command.Parameters.Add(new SqlParameter("@commonNameFilter", request.Filter));
          }
          if (request.SortColumn != null)
          {
            command.Parameters.Add(new SqlParameter("@sortcol", request.SortColumn));
          }
          if (request.ReturnAll != 0)
          {
            command.Parameters.Add(new SqlParameter("@returnAllFlag", 1));
          }
          if (request.ReturnFreeListOnly != 0)
          {
            command.Parameters.Add(new SqlParameter("@free", 1));
          }
          if (request.DaysTillExpiration != null)
          {
            command.Parameters.Add(new SqlParameter("@daysTillExpiration", request.DaysTillExpiration));
          }

          SqlParameter prmTotalRecords = command.Parameters.Add("@totalrecords", SqlDbType.Int, 4);
          prmTotalRecords.Direction = ParameterDirection.Output;

          SqlParameter prmTotalPages = command.Parameters.Add("@totalpages", SqlDbType.Int, 4);
          prmTotalPages.Direction = ParameterDirection.Output;

          connection.Open();
          ds = new DataSet(Guid.NewGuid().ToString());
          SqlDataAdapter adp = new SqlDataAdapter(command);
          adp.Fill(ds);

          totalRecords = Convert.ToInt32(prmTotalRecords.Value.ToString());
          totalPages = Convert.ToInt32(prmTotalPages.Value.ToString());
        }
      }
      return ds;
    }
    #endregion

    #region Special Search Engine Visibility Functionality
    /// <summary>
    /// Search Engine Visibility requires data not available in Billing.  Update DataSet with results from TrafficBlazer App Proc
    /// </summary>
    /// <param name="request"></param>
    /// <param name="config"></param>
    /// <param name="ds"></param>
    /// <returns></returns>
    private DataSet UpdateSEVAccountListData(MyaAccountListRequestData request, ConfigElement config, DataSet ds)
    {      
      var sevRequest = new SEVGetWebsiteIdRequestData(request.ShopperID
        , request.SourceURL
        , request.OrderID
        , request.Pathway
        , request.PageCount);

      int requestType = request.OverrideSEVWebsiteIdRequestType.HasValue ? request.OverrideSEVWebsiteIdRequestType.Value : sevRequest.SEVGetWebsiteIdRequestType;
      var response = Engine.Engine.ProcessRequest(sevRequest, requestType) as SEVGetWebsiteIdResponseData;

      if (response.IsSuccess)
      {
        if (response.ReplacementDataDictionary.Count > 0)
        {
          foreach (DataRow row in ds.Tables[0].Rows)
          {
            if (row["gdshop_product_typeID"].ToString() == "39")
            {
              try
              {
                SEVReplacementData srd;
                response.ReplacementDataDictionary.TryGetValue(Convert.ToInt32(row["resource_id"]), out srd);
                if (srd == null)
                {
                  row["externalResourceID"] = "new";
                }
                else
                {
                  row["externalResourceID"] = srd.UserWebsiteId;
                  row["commonName"] = srd.WebsiteUrl;
                }
              }
              catch (Exception ex)
              {
                throw new AtlantisException(sevRequest, "MyaAccountListRequest::UpdateSEVAccountListData", ex.Message, ex.StackTrace);
              }
            }
          }
        }
        else
        {
          foreach (DataRow row in ds.Tables[0].Rows)
          {
            if (row["gdshop_product_typeID"].ToString() == "39")
            {
              row["externalResourceID"] = "new";
            }
          }
        }
      }
      return ds;
    }
    #endregion

    #region Special Express Email Marketing Functionality
    /// <summary>
    /// Express Email Marketing CommonName data not correct in Billing.  Update DataSet with results from CampaignBlazer WS
    /// </summary>
    /// <param name="request"></param>
    /// <param name="config"></param>
    /// <param name="ds"></param>
    /// <returns></returns>
    private DataSet UpdateEEMAccountListData(MyaAccountListRequestData request, ConfigElement config, DataSet ds)
    {
      List<int> customerIds = new List<int>();

      foreach (DataRow row in ds.Tables[0].Rows)
      {
        if (row["externalResourceID"] != DBNull.Value)
        {
          customerIds.Add(Convert.ToInt32(row["externalResourceID"]));
        }        
      }

      var eemRequest = new EEMGetCustomerSummaryRequestData(request.ShopperID
        , request.SourceURL
        , request.OrderID
        , request.Pathway
        , request.PageCount
        , customerIds);

      int requestType = request.OverrideEEMGetCustomerSummaryRequestType.HasValue ? request.OverrideEEMGetCustomerSummaryRequestType.Value : eemRequest.EEMGetCustomerSummaryRequestType;
      var response = Engine.Engine.ProcessRequest(eemRequest, requestType) as EEMGetCustomerSummaryResponseData;

      if (response.IsSuccess)
      {
        if (response.ReplacementDataDictionary.Count > 0)
        {
          foreach (DataRow row in ds.Tables[0].Rows)
          {
            try
            {
              if (row["commonName"].ToString().ToLowerInvariant() != "new account")
              {
                EEMCustomerSummary erd;
                response.ReplacementDataDictionary.TryGetValue(Convert.ToInt32(row["externalResourceID"]), out erd);

                if (erd == null)
                {
                  row["commonName"] = "New Account";
                }
                else
                {
                  row["commonName"] = erd.companyName;
                }
              }
            }
            catch (Exception ex)
            {
              throw new AtlantisException(eemRequest, "MyaAccountListRequest::UpdateEEMAccountListData", ex.Message, ex.StackTrace);
            }
          }
        }
      }

      return ds;
    }
    #endregion
    #endregion
    
    #endregion
  }
}
