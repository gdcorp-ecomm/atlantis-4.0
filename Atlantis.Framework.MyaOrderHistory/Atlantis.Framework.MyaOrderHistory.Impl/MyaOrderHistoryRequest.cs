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
      string procName = string.Empty;
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

        List<SqlParameter> _params = BuildSqlParametersForProc(request, out procName);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(procName, connection))
          {
            command.CommandType = CommandType.StoredProcedure;

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
