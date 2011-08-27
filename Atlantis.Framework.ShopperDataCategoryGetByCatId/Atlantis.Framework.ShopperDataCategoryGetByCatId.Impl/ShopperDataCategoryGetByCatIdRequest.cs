using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ShopperDataCategoryGetByCatId.Interface;

namespace Atlantis.Framework.ShopperDataCategoryGetByCatId.Impl
{
  public class ShopperDataCategoryGetByCatIdRequest : IRequest
  {
    private const string PROC_NAME = "mya_ShopperDataSelectbyCategoryandShopper_sp";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      ShopperDataCategoryGetByCatIdRequestData request = (ShopperDataCategoryGetByCatIdRequestData)oRequestData;

      int data = 0;
      string catName = string.Empty;
      try
      {
        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROC_NAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            command.Parameters.Add(new SqlParameter("@Shopper_id", request.ShopperID));
            command.Parameters.Add(new SqlParameter("@Category", request.CategoryID));

            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
              while (reader.Read())
              {
                catName = reader["Name"] == DBNull.Value ? "" : reader["Name"].ToString().Trim();
                data = Int32.Parse(reader["Data"].ToString());
              }
            }

          }
        }

        oResponseData = new ShopperDataCategoryGetByCatIdResponseData(data, catName);
      }
      catch (Exception ex)
      {
        oResponseData = new ShopperDataCategoryGetByCatIdResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

  }
}
