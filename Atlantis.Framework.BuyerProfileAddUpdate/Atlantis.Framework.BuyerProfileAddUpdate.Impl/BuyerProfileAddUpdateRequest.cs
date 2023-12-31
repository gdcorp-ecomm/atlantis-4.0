﻿using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.BuyerProfileAddUpdate.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.BuyerProfileAddUpdate.Impl
{
  
  public class BuyerProfileAddUpdateRequest : IRequest
  {
    private const string PROC_NAME = "mya_updateBuyerProfile_sp";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      BuyerProfileAddUpdateRequestData request = (BuyerProfileAddUpdateRequestData)oRequestData;

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
            command.Parameters.Add(new SqlParameter("@shopper_id", request.ShopperID));
            command.Parameters.Add(new SqlParameter("@XMLDoc", request.ToXML()));
            SqlParameter newparam = command.Parameters.Add("@ReturnValue", SqlDbType.Int);
            newparam.Direction = ParameterDirection.ReturnValue;

            connection.Open();
            command.ExecuteNonQuery();
            result = (int)command.Parameters["@ReturnValue"].Value;
          }
        }
        oResponseData = new BuyerProfileAddUpdateResponseData(result);
      }
      catch (Exception ex)
      {
        oResponseData = new BuyerProfileAddUpdateResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

  }
}
