using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaIsMirageCurrent.Interface;

namespace Atlantis.Framework.MyaIsMirageCurrent.Impl
{
  public class MyaIsMirageCurrentRequest : IRequest
  {
    private const string _PROCNAME = "mya_isMirageCurrent_sp";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      MyaIsMirageCurrentRequestData isMirageCurrentRequest = (MyaIsMirageCurrentRequestData)oRequestData;
      MyaIsMirageCurrentResponseData result;

      try
      {
        bool isCurrent = true;

        string connectionString = Nimitz.NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("shopper_id", isMirageCurrentRequest.ShopperID));
            command.CommandTimeout = (int)isMirageCurrentRequest.RequestTimeout.TotalSeconds;
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
              while (reader.Read())
              {
                object mirageShopper = reader[0];
                if ((mirageShopper != null) && (mirageShopper.GetType() != typeof(DBNull)))
                {
                  if (isMirageCurrentRequest.ShopperID == mirageShopper.ToString())
                  {
                    isCurrent = false;
                  }
                }
                break;
              }
            }
          }
        }

        result = new MyaIsMirageCurrentResponseData(isCurrent);
      }
      catch (Exception ex)
      {
        result = new MyaIsMirageCurrentResponseData(oRequestData, ex);
      }

      return result;
    }

    #endregion

  }
}
