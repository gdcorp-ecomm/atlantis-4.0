using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaOrderHistory.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.MyaOrderHistory.Impl
{
  public class MyaOrderHistoryRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      DataSet ds = null;

      try
      {
        string connectionString = NetConnect.LookupConnectInfo(oConfig, ConnectLookupType.NetConnectionString);
        //when an error occurs a ';' is returned not a valid connection string or empty
        if (connectionString.Length <= 1)
        {
          throw new AtlantisException(oRequestData, "LookupConnectionString",
                "Database connection string lookup failed", "No Connection For MyaOrderHistoryRequest");
        }

        MyaOrderHistoryRequestData request = (MyaOrderHistoryRequestData)oRequestData;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(request.StoredProcedureName, connection))
          {
            command.CommandType = CommandType.StoredProcedure;

            List<SqlParameter> _params = BuildSqlParametersForProc(request);
            foreach (SqlParameter param in _params)
            {
              command.Parameters.Add(param);
            }
            
            command.CommandTimeout = (int)Math.Truncate(request.RequestTimeout.TotalSeconds);

            connection.Open();
            ds = new DataSet(Guid.NewGuid().ToString());
            SqlDataAdapter adp = new SqlDataAdapter(command);
            adp.Fill(ds);
          }
        }

        oResponseData = new MyaOrderHistoryResponseData(ds);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new MyaOrderHistoryResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new MyaOrderHistoryResponseData(ds, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

    private List<SqlParameter> BuildSqlParametersForProc(MyaOrderHistoryRequestData request)
    {
      List<SqlParameter> _paramList = new List<SqlParameter>(8);
      _paramList.Add(new SqlParameter("@shopperId", request.ShopperID));
      switch (request.StoredProcedureName)
      {
        case StoredProcedure.ReceiptByDate:
          _paramList.Add(new SqlParameter("@startDate", request.StartDate.ToShortDateString()));
          _paramList.Add(new SqlParameter("@endDate", request.EndDate.ToShortDateString()));
          break;
        case StoredProcedure.ReceiptByDomain:
          _paramList.Add(new SqlParameter("@domain", request.DomainName));
          break;
        case StoredProcedure.ReceiptByProductGroupId:
          _paramList.Add(new SqlParameter("@startDate", request.StartDate.ToShortDateString()));
          _paramList.Add(new SqlParameter("@endDate", request.EndDate.ToShortDateString()));
          _paramList.Add(new SqlParameter("@productGroupID", request.ProductGroupId));
          break;
        case StoredProcedure.ReceiptByProductTypeId:
          _paramList.Add(new SqlParameter("@startDate", request.StartDate.ToShortDateString()));
          _paramList.Add(new SqlParameter("@endDate", request.EndDate.ToShortDateString()));
          _paramList.Add(new SqlParameter("@gdshop_product_typeID", request.ProductTypeId));
          break;
        case StoredProcedure.ReceiptByPaymentProfileId:
          _paramList.Add(new SqlParameter("@startDate", request.StartDate.ToShortDateString()));
          _paramList.Add(new SqlParameter("@endDate", request.EndDate.ToShortDateString()));
          _paramList.Add(new SqlParameter("@pp_shopperProfileID", request.PaymentProfileId));
          break;
      }
      _paramList.Add(new SqlParameter("@pageno", request.PageNumber));
      _paramList.Add(new SqlParameter("@rowsperpage", request.RowsPerPage));
      _paramList.Add(new SqlParameter("@sortcol", request.SortByColumn));
      _paramList.Add(new SqlParameter("@sortdir", request.SortDirection));
      _paramList.Add(new SqlParameter("@returnAllFlag", request.ReturnAll));

      return _paramList;
    }
  }
}
