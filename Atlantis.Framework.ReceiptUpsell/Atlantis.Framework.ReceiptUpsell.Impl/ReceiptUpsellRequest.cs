using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ReceiptUpsell.Interface;
using System.Data.SqlClient;
using System.Data;

namespace Atlantis.Framework.ReceiptUpsell.Impl
{
  public class ReceiptUpsellRequest : IRequest
  {
    private const string _PROCNAME = "gdshop_receipt_upsell_insert_sp";
    private const string _ORDERIDPARAM = "s_order_id";
    private const string _ROWIDPARAM = "n_row_id";
    private const string _PFIDPARAM = "n_pf_id";
    private const string _QUANTITYPARAM = "n_quantity";
    private const string _ADJUSTEDPRICEPARAM = "n_adjusted_price";
    private const string _NEWPFIDPARAM = "n_new_pf_id";
    private const string _NEWQUANTITYPARAM = "n_new_quantity";
    private const string _NEWADJUSTEDPRICEPARAM = "n_new_adjusted_price";
    private const string _OFFERDESCRIPTIONPARAM = "s_offerDescription";
    private const string _VISITGUIDPARAM = "s_visitGuid";
    private const string _PAGECOUNTPARAM = "n_pageCount";
    
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;

      try
      {
        ReceiptUpsellRequestData request = (ReceiptUpsellRequestData)oRequestData;

        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            int nonUnifiedProductId = DataCache.DataCache.GetPFIDByUnifiedID(request.ProductId, request.PrivateLabelId);
            int nonUnifiedNewProductId = DataCache.DataCache.GetPFIDByUnifiedID(request.NewProductId, request.PrivateLabelId);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(_ORDERIDPARAM, request.OrderID));
            command.Parameters.Add(new SqlParameter(_ROWIDPARAM, request.RowId));
            command.Parameters.Add(new SqlParameter(_PFIDPARAM, nonUnifiedProductId));
            command.Parameters.Add(new SqlParameter(_QUANTITYPARAM, request.Quantity));
            command.Parameters.Add(new SqlParameter(_ADJUSTEDPRICEPARAM, request.AdjustedPrice));
            command.Parameters.Add(new SqlParameter(_NEWPFIDPARAM, nonUnifiedNewProductId));
            command.Parameters.Add(new SqlParameter(_NEWQUANTITYPARAM, request.NewQuantity));
            command.Parameters.Add(new SqlParameter(_NEWADJUSTEDPRICEPARAM, request.NewAdjustedPrice));
            command.Parameters.Add(new SqlParameter(_OFFERDESCRIPTIONPARAM, request.OfferDescription));
            command.Parameters.Add(new SqlParameter(_VISITGUIDPARAM, request.Pathway));
            command.Parameters.Add(new SqlParameter(_PAGECOUNTPARAM, request.PageCount));
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;

            connection.Open();
            int result = command.ExecuteNonQuery();
            bool success = (result == -1);
            oResponseData = new ReceiptUpsellResponseData(success);
          }
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new ReceiptUpsellResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new ReceiptUpsellResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }

}
