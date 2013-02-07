using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ResourceCountByPaymentProfile.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ResourceCountByPaymentProfile.Impl
{
  public class ResourceCountByPaymentProfileRequest : IRequest
  {
    private const string PROC_NAME = "mya_ResourceCountByProfile_sp";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      try
      {
        int numberOfRecords = 0;

        ResourceCountByPaymentProfileRequestData request = oRequestData as ResourceCountByPaymentProfileRequestData;

        string connectionString = NetConnect.LookupConnectInfo(oConfig, ConnectLookupType.NetConnectionString);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {

          using (SqlCommand command = new SqlCommand(PROC_NAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            command.Parameters.Add(new SqlParameter("@s_shopper_id", request.ShopperID));
            command.Parameters.Add(new SqlParameter("@n_shopperProfileID", request.ProfileId));

            connection.Open();
            numberOfRecords = (Int32)command.ExecuteScalar();
          }
        }


        oResponseData = new ResourceCountByPaymentProfileResponseData(numberOfRecords);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new ResourceCountByPaymentProfileResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new ResourceCountByPaymentProfileResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

  }
}
