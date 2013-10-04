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

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var numberOfRecords = 0;
      var numberOfPages = 0;
      var receiptList = new List<ReceiptItem>(5);

      var connectionString = NetConnect.LookupConnectInfo(config, ConnectLookupType.NetConnectionString);
      using (var connection = new SqlConnection(connectionString))
      {
        var request = (MyaOrderHistoryRequestData) requestData;
        string procName;
        var _params = BuildSqlParametersForProc(request, out procName);
        using (var command = new SqlCommand(procName, connection))
        {
          command.CommandType = CommandType.StoredProcedure;
          command.CommandTimeout = (int) request.RequestTimeout.TotalSeconds;
          foreach (var param in _params)
          {
            command.Parameters.Add(param);
          }
          connection.Open();

          using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
          {
            while (reader.HasRows)
            {
              while (reader.Read())
              {
                numberOfRecords = reader.GetInt32(0);
              }

              reader.NextResult();

              while (reader.Read())
              {
                numberOfPages = reader.GetInt32(0);
              }

              reader.NextResult();

              while (reader.Read())
              {
                var receiptId = !reader.IsDBNull(1) ? reader.GetString(1) : string.Empty;
                var receiptDate = !reader.IsDBNull(2) ? reader.GetDateTime(2) : DateTime.Now;
                var transactionCurrency = !reader.IsDBNull(3) ? reader.GetString(3) : "";
                var transactionTotal = !reader.IsDBNull(4) ? reader.GetInt32(4) : 0;
                var isRefunded = !reader.IsDBNull(5);
                var orderSource = !reader.IsDBNull(6) ? reader.GetString(6) : string.Empty;
                var detailXml = !reader.IsDBNull(7) ? reader.GetString(7) : string.Empty;
                var item = new ReceiptItem(receiptId, receiptDate, transactionCurrency, transactionTotal, isRefunded,
                                           orderSource, detailXml);
                receiptList.Add(item);
              }
            }
          }
        }
      }

      IResponseData responseData = new MyaOrderHistoryResponseData(numberOfRecords, numberOfPages, receiptList);

      return responseData;
    }

    #endregion

    private static IEnumerable<SqlParameter> BuildSqlParametersForProc(MyaOrderHistoryRequestData request, out string procName)
    {
      var paramList = new List<SqlParameter>(8);

      var sDate = new DateTime(1995,1,1).ToShortDateString();
      if (request.StartDate.Year > DateTime.MinValue.Year)
      {
        sDate = request.StartDate.ToShortDateString();
      }

      var eDate = DateTime.Now.ToShortDateString();
      if (request.EndDate.Year > DateTime.MinValue.Year)
      {
        eDate = request.EndDate.ToShortDateString();
      }

      paramList.Add(new SqlParameter("@shopperId", request.ShopperID));

      if (!string.IsNullOrEmpty(request.DomainName))
      {
        paramList.Add(new SqlParameter("@domain", request.DomainName));
        procName = StoredProcedure.ReceiptByDomain;
      }
      else if (request.ProductGroupId > 0)
      {
        paramList.Add(new SqlParameter("@startDate", sDate));
        paramList.Add(new SqlParameter("@endDate", eDate));
        paramList.Add(new SqlParameter("@productGroupID", request.ProductGroupId));
        procName = StoredProcedure.ReceiptByProductGroupId;
      }
      else if (request.PaymentProfileId > 0)
      {
        paramList.Add(new SqlParameter("@startDate", sDate));
        paramList.Add(new SqlParameter("@endDate", eDate));
        paramList.Add(new SqlParameter("@pp_shopperProfileID", request.PaymentProfileId));
        procName = StoredProcedure.ReceiptByPaymentProfileId;
      }
      else if (request.ProductTypeId > 0)
      {
        paramList.Add(new SqlParameter("@startDate", sDate));
        paramList.Add(new SqlParameter("@endDate", eDate));
        paramList.Add(new SqlParameter("@gdshop_product_typeID", request.ProductTypeId));
        procName = StoredProcedure.ReceiptByProductTypeId;
      }
      else if (!string.IsNullOrEmpty(request.OrderID))
      {
        paramList.Add(new SqlParameter("@order_id", request.OrderID));
        procName = StoredProcedure.ReceiptByOrderId;
      }
      else
      {
        paramList.Add(new SqlParameter("@startDate", sDate));
        paramList.Add(new SqlParameter("@endDate", eDate));
        procName = StoredProcedure.ReceiptByDate;
      }

      paramList.Add(new SqlParameter("@pageno", request.PageNumber));
      paramList.Add(new SqlParameter("@rowsperpage", request.RowsPerPage));
      paramList.Add(new SqlParameter("@sortcol", request.SortByColumn));
      paramList.Add(new SqlParameter("@sortdir", request.SortDirection));
      paramList.Add(new SqlParameter("@returnAllFlag", request.ReturnAll));

      return paramList;
    }

    private class StoredProcedure
    {
      public const string ReceiptByDate = "mya_receiptByShopperAndDate_sp";
      public const string ReceiptByDomain = "mya_receiptByShopperAndDomain_sp";
      public const string ReceiptByProductGroupId = "mya_receiptByShopperAndProductGroup_sp";
      public const string ReceiptByProductTypeId = "mya_receiptByShopperAndProductType_sp";
      public const string ReceiptByPaymentProfileId = "mya_receiptByShopperAndProfileID_sp";
      public const string ReceiptByOrderId = "mya_receiptByShopperAndOrderID_sp";
    }

  }
}
