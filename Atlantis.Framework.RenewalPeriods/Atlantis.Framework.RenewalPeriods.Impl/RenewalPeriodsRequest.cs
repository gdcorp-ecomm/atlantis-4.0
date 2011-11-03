using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.RenewalPeriods.Interface;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.RenewalPeriods.Impl
{
  public class RenewalPeriodsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      RenewalPeriodsResponseData responseData = null;

      try
      {
        RenewalPeriodsRequestData request = (RenewalPeriodsRequestData)requestData;
        responseData = GetRenewalPeriods(request, config);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new RenewalPeriodsResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new RenewalPeriodsResponseData(requestData, ex);
      }
      return responseData;
    }

    private RenewalPeriodsResponseData GetRenewalPeriods(RenewalPeriodsRequestData request, ConfigElement config)
    {

      RenewalPeriodsResponseData responseData = new RenewalPeriodsResponseData(new Dictionary<int, Dictionary<string, object>>());

      const string PROC_NAME = "gdshop_billingDomainGetRenewalPeriods_sp";
      const string RESOURCE_ID_LIST_PARAM = "@resource_idlist";
      const string SHOPPER_ID_PARAM = "@s_shopperID";

      string connectionString = LookupConnectionString(config);
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        using (SqlCommand command = new SqlCommand(PROC_NAME, connection))
        {
          command.CommandType = CommandType.StoredProcedure;
          command.CommandTimeout = 5;
          command.Parameters.Add(new SqlParameter(SHOPPER_ID_PARAM, request.ShopperID));

          if (request.ResourceIds != null)
          {
            DataTable dataTable = new DataTable("resource_idlist_utt");
            dataTable.Columns.Add(new DataColumn("resource_id", typeof(Int32)));
            foreach (int resourceId in request.ResourceIds)
            {
              DataRow domainRow = dataTable.NewRow();
              domainRow["resource_id"] = resourceId;
              dataTable.Rows.Add(domainRow);
            }
            SqlParameter resourceIds = new SqlParameter(RESOURCE_ID_LIST_PARAM, dataTable);
            resourceIds.SqlDbType = SqlDbType.Structured;
            resourceIds.TypeName = "resource_idlist_utt";
            command.Parameters.Add(resourceIds);
          }
          connection.Open();
          using (SqlDataReader reader = command.ExecuteReader())
          {
            if (reader != null && reader.HasRows)
            {
              Dictionary<int, Dictionary<string, object>> domainProperties = new Dictionary<int, Dictionary<string, object>>();
              while (reader.Read())
              {
                Dictionary<string, object> propertiesDictionary = new Dictionary<string, object>(reader.FieldCount);
                int resourceId = -1;
                for (int i = 0; i < reader.FieldCount; i++)
                {
                  string name = reader.GetName(i);
                  if (!propertiesDictionary.ContainsKey(name))
                  {
                    object value = reader.GetValue(i);
                    if (name.ToLower() == "resource_id")
                    {
                      resourceId = (int)value;
                    }
                    else
                    {
                      propertiesDictionary.Add(name, value);
                    }
                  }
                }

                if (propertiesDictionary.Count > 0 && !domainProperties.ContainsKey(resourceId))
                {
                  domainProperties.Add(resourceId, propertiesDictionary);
                }
              }
              responseData = new RenewalPeriodsResponseData(domainProperties);
            }
          }
        }
      }
      return responseData;
    }

    private static string LookupConnectionString(ConfigElement config)
    {
      string result = Nimitz.NetConnect.LookupConnectInfo(config.GetConfigValue("DataSourceName"),
                                     config.GetConfigValue("CertificateName"),
                                     config.GetConfigValue("ApplicationName"),
                                     "Atlantis.Framework.RenewalPeriods",
                                     ConnectLookupType.NetConnectionString);

      if (string.IsNullOrEmpty(result) || result.Length <= 2)
      {
        throw new Exception("Invalid Connection String");
      }

      return result;
    }
  }
}
