using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MYAGetExpiringProductsDetail.Interface;
using Atlantis.Framework.MyaRenewalChildItems.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.MyaRenewalChildItems.Impl
{
  public class MyaRenewalChildItemsRequest : IRequest
  {
    private const string PROCNAME = "gdshop_getRenewalItems_sp";
    private const string RESOURCE_TYPE = "s_resourceType";
    private const string RECURRING_ID = "n_recurring_id";
    private const string QUANTITY = "n_quantity";   

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      DataSet ds = null;      
      List<RenewalChildItem> childList = new List<RenewalChildItem>();
      try
      {
        MyaRenewalChildItemsRequestData request = (MyaRenewalChildItemsRequestData)oRequestData;

        string connectionString = LookupConnectionString(request, oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(RECURRING_ID, request.ResourceID));
            command.Parameters.Add(new SqlParameter(QUANTITY, 1));
            command.Parameters.Add(new SqlParameter(RESOURCE_TYPE, request.Namespace));
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;

            connection.Open();
            ds = new DataSet(Guid.NewGuid().ToString());
            SqlDataAdapter adp = new SqlDataAdapter(command);
            adp.Fill(ds);            

            childList = GetObjectListFromDataset(ds);
          }
        }

        oResponseData = new MyaRenewalChildItemsResponseData(childList);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new MyaRenewalChildItemsResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new MyaRenewalChildItemsResponseData(ds, oRequestData, ex);
      }

      return oResponseData;
    }

    private string LookupConnectionString(MyaRenewalChildItemsRequestData request, ConfigElement config)
    {
      string dataSource = config.GetConfigValue("DataSourceName");
      string applicationName = config.GetConfigValue("ApplicationName");
      string certificateName = config.GetConfigValue("CertificateName");

      string result = string.Empty;
      if (!string.IsNullOrEmpty(dataSource) && !string.IsNullOrEmpty(applicationName) && !string.IsNullOrEmpty(certificateName))
      {
        result = NetConnect.LookupConnectInfo(dataSource, certificateName, applicationName, "MyaRenewalChildItemsRequest.LookupConnectionString", ConnectLookupType.NetConnectionString);
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

    private List<RenewalChildItem> GetObjectListFromDataset(DataSet ds)
    {
      List<RenewalChildItem> productList = new List<RenewalChildItem>();

      if (ds.Tables.Count > 0)
      {
        foreach (DataRow row in ds.Tables[0].Rows)
        {
          if (IsColumnNotNull("sku", row))
          {
            RenewalChildItem product = new RenewalChildItem();

            var sku = Convert.ToString(row["sku"]);

            var indexOf = sku.IndexOf('-');

            int pfid;
            if (Int32.TryParse(sku.Substring(0, indexOf), out pfid))
            {
              product.PFID = pfid;
            }
            else
            {
              product.PFID = -1;
            }

            product.Quantity = IsColumnNotNull("quantity", row) ? Convert.ToInt32(row["quantity"]) : 0;
            product.ProductTypeID = IsColumnNotNull("gdshop_product_typeID", row) ? Convert.ToInt32(row["gdshop_product_typeID"]) : 0;

            productList.Add(product);
          }
        }
      }
      return productList;
    }

    private bool IsColumnNotNull(string columnName, DataRow row)
    {
      return row[columnName] != null && !(row[columnName] is DBNull);
    }
  }
}
