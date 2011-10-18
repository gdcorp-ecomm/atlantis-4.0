using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaAccountList.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.MyaAccountList.Impl
{
  public class MyaAccountListRequest : IRequest
  {

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      MyaAccountListRequestData request = (MyaAccountListRequestData)oRequestData;

      DataSet ds = null;

      try
      {
        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        int totalRecords, totalPages;
        /*
         dbo.mya_accountListGetHosting_sp  
        ( @shopper_id  varchar(10),  
         @pageno   int = 1,  
         @rowsperpage  int = 10,  
         @sortcol  varchar(132) = 'commonName',  
         @sortdir  varchar(5) = 'ASC',  
         @returnAllFlag  bit = 0,  
         @daysTillExpiration int = NULL,  
         @free   int = NULL,  
         @commonNameFilter nvarchar(600) = NULL,  
         @totalrecords  int OUTPUT,  
         @totalpages  int OUTPUT  
         */
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(request.StoredProcName, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            command.Parameters.Add(new SqlParameter("@shopper_id", request.ShopperID));
            command.Parameters.Add(new SqlParameter("@sortdir", request.SortDirection));
            command.Parameters.Add(new SqlParameter("@pageno", request.PageInfo.CurrentPage));
            if (request.PageInfo.PageSize > 0)
            {
              command.Parameters.Add(new SqlParameter("@rowsperpage", request.PageInfo.PageSize));
            }
            if (request.Filter != null)
            {
              command.Parameters.Add(new SqlParameter("@commonNameFilter", request.Filter));
            }
            if (request.ReturnAll != 0)
            {
              command.Parameters.Add(new SqlParameter("@returnAllFlag", 1));
            }
            if (request.ReturnFreeListOnly != 0)
            {
              command.Parameters.Add(new SqlParameter("@free", 1));
            }
            if (request.DaysTillExpiration != null)
            {
              command.Parameters.Add(new SqlParameter("@daysTillExpiration", request.DaysTillExpiration));
            }

            SqlParameter prmTotalRecords = command.Parameters.Add("@totalrecords", SqlDbType.Int, 4);
            prmTotalRecords.Direction = ParameterDirection.Output;

            SqlParameter prmTotalPages = command.Parameters.Add("@totalpages", SqlDbType.Int, 4);
            prmTotalPages.Direction = ParameterDirection.Output;

            connection.Open();
            ds = new DataSet(Guid.NewGuid().ToString());
            SqlDataAdapter adp = new SqlDataAdapter(command);
            adp.Fill(ds);

            totalRecords = Convert.ToInt32(prmTotalRecords.Value.ToString());
            totalPages = Convert.ToInt32(prmTotalPages.Value.ToString());
          }
        }

        oResponseData = new MyaAccountListResponseData(ds, totalPages, totalRecords);
      }
      catch (Exception ex)
      {
        oResponseData = new MyaAccountListResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
