using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaAccountList.Interface;
using Atlantis.Framework.Nimitz;

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
      catch (AtlantisException exAtlantis)
      {
        responseData = new MyaAccountListResponseData(exAtlantis);
      }
      catch (Exception ex)
        {
        responseData = new MyaAccountListResponseData(requestData, ex);
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
      string connectionString = NetConnect.LookupConnectInfo(config.GetConfigValue("TB_DataSourceName"), config.GetConfigValue("CertificateName"), config.GetConfigValue("ApplicationName"), "::UpdateSEVData", ConnectLookupType.NetConnectionString);

      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        using (SqlCommand command = new SqlCommand("dbo.tba_userwebsiteStatusGetForMYA_sp", connection))
        {
          command.CommandType = CommandType.StoredProcedure;
          command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
          command.Parameters.Add(new SqlParameter("@shopper_id", request.ShopperID));
          connection.Open();

          using (SqlDataReader dr = command.ExecuteReader())
          {
            while (dr.Read())
            {
              foreach (DataRow row in ds.Tables[0].Rows)
              {
                if (row["resource_id"].Equals(dr["recurring_id"]))
                {
                  row["externalResourceID"] = dr["userwebsite_id"] == DBNull.Value ? "new" : dr["userwebsite_id"];
                  row["commonName"] = dr["websiteurl"] == DBNull.Value ? "New Account" : dr["websiteurl"].ToString().Trim();
                  break;
                }
              }
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
      using (CampaignBlazerWS.CampaignBlazer eemWs = new CampaignBlazerWS.CampaignBlazer())
      {
        eemWs.Url = ((WsConfigElement)config).WSURL;
        eemWs.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
        string xmlData = eemWs.GetCustomerSummary(GetCustomerXml(ds));

        if (!string.IsNullOrWhiteSpace(xmlData))
        {
          XDocument xDoc = new XDocument();
          xDoc = XDocument.Parse(xmlData);

          foreach (DataRow row in ds.Tables[0].Rows)
          {
            if (row["commonName"].ToString().ToLowerInvariant() != "new account")
            {
              XElement sourceOfChangeElement = (from c in xDoc.Element("Customers").Elements("Customer")
                                                where c.Element("customer_id").Value == row["commonName"].ToString()
                                                select c).Single().Element("company_name");
              if (sourceOfChangeElement == null)
              {
                row["commonName"] = "New Account";
              }
              else
              {
                row["commonName"] = (sourceOfChangeElement != null && !string.IsNullOrWhiteSpace(sourceOfChangeElement.Value)) ? sourceOfChangeElement.Value : "New Account";
              }
            }
          }
        }
      }

      return ds;
    }

    private string GetCustomerXml(DataSet ds)
    {
      XElement customers = new XElement("Customers");
      foreach (DataRow row in ds.Tables[0].Rows)
      {
        if (row["externalResourceID"] != DBNull.Value)
        {
          XElement customer = new XElement("Customer",
            new XElement("customer_id", row["externalResourceID"]));
          customers.Add(customer);
        }
      }
      return customers.ToString();
    }
    #endregion
    #endregion
    
    #endregion
  }
}
