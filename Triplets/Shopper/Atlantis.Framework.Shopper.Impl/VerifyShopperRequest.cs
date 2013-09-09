using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.Shopper.Interface;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Atlantis.Framework.Shopper.Impl
{
  public class VerifyShopperRequest : IRequest
  {
    private const string _PROCNAME = "gdshop_privateLabelGetbyShopper_sp";
    private const string _SHOPPERIDPARAM = "s_shopper_id";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      if (string.IsNullOrEmpty(requestData.ShopperID))
      {
        return VerifyShopperResponseData.NotVerified;
      }

      int privateLabelId = 0;

      string connectionString = NetConnect.LookupConnectInfo(config);
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
        {
          command.CommandType = CommandType.StoredProcedure;
          command.Parameters.Add(new SqlParameter(_SHOPPERIDPARAM, requestData.ShopperID));
          command.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
          connection.Open();
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              object privateLabelIdObj = reader[0];
              privateLabelId = Convert.ToInt32(privateLabelIdObj);
              break;
            }
          }
        }
      }

      return VerifyShopperResponseData.FromPrivateLabelId(privateLabelId);
    }

  }
}
