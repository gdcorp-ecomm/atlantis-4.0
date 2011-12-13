using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.EcomOrderLevelPromo.Interface;
using System.Data.SqlClient;
using System.Data;

namespace Atlantis.Framework.EcomOrderLevelPromo.Impl
{
  public class EcomOrderLevelPromoRequest : IRequest
  {

    private const string _PROCNAME = "rex_getOrderLevelDiscounts_sp";
    private const string _PrivateLabelIDParam = "n_privateLabelID";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      try
      {
        Dictionary<string, OrderLevelPromo> resultValues = new Dictionary<string, OrderLevelPromo>();
        EcomOrderLevelPromoRequestData request = (EcomOrderLevelPromoRequestData)requestData; ;
        string connectionString = Nimitz.NetConnect.LookupConnectInfo(config);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(_PrivateLabelIDParam, request.PrivateLabelID));
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            connection.Open();           
            using (SqlDataReader reader = command.ExecuteReader())
            {
              while (reader.Read())
              {
                string iscCode = ReadField<string>(reader, "iscCode", string.Empty);
                string iscDescription = ReadField<string>(reader, "iscCodeDescription", string.Empty);
                int isActive = ReadField<int>(reader, "isActive", 0);
                if (!resultValues.ContainsKey(iscCode.ToUpper()))
                {
                  if (isActive==1)
                  {
                    OrderLevelPromo currentPromo = new OrderLevelPromo()
                    {
                      IsActive = true,
                      ISCCode = iscCode,
                      ISCDescription = iscDescription
                    };
                    resultValues[iscCode.ToUpper()] = currentPromo;
                  }
                }
              }
            }
          }
        }
        responseData = new EcomOrderLevelPromoResponseData(resultValues);
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new EcomOrderLevelPromoResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new EcomOrderLevelPromoResponseData(requestData, ex);
      }

      return responseData;
    }

    private static VT ReadField<VT>(IDataReader currentReader, string columnName, VT defaultValue)
    {
      try
      {
        int idx = currentReader.GetOrdinal(columnName);
        if (currentReader.IsDBNull(idx))
          return defaultValue;
        else
          return (VT)currentReader[idx];
      }
      catch (System.Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.ToString());
      }
      return defaultValue;
    }
  }
}
