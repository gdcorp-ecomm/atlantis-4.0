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
      try
      {
        int numberOfRecords = 0;
        int numberOfPages = 0;
        List<ReceiptItem> receiptList = new List<ReceiptItem>(5);
        string procName = string.Empty;

        string connectionString = NetConnect.LookupConnectInfo(oConfig, ConnectLookupType.NetConnectionString);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          MyaOrderHistoryRequestData request = (MyaOrderHistoryRequestData)oRequestData;
          List<SqlParameter> _params = BuildSqlParametersForProc(request, out procName);
          using (SqlCommand command = new SqlCommand(procName, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            foreach (SqlParameter param in _params)
            {
              command.Parameters.Add(param);
            }
            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
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
                  string receiptId = !reader.IsDBNull(1) ? reader.GetString(1) : string.Empty;
                  DateTime receiptDate = !reader.IsDBNull(2) ? reader.GetDateTime(2) : DateTime.Now;
                  string transactionCurrency = !reader.IsDBNull(3) ? reader.GetString(3) : "";
                  int transactionTotal = !reader.IsDBNull(4) ? reader.GetInt32(4) : 0;
                  bool isRefunded = !reader.IsDBNull(5);
                  string orderSource = !reader.IsDBNull(6) ? reader.GetString(6) : string.Empty;
                  string detailXml = !reader.IsDBNull(7) ? reader.GetString(7) : string.Empty;

                  ReceiptItem item = new ReceiptItem(receiptId, receiptDate, transactionCurrency, transactionTotal, isRefunded, orderSource, detailXml);
                  receiptList.Add(item);

                }

              }
            }
          }
        }

        oResponseData = new MyaOrderHistoryResponseData(numberOfRecords, numberOfPages, receiptList);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new MyaOrderHistoryResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new MyaOrderHistoryResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

    private List<SqlParameter> BuildSqlParametersForProc(MyaOrderHistoryRequestData request, out string procName)
    {
      List<SqlParameter> _paramList = new List<SqlParameter>(8);

      string sDate = new DateTime(1995,1,1).ToShortDateString();
      if (request.StartDate != null && request.StartDate.Year > DateTime.MinValue.Year)
      {
        sDate = request.StartDate.ToShortDateString();
      }

      string eDate = DateTime.Now.ToShortDateString();
      if (request.EndDate != null && request.EndDate.Year > DateTime.MinValue.Year)
      {
        eDate = request.EndDate.ToShortDateString();
      }

      _paramList.Add(new SqlParameter("@shopperId", request.ShopperID));

      if (!string.IsNullOrWhiteSpace(request.DomainName))
      {
        _paramList.Add(new SqlParameter("@domain", request.DomainName));
        procName = StoredProcedure.ReceiptByDomain;
      }
      else if (request.ProductGroupId > 0)
      {
        _paramList.Add(new SqlParameter("@startDate", sDate));
        _paramList.Add(new SqlParameter("@endDate", eDate));
        _paramList.Add(new SqlParameter("@productGroupID", request.ProductGroupId));
        procName = StoredProcedure.ReceiptByProductGroupId;
      }
      else if (request.PaymentProfileId > 0)
      {
        _paramList.Add(new SqlParameter("@startDate", sDate));
        _paramList.Add(new SqlParameter("@endDate", eDate));
        _paramList.Add(new SqlParameter("@pp_shopperProfileID", request.PaymentProfileId));
        procName = StoredProcedure.ReceiptByPaymentProfileId;
      }
      else if (request.ProductTypeId > 0)
      {
        _paramList.Add(new SqlParameter("@startDate", sDate));
        _paramList.Add(new SqlParameter("@endDate", eDate));
        _paramList.Add(new SqlParameter("@gdshop_product_typeID", request.ProductTypeId));
        procName = StoredProcedure.ReceiptByProductTypeId;
      }
      else
      {
        _paramList.Add(new SqlParameter("@startDate", sDate));
        _paramList.Add(new SqlParameter("@endDate", eDate));
        procName = StoredProcedure.ReceiptByDate;
      }

      _paramList.Add(new SqlParameter("@pageno", request.PageNumber));
      _paramList.Add(new SqlParameter("@rowsperpage", request.RowsPerPage));
      _paramList.Add(new SqlParameter("@sortcol", request.SortByColumn));
      _paramList.Add(new SqlParameter("@sortdir", request.SortDirection));
      _paramList.Add(new SqlParameter("@returnAllFlag", request.ReturnAll));

      return _paramList;
    }

    private class StoredProcedure
    {
      public const string ReceiptByDate = "mya_receiptByShopperAndDate_sp";
      public const string ReceiptByDomain = "mya_receiptByShopperAndDomain_sp";
      public const string ReceiptByProductGroupId = "mya_receiptByShopperAndProductGroup_sp";
      public const string ReceiptByProductTypeId = "mya_receiptByShopperAndProductType_sp";
      public const string ReceiptByPaymentProfileId = "mya_receiptByShopperAndProfileID_sp";
    }

  }
}
