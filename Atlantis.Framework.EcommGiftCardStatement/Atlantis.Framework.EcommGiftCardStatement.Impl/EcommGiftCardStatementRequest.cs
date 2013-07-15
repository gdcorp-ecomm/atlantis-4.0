using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.EcommGiftCardStatement.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.EcommGiftCardStatement.Impl
{
  public class EcommGiftCardStatementRequest : IRequest
  {
    private const string PROC_NAME = "reports_giftCardServiceStatement_sp";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      try
      {

        EcommGiftCardStatementRequestData request = oRequestData as EcommGiftCardStatementRequestData;

        int rowType = -1;
        string displayDate = string.Empty, description = string.Empty, amount = string.Empty;

        List<GiftCardTransaction> GiftCardTransactionList = new List<GiftCardTransaction>(2);

        string connectionString = NetConnect.LookupConnectInfo(oConfig, ConnectLookupType.NetConnectionString);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {

          using (SqlCommand command = new SqlCommand(PROC_NAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            command.Parameters.Add(new SqlParameter("@d_startDate", request.StartDate));
            command.Parameters.Add(new SqlParameter("@d_endDate", request.EndDate));
            command.Parameters.Add(new SqlParameter("@resource_id", request.ResourceId));

            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {

              while (reader.Read())
              {
                rowType = !reader.IsDBNull(reader.GetOrdinal("rowType")) ? reader.GetInt32(reader.GetOrdinal("rowType")) : -1;
                displayDate = !reader.IsDBNull(reader.GetOrdinal("displayDate")) ? reader.GetString(reader.GetOrdinal("displayDate")) : string.Empty;
                description = !reader.IsDBNull(reader.GetOrdinal("description")) ? reader.GetString(reader.GetOrdinal("description")) : string.Empty;
                amount = !reader.IsDBNull(reader.GetOrdinal("amount")) ? reader.GetString(reader.GetOrdinal("amount")) : string.Empty;

                GiftCardTransaction giftcardTransaction = new GiftCardTransaction(rowType, displayDate, description, amount);
                GiftCardTransactionList.Add(giftcardTransaction);
              }
            }
          }
        }


        oResponseData = new EcommGiftCardStatementResponseData(GiftCardTransactionList);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new EcommGiftCardStatementResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new EcommGiftCardStatementResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
