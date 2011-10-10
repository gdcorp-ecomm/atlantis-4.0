using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.DCCGetForwarding.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.DCCGetForwarding.Impl
{
  public class DCCGetForwardingRequest : IRequest
  {
    private const string PROC_NAME = "mya_redirect_getbyDomainName_sp";
    private const string DOMAIN_NAME_PARAM = "@domainName";

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DCCGetForwardingRequestData request = (DCCGetForwardingRequestData)oRequestData;
      DCCGetForwardingResponseData response;

      try
      {
        response = GetForwardingData(request, oConfig);
      }
      catch (Exception ex)
      {
        response = new DCCGetForwardingResponseData(oRequestData, ex);
      }

      return response;
    }

    private DCCGetForwardingResponseData GetForwardingData(DCCGetForwardingRequestData request, ConfigElement config)
    {
      DCCGetForwardingResponseData responseData = new DCCGetForwardingResponseData();

      string connectionString = LookupConnectionString(config);
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        using (SqlCommand command = new SqlCommand(PROC_NAME, connection))
        {
          command.CommandType = CommandType.StoredProcedure;
          command.CommandTimeout = 5;
          command.Parameters.Add(new SqlParameter(DOMAIN_NAME_PARAM, request.DomainName));
          connection.Open();
          using (SqlDataReader reader = command.ExecuteReader())
          {
            if(reader != null && reader.HasRows)
            {
              while (reader.Read())
              {
                IDictionary<string, object> propertiesDictionary = new Dictionary<string, object>(reader.FieldCount);
 
                for(int i = 0; i < reader.FieldCount; i++)
                {
                  if(!propertiesDictionary.ContainsKey(reader.GetName(i)))
                  {
                    propertiesDictionary.Add(reader.GetName(i), reader.GetValue(i));
                  }
                }

                if(propertiesDictionary.Count > 0)
                {
                  responseData = new DCCGetForwardingResponseData(propertiesDictionary);
                }
              }
            }
          }
        }
      }
      return responseData; 
    }

    private static string LookupConnectionString(ConfigElement config)
    {
      return NetConnect.LookupConnectInfo(config, ConnectLookupType.NetConnectionString);
    }
  }
}
