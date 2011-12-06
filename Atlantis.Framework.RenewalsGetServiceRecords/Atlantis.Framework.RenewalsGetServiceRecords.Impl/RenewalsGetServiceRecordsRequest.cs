using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.RenewalsGetServiceRecords.Interface;

namespace Atlantis.Framework.RenewalsGetServiceRecords.Impl
{
  public class RenewalsGetServiceRecordsRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      DataSet ds = null;

      try
      {
        RenewalsGetServiceRecordsRequestData request = (RenewalsGetServiceRecordsRequestData)oRequestData;

        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(_SHOPPERIDPARAM, request.ShopperID));
            command.Parameters.Add(new SqlParameter(_DAYSPARAM, request.DaysToExpiration));
            command.Parameters.Add(new SqlParameter(_PAGENUMPARAM, request.PageNumber));
            command.Parameters.Add(new SqlParameter(_ROWSPERPAGEPARAM, request.RowsPerPage));
            command.Parameters.Add(new SqlParameter(_SORTCOLPARAM, request.SortColumn));
            command.Parameters.Add(new SqlParameter(_SORTDIRPARAM, request.SortDirection));
            command.Parameters.Add(new SqlParameter(_ALLRECORDSPARAM, request.AllRecords));
            command.Parameters.Add(new SqlParameter(_SYNCABLEONLYPARAM, request.SyncableOnly));
            command.Parameters.Add(new SqlParameter(_ISCDATEPARAM, request.PromoStartDate.ToString("d")));
            command.CommandTimeout = (int)Math.Truncate(request.RequestTimeout.TotalSeconds);

            connection.Open();
            ds = new DataSet(Guid.NewGuid().ToString());
            SqlDataAdapter adp = new SqlDataAdapter(command);
            adp.Fill(ds);
          }
        }

        oResponseData = new RenewalsGetServiceRecordsResponseData(ds);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new RenewalsGetServiceRecordsResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new RenewalsGetServiceRecordsResponseData(ds, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

    #region Private Methods

    private const string _PROCNAME = "gdshop_getMyRenewalsRenewingServiceRecords_sp";
    private const string _SHOPPERIDPARAM = "shopper_id";
    private const string _DAYSPARAM = "days";
    private const string _PAGENUMPARAM = "pageno";
    private const string _ROWSPERPAGEPARAM = "rowsperpage";
    private const string _SORTCOLPARAM = "sortcol";
    private const string _SORTDIRPARAM = "sortdir";
    private const string _ALLRECORDSPARAM = "allRecords";
    private const string _SYNCABLEONLYPARAM = "syncAbleOnly";
    private const string _ISCDATEPARAM = "iscDate";

    #endregion
  }
}
