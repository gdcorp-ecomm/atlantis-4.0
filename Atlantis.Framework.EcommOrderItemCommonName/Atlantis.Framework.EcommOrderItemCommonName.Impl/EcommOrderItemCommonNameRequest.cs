using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.EcommOrderItemCommonName.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommOrderItemCommonName.Impl
{
  public class EcommOrderItemCommonNameRequest : IRequest
  {
    #region Constants
    private const string LOOKUP_STORED_PROCEDURE_XML = "<RefundFindProc><param name='producttypeid' value='{0}'/></RefundFindProc>";
    private const string ORDER_ID_PARAM = "@order_id";
    private const string ROW_ID_PARAM = "@row_id";
    #endregion

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      string orderItemProc = string.Empty;
      string commonName = string.Empty;
      EcommOrderItemCommonNameResponseData responseData = null;

      try
      {
        var request = (EcommOrderItemCommonNameRequestData)requestData;

        if (!string.IsNullOrEmpty(orderItemProc = GetOrderItemCommonNameProc(request.ProductTypeId)))
        {
          using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
          {
            using (var cmd = new SqlCommand(orderItemProc, cn))
            {
              cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
              cmd.CommandType = CommandType.StoredProcedure;
              cmd.Parameters.Add(new SqlParameter(ORDER_ID_PARAM, request.OrderID));
              cmd.Parameters.Add(new SqlParameter(ROW_ID_PARAM, request.RowId));

              cn.Open();

              using (SqlDataReader dr = cmd.ExecuteReader())
              {
                if (dr.Read())
                {
                  commonName = dr["commonName"] == DBNull.Value ? string.Empty : dr["commonName"].ToString();
                }
              }
            }
          }
          responseData = new EcommOrderItemCommonNameResponseData(commonName);
        }
        else
        {
          Exception ex = new Exception(string.Format("No SQL procedure found for productTypeId: {0}.", request.ProductTypeId));
          responseData = new EcommOrderItemCommonNameResponseData(requestData, ex);
        }
      }

      catch (Exception ex)
      {
        responseData = new EcommOrderItemCommonNameResponseData(requestData, ex);
      }

      return responseData;
    }

    private string GetOrderItemCommonNameProc(int productTypeId)
    {
      return DataCache.DataCache.GetCacheData(string.Format(LOOKUP_STORED_PROCEDURE_XML, productTypeId));
    }
  }
}
