using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MYAGetMobileCarriers.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.MYAGetMobileCarriers.Impl
{
  public class MYAGetMobileCarriersRequest : IRequest
  {
    private const string PROCNAME = "gdshop_getmktgMobileCarrier_sp";


    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      MYAGetMobileCarriersRequestData request = (MYAGetMobileCarriersRequestData)oRequestData;

      List<MobileCarrierItem> carrierList = new List<MobileCarrierItem>();

      try
      {
        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;

            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
              if (reader.HasRows)
              {
                int CARRIER_ID = reader.GetOrdinal("gdshop_mktgMobileCarrierID");
                int DESCRIPTION = reader.GetOrdinal("description");
                while (reader.Read())
                {
                  MobileCarrierItem item = new MobileCarrierItem();
                  if (reader.GetString(DESCRIPTION) != null)
                  {
                    item.MobileCarrierID = reader.GetInt32(CARRIER_ID);
                    item.Description = reader.GetString(DESCRIPTION);
                    carrierList.Add(item);
                  }
                }
              }
            }
          }
        }

        oResponseData = new MYAGetMobileCarriersResponseData(carrierList);
      }
      catch (Exception ex)
      {
        oResponseData = new MYAGetMobileCarriersResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
