using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MYAGetExpiringProductsDetail.Interface;
using Atlantis.Framework.MyaRenewalBundleItems.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.MyaRenewalBundleItems.Impl
{
  public class MyaRenewalBundleItemsRequest : IRequest
  {
    private const string PROCNAME = "gdshop_getRenewalBundleItems_sp";
    private const string RECURRING_ID = "n_recurring_id";

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      DataSet ds = null;
      List<RenewalBundleItem> childList = new List<RenewalBundleItem>();
      
      try
      {
        MyaRenewalBundleItemsRequestData request = (MyaRenewalBundleItemsRequestData)oRequestData;

        string connectionString = LookupConnectionString(request, oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(RECURRING_ID, request.ResourceID));                        
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;

            connection.Open();
            ds = new DataSet(Guid.NewGuid().ToString());
            SqlDataAdapter adp = new SqlDataAdapter(command);
            adp.Fill(ds);

            childList = GetObjectListFromDataset(ds);
          }
        }

        oResponseData = new MyaRenewalBundleItemsResponseData(childList);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new MyaRenewalBundleItemsResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new MyaRenewalBundleItemsResponseData(ds, oRequestData, ex);
      }

      return oResponseData;
    }

    private string LookupConnectionString(MyaRenewalBundleItemsRequestData request, ConfigElement config)
    {
      string dataSource = config.GetConfigValue("DataSourceName");
      string applicationName = config.GetConfigValue("ApplicationName");
      string certificateName = config.GetConfigValue("CertificateName");

      string result = string.Empty;
      if (!string.IsNullOrEmpty(dataSource) && !string.IsNullOrEmpty(applicationName) && !string.IsNullOrEmpty(certificateName))
      {
        result = NetConnect.LookupConnectInfo(dataSource, certificateName, applicationName, "MyaRenewalBundleItemsRequest.LookupConnectionString", ConnectLookupType.NetConnectionString);
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

    private List<RenewalBundleItem> GetObjectListFromDataset(DataSet ds)
    {
      List<RenewalBundleItem> productList = new List<RenewalBundleItem>();

      if (ds.Tables.Count > 0)
      {
        foreach (DataRow row in ds.Tables[0].Rows)
        {
          RenewalBundleItem item = new RenewalBundleItem();

          if (IsColumnNotNull("resource_recurring_id", row))
          {
            item.ResourceRecurringID = Convert.ToInt32(row["resource_recurring_id"]);
            item.PresentationSequenceID = IsColumnNotNull("presentation_sequence_id", row) ? Convert.ToInt32(row["presentation_sequence_id"]) : 0;
            item.Namespace = IsColumnNotNull("presentation_sequence_id", row) ? Convert.ToString(row["namespace"]) : string.Empty;
          }


          productList.Add(item);
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
