using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaMirageData.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.MyaMirageData.Impl
{
  public class MyaMirageDataRequest : IRequest
  {
    const string _MIRAGEDATAPROC = "gdshop_shopperProductQuantityMapGet_sp";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MyaMirageDataResponseData result;
      Dictionary<string, int> responseData = new Dictionary<string,int>();

      try
      {
        MyaMirageDataRequestData mirageRequest = (MyaMirageDataRequestData)requestData;
        if (!string.IsNullOrEmpty(requestData.ShopperID))
        {
          string connectionString = NetConnect.LookupConnectInfo(config);

          using (SqlConnection connection = new SqlConnection(connectionString))
          {
            using (SqlCommand command = new SqlCommand(_MIRAGEDATAPROC, connection))
            {
              command.CommandType = CommandType.StoredProcedure;
              command.Parameters.Add(new SqlParameter("shopper_id", mirageRequest.ShopperID));
              command.CommandTimeout = (int)mirageRequest.RequestTimeout.TotalSeconds;
              connection.Open();

              using (SqlDataReader reader = command.ExecuteReader())
              {
                while (reader.Read())
                {
                  int quantity;
                  if (Int32.TryParse(reader["quantity"].ToString(), out quantity))
                  {
                    responseData[reader["pf_id_or_namespace"].ToString()] = quantity;
                  }
                }
              }
            }
          }
        }

        result = new MyaMirageDataResponseData(responseData);
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException(requestData, "MyaMirageDataRequest.RequestHandler", ex.Message, ex.StackTrace, ex);
        result = new MyaMirageDataResponseData(aex);
      }

      return result;
    }

    #endregion
  }
}
