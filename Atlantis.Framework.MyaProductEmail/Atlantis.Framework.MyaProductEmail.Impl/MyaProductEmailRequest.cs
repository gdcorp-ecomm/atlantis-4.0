using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaProduct.Interface.Enums;
using Atlantis.Framework.MyaProductEmail.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.MyaProductEmail.Impl
{
  public class MyaProductEmailRequest : IRequest
  {
    private const string EXPIRATION_DATE_SORT_COLUMN = "account_expiration_date";
    private const string COMMON_NAME_SORT_COLUMN = "commonName";

    #region Parameters

    private const string STORED_PROCEDURE = "dbo.mya_getActiveUnifiedListbyProductTypeList_sp";
    private const string SHOPPER_ID_PARAM = "@shopper_id";
    private const string PRODUCT_TYPE_ID_PARAM = "@product_typeIDList";
    private const string PAGE_NUMBER_PARAM = "@pageno";
    private const string ROWS_PER_PAGE_PARAM = "@rowsperpage";
    private const string SORT_COLUMN_PARAM = "@sortcol";
    private const string SORT_DIR_PARAM = "@sortdir";
    private const string RETURN_ALL_PARAM = "@returnAllFlag";
    private const string TOTAL_RECORDS_OUT_PARAM = "@totalrecords";
    private const string TOTAL_PAGES_OUT_PARAM = "@totalpages";

    #endregion

    private const string PRODUCT_TYPE_ID = "16";

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      MyaProductEmailRequestData requestData = null;
      MyaProductEmailResponseData responseData;

      try
      {
        requestData = (MyaProductEmailRequestData)oRequestData;

        using (SqlConnection connection = new SqlConnection(LookupConnectionString(oConfig)))
        {
          using (SqlCommand command = new SqlCommand(STORED_PROCEDURE, connection))
          {
            command.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(SHOPPER_ID_PARAM, requestData.ShopperID));
            command.Parameters.Add(new SqlParameter(PRODUCT_TYPE_ID_PARAM, PRODUCT_TYPE_ID));
            command.Parameters.Add(new SqlParameter(SORT_COLUMN_PARAM, GetSortColumnValue(requestData.SortColumn)));
            command.Parameters.Add(new SqlParameter(SORT_DIR_PARAM, GetSortDirectionValue(requestData.SortDirection)));
            command.Parameters.Add(new SqlParameter(RETURN_ALL_PARAM, requestData.PagingInfo.ReturnAll));

            if (!requestData.PagingInfo.ReturnAll)
            {
              command.Parameters.Add(new SqlParameter(PAGE_NUMBER_PARAM, requestData.PagingInfo.CurrentPage));
              command.Parameters.Add(new SqlParameter(ROWS_PER_PAGE_PARAM, requestData.PagingInfo.RowsPerPage));
            }

            SqlParameter totalPagesParam = new SqlParameter(TOTAL_PAGES_OUT_PARAM, SqlDbType.Int);
            totalPagesParam.Direction = ParameterDirection.Output;
            command.Parameters.Add(totalPagesParam);

            SqlParameter totalRecordsParam = new SqlParameter(TOTAL_RECORDS_OUT_PARAM, SqlDbType.Int);
            totalRecordsParam.Direction = ParameterDirection.Output;
            command.Parameters.Add(totalRecordsParam);

            connection.Open();

            IList<EmailProduct> sharedHostingProductList = new List<EmailProduct>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
              if (reader != null && reader.HasRows)
              {
                while (reader.Read())
                {
                  EmailProduct sharedHostingProduct = GetEmailProduct(reader, requestData);
                  if (sharedHostingProduct != null)
                  {
                    sharedHostingProductList.Add(sharedHostingProduct);
                  }
                }
              }
            }

            int totalRecords = 0;
            int totalPages = 0;

            if (totalRecordsParam.Value != null)
            {
              totalRecords = (int)totalRecordsParam.Value;
            }

            if (totalPagesParam.Value != null)
            {
              totalPages = (int)totalPagesParam.Value;
            }

            responseData = new MyaProductEmailResponseData(sharedHostingProductList, totalRecords, totalPages);
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new MyaProductEmailResponseData(requestData, ex);
      }

      return responseData;
    }

    private static EmailProduct GetEmailProduct(SqlDataReader reader, MyaProductEmailRequestData requestData)
    {
      EmailProduct sharedHostingProduct = null;

      if (reader.FieldCount > 0)
      {
        IDictionary<string, object> productProperties = new Dictionary<string, object>();

        for (int i = 0; i < reader.FieldCount; i++)
        {
          if (!productProperties.ContainsKey(reader.GetName(i)))
          {
            productProperties.Add(reader.GetName(i), reader.GetValue(i));
          }
        }

        sharedHostingProduct = new EmailProduct(requestData.PrivateLabelId, productProperties);
      }

      return sharedHostingProduct;
    }

    private static string LookupConnectionString(ConfigElement config)
    {
      string result = NetConnect.LookupConnectInfo(config.GetConfigValue("DataSourceName"),
                                                   config.GetConfigValue("CertificateName"),
                                                   config.GetConfigValue("ApplicationName"),
                                                   "MyaProductEmailRequest.LookupConnectionString", ConnectLookupType.NetConnectionString);

      if (string.IsNullOrEmpty(result) || result.Length <= 2)
      {
        throw new Exception("Invalid Connection String");
      }

      return result;
    }

    private static string GetSortDirectionValue(SortDirectionType sortDirectionType)
    {
      string sortDirection = string.Empty;

      switch (sortDirectionType)
      {
        case SortDirectionType.Ascending:
          sortDirection = "ASC";
          break;
        case SortDirectionType.Descending:
          sortDirection = "DESC";
          break;
      }

      return sortDirection;
    }

    private static string GetSortColumnValue(MyaProductEmailRequestData.SortColumnType sortColumnType)
    {
      string sortColumn = string.Empty;

      switch (sortColumnType)
      {
        case MyaProductEmailRequestData.SortColumnType.AccountExpirationDate:
          sortColumn = EXPIRATION_DATE_SORT_COLUMN;
          break;
        case MyaProductEmailRequestData.SortColumnType.CommonName:
          sortColumn = COMMON_NAME_SORT_COLUMN;
          break;
      }

      return sortColumn;
    }
  }
}
