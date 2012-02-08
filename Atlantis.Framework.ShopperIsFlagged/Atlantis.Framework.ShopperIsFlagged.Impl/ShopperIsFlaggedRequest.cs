using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ShopperIsFlagged.Interface;
namespace Atlantis.Framework.ShopperIsFlagged.Impl
{
  public class ShopperIsFlaggedRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ShopperIsFlaggedResponseData response = null;

      try
      {
        string connectionString = NetConnect.LookupConnectInfo(config);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand("mya_SixDigitValidation_sp", connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
            command.Parameters.AddWithValue("@shopper_id", requestData.ShopperID);

            command.Connection.Open();
            bool isFlagged = Convert.ToBoolean(command.ExecuteScalar()); //returns 1 for flagged, 0 for not flagged
            command.Connection.Close();

            response = new ShopperIsFlaggedResponseData(isFlagged);
          }
        }
      }
      catch (AtlantisException aex)
      {
        response = new ShopperIsFlaggedResponseData(aex);
      }
      catch (Exception ex)
      {
        response = new ShopperIsFlaggedResponseData(requestData, ex);
      }

      return response;
    }
  }
}

