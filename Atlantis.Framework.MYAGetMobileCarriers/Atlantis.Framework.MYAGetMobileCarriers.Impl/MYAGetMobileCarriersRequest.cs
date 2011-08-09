using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MYAGetMobileCarriers.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.MYAGetMobileCarriers.Impl
{
  public class MYAGetMobileCarriersRequest : IRequest
  {
    private const string PROCNAME = "gdshop_getmktgMobileCarrier_sp";


    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      MYAGetMobileCarriersRequestData request = (MYAGetMobileCarriersRequestData)oRequestData;

      DataSet ds = null;
      List<MobileCarrierItem> carrierList = new List<MobileCarrierItem>();

      try
      {
        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;

            connection.Open();
            ds = new DataSet(Guid.NewGuid().ToString());
            SqlDataAdapter adp = new SqlDataAdapter(command);
            adp.Fill(ds);

            carrierList = GetObjectListFromDataset(ds);
          }
        }

        oResponseData = new MYAGetMobileCarriersResponseData(carrierList);
      }
      catch (Exception ex)
      {
        oResponseData = new MYAGetMobileCarriersResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

    private List<MobileCarrierItem> GetObjectListFromDataset(DataSet ds)
    {
      List<MobileCarrierItem> carrierList = new List<MobileCarrierItem>();

      if (ds.Tables.Count > 0)
      {
        foreach (DataRow row in ds.Tables[0].Rows)
        {
          MobileCarrierItem item = new MobileCarrierItem();

          if (IsColumnNotNull("gdshop_mktgMobileCarrierID", row))
          {
            item.MobileCarrierID = Convert.ToInt32(row["gdshop_mktgMobileCarrierID"]);
            item.Description = Convert.ToString(row["description"]);
          }
          carrierList.Add(item);
        }
      }

      return carrierList;
    }

    private bool IsColumnNotNull(string columnName, DataRow row)
    {
      return row[columnName] != null && !(row[columnName] is DBNull);
    }
  }
}
