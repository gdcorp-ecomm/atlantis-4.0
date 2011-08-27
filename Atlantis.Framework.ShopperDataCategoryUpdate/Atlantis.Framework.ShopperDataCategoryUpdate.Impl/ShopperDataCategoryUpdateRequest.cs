using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ShopperDataCategoryUpdate.Interface;

namespace Atlantis.Framework.ShopperDataCategoryUpdate.Impl
{
  public class ShopperDataCategoryUpdateRequest : IRequest
  {
    private const string PROC_NAME = "mya_ShopperDataUpdate_sp";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      ShopperDataCategoryUpdateRequestData request = (ShopperDataCategoryUpdateRequestData)oRequestData;

      int result = -1;
      try
      {
        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROC_NAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            command.Parameters.Add(new SqlParameter("@Category", request.CategoryID));
            command.Parameters.Add(new SqlParameter("@Shopper_id", request.ShopperID));
            command.Parameters.Add(new SqlParameter("@Data", request.ShopperData));
            SqlParameter newparam = command.Parameters.Add("@ReturnValue", SqlDbType.Int);
            newparam.Direction = ParameterDirection.ReturnValue;

            connection.Open();
            command.ExecuteNonQuery();

            result = (int)command.Parameters["@ReturnValue"].Value;
          }
        }

        oResponseData = new ShopperDataCategoryUpdateResponseData(result);
      }
      catch (Exception ex)
      {
        oResponseData = new ShopperDataCategoryUpdateResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
