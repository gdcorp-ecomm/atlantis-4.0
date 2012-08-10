using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.PromoOffering.Interface;

namespace Atlantis.Framework.PromoOffering.Impl
{
  public class PromoOfferingRequest : IRequest
  {
    private const string PROCNAME = "rex_getPromoOfferings_sp";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      PromoOfferingResponseData returnValue = new PromoOfferingResponseData(new List<ResellerPromoItem>(5));

      try
      {
        IList<ResellerPromoItem> promotions = new List<ResellerPromoItem>(5);

        PromoOfferingRequestData request = requestData as PromoOfferingRequestData;
        if (null != request)
        {
          using (SqlConnection connection = new SqlConnection(NetConnect.LookupConnectInfo(config)))
          {
            using (SqlCommand command = new SqlCommand(PROCNAME, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = (int)request.RequestTimeout.TotalSeconds })
            {
              command.Parameters.Add(new SqlParameter("@n_privateLabelID", request.PrivateLabelId));

              connection.Open();

              using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
              {
                if (reader.HasRows)
                {
                  int groupIdOrdinal = reader.GetOrdinal("groupid");
                  int descOrdinal = reader.GetOrdinal("description");
                  int activeOrdinal = reader.GetOrdinal("isactive");
                  int promoGroupIdOrdinal = reader.GetOrdinal("pl_promoGroupid");

                  while (reader.Read())
                  {
                    ResellerPromoItem item = new ResellerPromoItem(
                      !reader.IsDBNull(descOrdinal) ? reader.GetString(descOrdinal) : string.Empty,
                      !reader.IsDBNull(groupIdOrdinal) ? reader.GetInt32(groupIdOrdinal) : 0,
                      !reader.IsDBNull(activeOrdinal) ? reader.GetInt32(activeOrdinal) == 1 : false,
                      !reader.IsDBNull(promoGroupIdOrdinal) ? reader.GetInt32(promoGroupIdOrdinal) : 0);
                    promotions.Add(item);
                  }
                }
                reader.Close();
              }

            }
          }
        }

        returnValue = new PromoOfferingResponseData(promotions);

      }
      catch (AtlantisException aex)
      {
        returnValue = new PromoOfferingResponseData(aex);
      }
      catch (Exception ex)
      {
        returnValue = new PromoOfferingResponseData(requestData, ex);
      }

      return returnValue;
    }

  }
}
