using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PromoReferISCAvailCheck.Interface;

namespace Atlantis.Framework.PromoReferISCAvailCheck.Impl
{
  public class PromoReferISCAvailCheckRequest : IRequest
  {
    private const string _PROCNAMESCHEDULEGET = "gdshop_promoReferISCAvailCheck_sp";
    private const string _COUPONCODE = "s_couponCode";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      PromoReferISCAvailCheckResponseData responseData = null;
      var request = (PromoReferISCAvailCheckRequestData)requestData;

      try
      {

        if (!string.IsNullOrEmpty(request.CouponCode))
        {

          string connectionString = Nimitz.NetConnect.LookupConnectInfo(config);
          using (var connection = new SqlConnection(connectionString))
          {
            using (var command = new SqlCommand(_PROCNAMESCHEDULEGET, connection))
            {
              command.CommandType = CommandType.StoredProcedure;
              command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
              command.Parameters.Add(new SqlParameter(_COUPONCODE, request.CouponCode));
              connection.Open();
              int isAvailable = (int)command.ExecuteScalar();
              responseData = new PromoReferISCAvailCheckResponseData(isAvailable);
            }
          }
        }
        else
        {
          //null or empty promocodes are not available
          responseData = new PromoReferISCAvailCheckResponseData(0);
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new PromoReferISCAvailCheckResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new PromoReferISCAvailCheckResponseData(request, ex);
      }

      return responseData;
    }

  }
}
