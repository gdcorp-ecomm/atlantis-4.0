using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Atlantis.Framework.Interface;
using Atlantis.Framework.ResourceIdGetByExtResource.Interface;

namespace Atlantis.Framework.ResourceIdGetByExtResource.Impl
{
  public class ResourceIdGetByExtResourceRequest : IRequest
  {
    #region Parameter Constants

    private const string CONFIG_STORED_PROCEDURE = "gdshop_product_typeOrionGetExternalResourceProcedureName_sp";
    private const string EXTERNAL_RESOURCE_ID_PARAM = "@s_externalResourceID";

    #endregion

    private static ConfigElement _config;

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ResourceIdGetByExtResourceResponseData responseData = null;

      try
      {
        _config = config;

        var request = (ResourceIdGetByExtResourceRequestData)requestData;

        //Get proc to execute
        ProcConfigItem configItem = ProcConfigList[request.OrionNamespace];
        if (configItem != null)
        {

          using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(configItem.ConnectionString, config.GetConfigValue("CertificateName"), config.GetConfigValue("ApplicationName"), "ResourceIdGetByExtResourceRequest", Nimitz.ConnectLookupType.NetConnectionString)))
          {
            cn.Open();

            using (var cmd = new SqlCommand(configItem.StoredProcedure, cn))
            {
              cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
              cmd.CommandType = CommandType.StoredProcedure;
              cmd.Parameters.AddRange(SetSqlParameters(request, configItem));

              var resourceId = 0;

              using (SqlDataReader reader = cmd.ExecuteReader())
              {
                if (reader.Read())
                {
                  resourceId = GetResourceId(reader, request);
                }
              }

              responseData = new ResourceIdGetByExtResourceResponseData(resourceId);
            }
            cn.Close();
          }
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new ResourceIdGetByExtResourceResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        var atlEx = new AtlantisException(requestData, "ResourceIdGetByExtResourceRequest.RequestHandler", ex.Message, string.Empty, ex);
        responseData = new ResourceIdGetByExtResourceResponseData(atlEx);
      }

      return responseData;
    }

    #region Populate Server Hosting Product
    private static int GetResourceId(IDataReader reader, ResourceIdGetByExtResourceRequestData request)
    {
      int resourceId = 0;

      if (reader.FieldCount > 0)
      {
        resourceId = reader.GetInt32(0);
      }

      return resourceId;
    }
    #endregion

    #region SQL Parameter Handling

    private static SqlParameter[] SetSqlParameters(ResourceIdGetByExtResourceRequestData request, ProcConfigItem configItem)
    {
      var paramColl = new List<SqlParameter>
                        {
                          new SqlParameter(EXTERNAL_RESOURCE_ID_PARAM, request.ExternalResourceId),
                        };

      var paramArray = new SqlParameter[paramColl.Count];
      paramColl.CopyTo(paramArray, 0);

      return paramArray;
    }

    #endregion

    #region ProcConfigList DataCached

    internal static Dictionary<string, ProcConfigItem> GetProcConfigList(string key)
    {
      var procConfigList = new Dictionary<string, ProcConfigItem>();
      using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(_config)))
      {
        cn.Open();

        using (var cmd = new SqlCommand(CONFIG_STORED_PROCEDURE, cn))
        {
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.CommandTimeout = (int)TimeSpan.FromSeconds(5d).TotalMilliseconds;

          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              if(!procConfigList.ContainsKey(reader.GetString(2)))
                procConfigList.Add(reader.GetString(2), new ProcConfigItem(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4)));
            }
          }
        }

        cn.Close();
      }

      return procConfigList;
    }

    private static Dictionary<string, ProcConfigItem> ProcConfigList
    {
      get
      {
        return DataCache.DataCache.GetCustomCacheData("{extResConfigItems}", GetProcConfigList);
      }
    }

    #endregion ProcConfigList DataCached
  }
}
