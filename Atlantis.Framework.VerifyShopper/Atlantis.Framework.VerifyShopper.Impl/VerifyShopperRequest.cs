using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.VerifyShopper.Interface;

namespace Atlantis.Framework.VerifyShopper.Impl
{
  public class VerifyShopperRequest : IRequest
  {
    private const string _PROCNAME = "gdshop_privateLabelGetbyShopper_sp";
    private const string _SHOPPERIDPARAM = "s_shopper_id";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      int privateLabelId = 0;

      try
      {
        VerifyShopperRequestData request = (VerifyShopperRequestData)oRequestData;

        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(_SHOPPERIDPARAM, request.ShopperID));
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
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

        oResponseData = new VerifyShopperResponseData(privateLabelId);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new VerifyShopperResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new VerifyShopperResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

  }
}
