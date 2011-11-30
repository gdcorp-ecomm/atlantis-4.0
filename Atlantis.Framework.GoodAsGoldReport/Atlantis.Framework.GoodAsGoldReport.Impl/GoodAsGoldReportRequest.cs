using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.GoodAsGoldReport.Interface;
using Atlantis.Framework.GoodAsGoldReport.Interface.Enum;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using System.Collections.Generic;
using System.Linq;

namespace Atlantis.Framework.GoodAsGoldReport.Impl
{
  public class GoodAsGoldReportRequest : IRequest
  {
    private const string PROC_NAME = "reports_prepaidServiceStatementPaged_sp";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      GoodAsGoldReportRequestData request = (GoodAsGoldReportRequestData)oRequestData;

      DataSet ds = null;

      try
      {
        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        int totalRecords, totalPages;
        string begBal, endBal, curBal;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROC_NAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            command.Parameters.Add(new SqlParameter("@d_startDate", request.StateDate));
            command.Parameters.Add(new SqlParameter("@d_endDate", request.EndDate));
            command.Parameters.Add(new SqlParameter("@s_shopper_id", request.ShopperID));
            command.Parameters.Add(new SqlParameter("@sortcol", request.SortColumn));
            command.Parameters.Add(new SqlParameter("@sortdir", request.SortDirection));
            command.Parameters.Add(new SqlParameter("@pageno", request.PageInfo.CurrentPage));

            if (request.Filter != "none")
            {
              command.Parameters.Add(new SqlParameter("@transactiontype", request.Filter));
            }

            if (request.PageInfo.PageSize > 0)
            {
              command.Parameters.Add(new SqlParameter("@rowsperpage", request.PageInfo.PageSize));  
            }

            SqlParameter prmTotalRecords = command.Parameters.Add("@totalrecords", SqlDbType.Int, 4);
            prmTotalRecords.Direction = ParameterDirection.Output;

            SqlParameter prmTotalPages = command.Parameters.Add("@totalpages", SqlDbType.Int, 4);
            prmTotalPages.Direction = ParameterDirection.Output;

            SqlParameter prmBegBalance = command.Parameters.Add("@BeginningBalance", SqlDbType.VarChar, 20);
            prmBegBalance.Direction = ParameterDirection.Output;

            SqlParameter prmEndBalance = command.Parameters.Add("@EndingBalance", SqlDbType.VarChar, 20);
            prmEndBalance.Direction = ParameterDirection.Output;

            SqlParameter prmCurBalance = command.Parameters.Add("@CurrentBalance", SqlDbType.VarChar, 20);
            prmCurBalance.Direction = ParameterDirection.Output;

            connection.Open();
            ds = new DataSet(Guid.NewGuid().ToString());
            SqlDataAdapter adp = new SqlDataAdapter(command);
            adp.Fill(ds);

            totalRecords = Convert.ToInt32(prmTotalRecords.Value.ToString());
            totalPages = Convert.ToInt32(prmTotalPages.Value.ToString());
            begBal = prmBegBalance.Value.ToString();
            endBal = prmEndBalance.Value.ToString();
           
            if (prmCurBalance.Value != System.DBNull.Value)
            {
              curBal = prmCurBalance.Value.ToString();
            }
            else
            {
              curBal = "$0.00";
            }         
          }
        }

        oResponseData = new GoodAsGoldReportResponseData(ds, totalPages, totalRecords, begBal, endBal, curBal);
      }
      catch (Exception ex)
      {
        oResponseData = new GoodAsGoldReportResponseData(oRequestData, ex);      
      }

      return oResponseData;
    }

    #endregion

  }
}
