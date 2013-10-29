using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.Shopper.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Atlantis.Framework.Shopper.Impl
{
  class VerifyCountryAllowedRequest : IRequest
  {
    private const string _PROCNAME = "gdshop_blockedCountryGet_sp";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      VerifyCountryAllowedResponseData response;
      VerifiyCountryAllowedRequestData currentRequest = (VerifiyCountryAllowedRequestData)requestData;
      string connectionString = NetConnect.LookupConnectInfo(config);
      HashSet<string> blockedCountries = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
        {
          command.CommandType = CommandType.StoredProcedure;
          command.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
          connection.Open();
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              if (reader[0] != DBNull.Value)
              {
                string countryCode = reader[0] as string;
                if (!string.IsNullOrEmpty(countryCode) && !blockedCountries.Contains(countryCode))
                {
                  blockedCountries.Add(countryCode);
                }
              }
            }
          }
        }
      }
      response = new VerifyCountryAllowedResponseData(blockedCountries);
      return response;
    }

  }
}