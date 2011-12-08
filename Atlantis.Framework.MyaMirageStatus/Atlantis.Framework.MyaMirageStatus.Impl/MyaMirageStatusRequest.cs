using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaMirageStatus.Interface;

namespace Atlantis.Framework.MyaMirageStatus.Impl
{
  public class MyaMirageStatusRequest : IRequest
  {
    private const string _PROCNAME = "mya_mirageStatusByShopper_sp";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      MyaMirageStatusRequestData mirageRequest = (MyaMirageStatusRequestData)oRequestData;
      MyaMirageStatusResponseData result;

      try
      {
        DateTime? lastMirageBuild = null;

        if (!string.IsNullOrEmpty(mirageRequest.ShopperID))
        {
          string connectionString = Nimitz.NetConnect.LookupConnectInfo(oConfig);
          using (SqlConnection connection = new SqlConnection(connectionString))
          {
            using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
            {
              command.CommandType = CommandType.StoredProcedure;
              command.Parameters.Add(new SqlParameter("shopper_id", mirageRequest.ShopperID));
              command.CommandTimeout = (int)mirageRequest.RequestTimeout.TotalSeconds;
              connection.Open();
              using (SqlDataReader reader = command.ExecuteReader())
              {
                if (reader.Read())
                {
                  object mirageLastBuildDate = reader[0];
                  if ((mirageLastBuildDate != null) && (mirageLastBuildDate.GetType() == typeof(DateTime)))
                  {
                    lastMirageBuild = (DateTime)mirageLastBuildDate;
                  }
                }
              }
            }
          }
        }

        result = new MyaMirageStatusResponseData(lastMirageBuild);
      }
      catch (Exception ex)
      {
        result = new MyaMirageStatusResponseData(ex, oRequestData);
      }

      return result;
    }

    #endregion
  }
}
