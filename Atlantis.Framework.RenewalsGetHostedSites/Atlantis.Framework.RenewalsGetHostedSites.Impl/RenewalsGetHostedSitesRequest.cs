using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.RenewalsGetHostedSites.Interface;

namespace Atlantis.Framework.RenewalsGetHostedSites.Impl
{
  public class RenewalsGetHostedSitesRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      DataSet ds = null;

      try
      {
        string connectionString = NetConnect.LookupConnectInfo(oConfig);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            RenewalsGetHostedSitesRequestData request = (RenewalsGetHostedSitesRequestData)oRequestData;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(_SHOPPERID, request.ShopperID));
            command.Parameters.Add(new SqlParameter(_PAGENO, request.PageNumber));
            command.Parameters.Add(new SqlParameter(_ROWSPERPAGE, request.RowsPerPage));
            command.Parameters.Add(new SqlParameter(_SORTCOL, request.SortColumn));
            command.Parameters.Add(new SqlParameter(_SORTDIR, request.SortDirection));
            command.CommandTimeout = (int)Math.Truncate(oRequestData.RequestTimeout.TotalSeconds);

            connection.Open();
            ds = new DataSet(Guid.NewGuid().ToString());
            SqlDataAdapter adp = new SqlDataAdapter(command);
            adp.Fill(ds);
          }
        }

        oResponseData = new RenewalsGetHostedSitesResponseData(ds);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new RenewalsGetHostedSitesResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new RenewalsGetHostedSitesResponseData(ds, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

    #region Private Methods

    private const string _PROCNAME = "MyRenewalsGetHostedSites_sp";
    private const string _SHOPPERID = "s_shopper_id";
    private const string _PAGENO = "pageno";
    private const string _ROWSPERPAGE = "rowsperpage";
    private const string _SORTCOL = "sortcol";
    private const string _SORTDIR = "sortdir";

    #endregion
  }
}
