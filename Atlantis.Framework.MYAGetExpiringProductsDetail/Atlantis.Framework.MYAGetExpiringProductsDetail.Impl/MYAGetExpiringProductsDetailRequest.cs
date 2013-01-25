using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MYAGetExpiringProductsDetail.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.MYAGetExpiringProductsDetail.Impl
{
  public class MYAGetExpiringProductsDetailRequest : IRequest
  {
    private const string PROC_NAME = "mya_getExpiringProductsDetailGet_sp";
    private const string SHOPPER_ID_PARAM = "shopper_id";
    private const string DAYS_PARAM = "days";
    private const string PAGE_NO_PARAM = "pageno";
    private const string ROWS_PER_PAGE_PARAM = "rowsperpage";
    private const string SORT_XML_PARAM = "sortXML";
    private const string RETURN_ALL_FLAG_PARAM = "returnAllFlag";
    private const string SYNCABLE_ONLY_PARAM = "syncAbleOnly";
    private const string ISC_DATE_PARAM = "iscDate";
    private const string TOTAL_RECORDS_PARAM = "totalrecords";
    private const string TOTAL_PAGES_PARAM = "totalpages";
    private const string PRODUCT_TYPE_ID_LIST = "product_typeIDList";

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      DataSet dataSet = null;

      try
      {
        MYAGetExpiringProductsDetailRequestData request = (MYAGetExpiringProductsDetailRequestData)oRequestData;

        string connectionString = LookupConnectionString(request, oConfig);
        int totalRecords;
        int totalPages;
        List<RenewingProductObject> productList;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROC_NAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(SHOPPER_ID_PARAM, request.ShopperID));
            command.Parameters.Add(new SqlParameter(DAYS_PARAM, request.Days));
            command.Parameters.Add(new SqlParameter(PAGE_NO_PARAM, request.PageNumber));
            command.Parameters.Add(new SqlParameter(ROWS_PER_PAGE_PARAM, request.RowsPerPage));
            command.Parameters.Add(new SqlParameter(SORT_XML_PARAM, request.SortXml));
            command.Parameters.Add(new SqlParameter(RETURN_ALL_FLAG_PARAM, request.ReturnAll));
            command.Parameters.Add(new SqlParameter(SYNCABLE_ONLY_PARAM, request.SyncableOnly));
            command.Parameters.Add(new SqlParameter(ISC_DATE_PARAM, request.IscDate));
            if (!string.IsNullOrEmpty(request.ProductTypeListString))
            {
              ProductTypeListSizeCheck(request);
              command.Parameters.Add(new SqlParameter(PRODUCT_TYPE_ID_LIST, request.ProductTypeListString));
            }

            command.Parameters.Add(new SqlParameter(TOTAL_RECORDS_PARAM, SqlDbType.Int)).Direction = ParameterDirection.Output;
            command.Parameters.Add(new SqlParameter(TOTAL_PAGES_PARAM, SqlDbType.Int)).Direction = ParameterDirection.Output;

            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;

            
            dataSet = new DataSet(Guid.NewGuid().ToString());

            connection.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            sqlDataAdapter.Fill(dataSet);

            totalRecords = (int)command.Parameters[TOTAL_RECORDS_PARAM].Value;
            totalPages = (int)command.Parameters[TOTAL_PAGES_PARAM].Value;

            productList = GetObjectListFromDataset(dataSet);
          }
        }

        oResponseData = new MYAGetExpiringProductsDetailResponseData(dataSet, productList, totalRecords, totalPages);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new MYAGetExpiringProductsDetailResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new MYAGetExpiringProductsDetailResponseData(dataSet, oRequestData, ex);
      }

      return oResponseData;
    }

    private void ProductTypeListSizeCheck(MYAGetExpiringProductsDetailRequestData request)
    {
      if (request.ProductTypeHashSet.Count > 5 && DateTime.Now.Minute == 0)
      {
        try
        {
          Engine.Engine.LogAtlantisException(new AtlantisException(request,
                                                                   "MYAGetExpiringProductsDetailRequest.ProductTypeListSizeCheck",
                                                                   "1",
                                                                   "MYAGetExpiringProductsDetailRequestData.ProductTypeHashSet count was larger than 5.  If you intended to get all products, leave this HashSet empty.",
                                                                   "ProductTypeHashSet: " + request.ProductTypeListString, 
                                                                   IPAddress.Loopback.ToString()));
        }
        catch{}
      }
    }

    private string LookupConnectionString(MYAGetExpiringProductsDetailRequestData request, ConfigElement config)
    {
      string dataSource = config.GetConfigValue("DataSourceName");
      string applicationName = config.GetConfigValue("ApplicationName");
      string certificateName = config.GetConfigValue("CertificateName");

      string result = string.Empty;
      if (!string.IsNullOrEmpty(dataSource) && !string.IsNullOrEmpty(applicationName) && !string.IsNullOrEmpty(certificateName))
      {
        result = NetConnect.LookupConnectInfo(dataSource, certificateName, applicationName, "MYAGetExpiringProductsDetailRequest.LookupConnectionString", ConnectLookupType.NetConnectionString);
      }

      //when an error occurs a ';' is returned not a valid connection string or empty
      if (result.Length <= 1)
      {
        throw new AtlantisException(request, "LookupConnectionString",
                "Database connection string lookup failed", "No ConnectionFound For:"
                + dataSource + ":"
                + applicationName
                + ":" + certificateName);
      }

      return result;
    }

    private int? ParseDataRow(object rowValue, int? defaultValue)
    {
      int? value;

      if (rowValue is DBNull)
      {
        value = defaultValue;
      }
      else
      {
        value = Convert.ToInt32(rowValue);
      }

      return value;
    }

    private bool? ParseDataRow(object rowValue, bool? defaultValue)
    {
      bool? value = defaultValue;

      if (rowValue is Int32)
      {
        value = Convert.ToInt32(rowValue) == 1;
      }
      else if (rowValue is Boolean)
      {
        value = Convert.ToBoolean(rowValue);
      }
      else if (rowValue is Byte)
      {
        value = Convert.ToByte(rowValue) == 1;
      }

      return value;
    }

    private string ParseDataRow(object rowValue, string defaultValue)
    {
      string value;

      if (rowValue is DBNull)
      {
        value = defaultValue;
      }
      else
      {
        value = Convert.ToString(rowValue);
      }

      return value;
    }

    private DateTime? ParseDataRow(object rowValue, DateTime? defaultValue)
    {
      DateTime? value;

      if (rowValue is DBNull)
      {
        value = defaultValue;
      }
      else
      {
        value = Convert.ToDateTime(rowValue);
      }

      return value;
    }

    private List<RenewingProductObject> GetObjectListFromDataset(DataSet ds)
    {      
      List<RenewingProductObject> productList = new List<RenewingProductObject>();
      foreach (DataRow row in ds.Tables[0].Rows)
      {
        RenewingProductObject product = new RenewingProductObject();

        product.Id = ParseDataRow(row["id"], (int?) null);
        product.AutoRenewFlag = ParseDataRow(row["autoRenewFlag"], false).Value;
        product.BillingAttempt = ParseDataRow(row["billing_attempt"], (int?) null);
        product.Description = ParseDataRow(row["description"], string.Empty);
        product.DisplayImageFlag = ParseDataRow(row["displayimageflag"], false);
        product.DomainID = ParseDataRow(row["domainid"], (int?) null);
        product.CommonName = ParseDataRow(row["domainname"], string.Empty);
        product.DontSync = ParseDataRow(row["dontsync"], (bool?) null);
        product.AccountExpirationDate = ParseDataRow(row["expiration_date"], (DateTime?) null);
        product.ProductTypeID = ParseDataRow(row["gdshop_product_typeID"], (int?) null);
        product.HasAddon = ParseDataRow(row["hasaddon"], (bool?) null);
        product.IsHostingProduct = ParseDataRow(row["isHosting"], false).Value;
        product.IsPastDue = ParseDataRow(row["isPastDue"], false).Value;
        product.IsRenewalPriceLocked = ParseDataRow(row["isrenewalPriceLocked"], false).Value;
        product.Namespace = ParseDataRow(row["namespace"], string.Empty);
        product.OriginalListPrice = ParseDataRow(row["originalListPrice"], (int?) null);
        product.PFID = ParseDataRow(row["pf_id"], (int?) null);
        product.RecurringPayment = ParseDataRow(row["recurring_payment"], string.Empty);
        product.UnifiedRenewalProductId = ParseDataRow(row["unified_renewal_pf_id"], (int?) null);
        product.BillingResourceId = ParseDataRow(row["resource_id"], (int?) null);
        product.UnifiedProductID = ParseDataRow(row["unified_productID"], (int?) null);
        if (ds.Tables[0].Columns.Contains("externalResourceID"))
        {
          product.ExternalResourceId = ParseDataRow(row["externalResourceID"], string.Empty);
        }
        else
        {
          product.ExternalResourceId = string.Empty;
        }

        productList.Add(product);
      }

      return productList;
    }
  }
}
