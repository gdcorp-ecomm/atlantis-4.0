using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.RenewalsMyRenewingDomains.Interface;

namespace Atlantis.Framework.RenewalsMyRenewingDomains.Impl
{
  public class RenewalsMyRenewingDomainsRequest : IRequest
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
                "Database connection string lookup failed", "No Connection For RenewalsMyRenewingDomainsRequest");
        }

        RenewalsMyRenewingDomainsRequestData request = (RenewalsMyRenewingDomainsRequestData)oRequestData;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(_SHOPPERIDPARAM, request.ShopperID));
            command.Parameters.Add(new SqlParameter(_DAYSFROMEXPPARAM, request.DaysFromExpiration));
            command.CommandTimeout = (int)Math.Truncate(oRequestData.RequestTimeout.TotalSeconds);

            connection.Open();
            ds = new DataSet(Guid.NewGuid().ToString());
            SqlDataAdapter adp = new SqlDataAdapter(command);
            adp.Fill(ds);
          }
        }

        oResponseData = new RenewalsMyRenewingDomainsResponseData(ds);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new RenewalsMyRenewingDomainsResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new RenewalsMyRenewingDomainsResponseData(ds, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

    #region Private Methods

    private const string _PROCNAME = "gdshop_MyRenewalsRenewingDomainListByShopper_sp";
    private const string _SHOPPERIDPARAM = "shopper_ID";
    private const string _DAYSFROMEXPPARAM = "DaysFromExp";

    #endregion
  }
}
