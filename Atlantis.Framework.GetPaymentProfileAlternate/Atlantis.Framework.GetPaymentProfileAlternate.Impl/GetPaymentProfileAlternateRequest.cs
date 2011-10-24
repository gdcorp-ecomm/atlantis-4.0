using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.GetPaymentProfileAlternate.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetPaymentProfileAlternate.Impl
{
  class GetPaymentProfileAlternateRequest : IRequest
  {
    private static readonly string procName = "dbo.gdshop_GetBackupPPShopperProfileID";
    private static readonly string alternatePaymentProfileIdParam = "pp_backupShopperProfileID";
    private static readonly string shopperIdParam = "shopper_id";

    #region IRequest Members
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement config)
    {

      GetPaymentProfileAlternateResponseData responseData;
      GetPaymentProfileAlternateRequestData requestData = null;
      try
      {
        requestData = (GetPaymentProfileAlternateRequestData)oRequestData;

        using (var connection = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
        {
          using (var command = new SqlCommand(procName, connection))
          {
            command.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter(shopperIdParam, requestData.ShopperID));

            var p2 = new SqlParameter(alternatePaymentProfileIdParam, SqlDbType.Int, 4);
            p2.Direction = ParameterDirection.Output;
            command.Parameters.Add(p2);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            object res = p2.Value;
            int alternateProfileId = (res != null && res is Int32) ? Convert.ToInt32(res) : -1;
            responseData = new GetPaymentProfileAlternateResponseData(alternateProfileId, true);
          }
        }
      }
      catch (Exception ex)
      {
        if (oRequestData is GetPaymentProfileAlternateRequestData)
        {
          string data = String.Concat("requestData.RequestTimeout(s)=", requestData.RequestTimeout.Seconds);
          throw BuildException(requestData, "RequestHandler", ex, data);
        }
        else
        {
          throw BuildException(oRequestData, "RequestHandler", ex, null);
        }
      }

      return responseData;
    }

    #endregion

    private static AtlantisException BuildException(RequestData requestData, string sourceFunction, Exception ex, string data)
    {
      return new AtlantisException(requestData, "GetPaymentProfileAlternateRequestData." + sourceFunction, ex.Message + Environment.NewLine + ex.StackTrace, data, ex);
    }

  }
}
