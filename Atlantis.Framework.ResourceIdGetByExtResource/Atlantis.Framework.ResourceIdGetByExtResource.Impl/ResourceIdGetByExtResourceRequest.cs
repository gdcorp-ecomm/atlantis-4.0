﻿using System;
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
    private const string COLUMN_RESOURCE_ID = "resource_id";
    private const string COLUMN_PRODUCT_TYPE_ID = "gdshop_product_typeId";
    private const string COLUMN_NAMESPACE = "nameSpace";
    private const string COLUMN_ORION_NAMESPACE = "OrionNamespace";
    private const string COLUMN_EXTERNAL_RESOURCE_PROC = "getByExternalResourceProcedureName";
    private const string COLUMN_EXTERNAL_RESOURCE_CONN_STR = "getByExternalResourceConnectionString";

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
        ProcConfigItem configItem = ProcConfigList[request.OrionNamespace.ToLowerInvariant()];
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

              responseData = new ResourceIdGetByExtResourceResponseData(resourceId, configItem.ProductTypeId);
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
        resourceId = Convert.ToInt32(reader[COLUMN_RESOURCE_ID]);

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
              var orionNamespace = Convert.ToString(reader["OrionNamespace"]).ToLowerInvariant();
              if (!procConfigList.ContainsKey(orionNamespace))
                procConfigList.Add(orionNamespace, new ProcConfigItem(Convert.ToInt32(reader[COLUMN_PRODUCT_TYPE_ID]), 
                                                                      Convert.ToString(reader[COLUMN_NAMESPACE]), 
                                                                      Convert.ToString(reader[COLUMN_ORION_NAMESPACE]), 
                                                                      Convert.ToString(reader[COLUMN_EXTERNAL_RESOURCE_PROC]), 
                                                                      Convert.ToString(reader[COLUMN_EXTERNAL_RESOURCE_CONN_STR])));
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
