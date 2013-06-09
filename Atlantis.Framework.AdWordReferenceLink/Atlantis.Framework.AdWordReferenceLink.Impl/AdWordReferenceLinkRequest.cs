using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.AdWordReferenceLink.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.AdWordReferenceLink.Impl
{
  public class AdWordReferenceLinkRequest : IRequest
  {
    private const string PROCNAME = "gdshop_adwordGetReferenceLinkByCouponCode_sp";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      AdWordReferenceLinkRequestData request = (AdWordReferenceLinkRequestData)oRequestData;

      try
      {
        string refLink = string.Empty;
        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            command.Parameters.AddWithValue("@s_couponCode", request.CouponCode);

            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
              while (reader.Read())
              {
                int colpos = reader.GetOrdinal("referenceLink");
                refLink = !reader.IsDBNull(colpos) ? reader.GetString(colpos) : string.Empty;
              }
            }

          }
        }

        oResponseData = new AdWordReferenceLinkResponseData(refLink);
      }
      catch (Exception ex)
      {
        oResponseData = new AdWordReferenceLinkResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

  }
}
