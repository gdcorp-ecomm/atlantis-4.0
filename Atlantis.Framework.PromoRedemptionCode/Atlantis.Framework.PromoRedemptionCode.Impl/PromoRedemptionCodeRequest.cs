using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.PromoRedemptionCode.Interface;

namespace Atlantis.Framework.PromoRedemptionCode.Impl
{
  public class PromoRedemptionCodeRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      string procName = "prs_redemptionCodeStatusGet_sp";
      PromoRedemptionCodeResponseData returnValue = null;
      try
      {
        PromoRedemptionCodeRequestData request = requestData as PromoRedemptionCodeRequestData;
        RedemptionCodeStatus status = null;

        if (request != null)
        {
          string connectionString = NetConnect.LookupConnectInfo(config);
          using (var connection = new SqlConnection(connectionString))
          {
            using (var cmd = new SqlCommand(procName, connection))
            {
              cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
              cmd.CommandType = CommandType.StoredProcedure;

              cmd.Parameters.Add(new SqlParameter("@n_vendorID", request.VendorId));
              cmd.Parameters.Add(new SqlParameter("@s_externalRedemptionCode", request.ExternalRedemptionCode));

              connection.Open();

              using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
              {
                if (reader.HasRows)
                {
                  int redemptionCodeIdOrdinal = reader.GetOrdinal("prs_redemptionCodeID");
                  int redemptionCodeStatusIdOrdinal = reader.GetOrdinal("prs_redemptionCodeStatusID");
                  int redemptionCodeStatusDescriptionOrdinal = reader.GetOrdinal("redemptionCodeStatusDescription");
                  int actionDateOrdinal = reader.GetOrdinal("actionDate");
                  int createDateOrdinal = reader.GetOrdinal("createDate");
                  int packageIdOrdinal = reader.GetOrdinal("prs_PackageID");
                  int packageDescriptionOrdinal = reader.GetOrdinal("packageDescription");
                  int promoTrackingCodeOrdinal = reader.GetOrdinal("promo_tracking_code");
                  int partnerDescriptionOrdinal = reader.GetOrdinal("partnerDescription");

                  reader.Read();

                  status = new RedemptionCodeStatus(
                    !reader.IsDBNull(redemptionCodeIdOrdinal) ? reader.GetInt32(redemptionCodeIdOrdinal) : 0,
                    !reader.IsDBNull(redemptionCodeStatusIdOrdinal) ? reader.GetInt32(redemptionCodeStatusIdOrdinal) : 0,
                    !reader.IsDBNull(redemptionCodeStatusDescriptionOrdinal) ? reader.GetString(redemptionCodeStatusDescriptionOrdinal) : string.Empty,
                    !reader.IsDBNull(actionDateOrdinal) ? reader.GetDateTime(actionDateOrdinal) : new DateTime(),
                    !reader.IsDBNull(createDateOrdinal) ? reader.GetDateTime(createDateOrdinal) : new DateTime(),
                    !reader.IsDBNull(packageIdOrdinal) ? reader.GetInt32(packageIdOrdinal) : 0,
                    !reader.IsDBNull(packageDescriptionOrdinal) ? reader.GetString(packageDescriptionOrdinal) : string.Empty,
                    !reader.IsDBNull(promoTrackingCodeOrdinal) ? reader.GetString(promoTrackingCodeOrdinal) : string.Empty,
                    !reader.IsDBNull(partnerDescriptionOrdinal) ? reader.GetString(partnerDescriptionOrdinal) : string.Empty
                  );
                }

                reader.Close();
              }
            }
          }
        }

        returnValue = new PromoRedemptionCodeResponseData(status);
      }
      catch (AtlantisException aex)
      {
        returnValue = new PromoRedemptionCodeResponseData(aex);
      }
      catch (Exception ex)
      {
        returnValue = new PromoRedemptionCodeResponseData(requestData, ex);
      }

      return returnValue;
    }
  }
}