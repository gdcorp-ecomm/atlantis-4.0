using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.EcommGiftCardIsCancelled.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.EcommGiftCardIsCancelled.Impl
{
  public class EcommGiftCardIsCancelledRequest : IRequest
  {
    private const string PROCNAME = "gdshop_billingGiftCardGetStatus_sp";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      DataSet ds = null;
      try
      {
        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROCNAME, connection))
          {
            EcommGiftCardIsCancelledRequestData request = (EcommGiftCardIsCancelledRequestData)oRequestData;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            command.Parameters.Add(new SqlParameter("resource_id", request.ResourceId));

            connection.Open();
            ds = new DataSet(Guid.NewGuid().ToString());
            SqlDataAdapter adp = new SqlDataAdapter(command);
            adp.Fill(ds);
          }
        }

        oResponseData = new EcommGiftCardIsCancelledResponseData(ds);
      }
      catch (Exception ex)
      {
        oResponseData = new EcommGiftCardIsCancelledResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
