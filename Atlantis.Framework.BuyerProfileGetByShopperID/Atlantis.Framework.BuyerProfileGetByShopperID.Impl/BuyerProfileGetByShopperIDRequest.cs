using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.BuyerProfileGetByShopperID.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.BuyerProfileGetByShopperID.Impl
{
  public class BuyerProfileGetByShopperIDRequest : IRequest
  {
    private const string PROC_NAME = "mya_GetBuyerProfiles_sp";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      BuyerProfileGetByShopperIDRequestData request = (BuyerProfileGetByShopperIDRequestData)oRequestData;

      List<ProfileSummary> _profiles = new List<ProfileSummary>();

      try
      {
        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROC_NAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            command.Parameters.Add(new SqlParameter("@shopper_id", request.ShopperID));

            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
              if (reader.HasRows)
              {
                while (reader.Read())
                {
                  string name = string.Empty;
                  string id = string.Empty;
                  bool isdefault = false;
                  id = reader["gdshop_BuyerProfileID"].ToString();
                  name = reader["profileName"].ToString(); 
                  isdefault = reader["defaultProfileFlag"].ToString() == "1";
                  ProfileSummary ps = new ProfileSummary(id, name, isdefault);
                  _profiles.Add(ps);

                }
              }
            }
          }
        }
        oResponseData = new BuyerProfileGetByShopperIDResponseData(_profiles);
      }
      catch (Exception ex)
      {
        oResponseData = new BuyerProfileGetByShopperIDResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
