using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaRecentNamespaces.Interface;

namespace Atlantis.Framework.MyaRecentNamespaces.Impl
{
  public class MyaRecentNamespacesRequest : IRequest
  {
    private const string _PROCNAME = "mya_getCurrentProductNameSpaces_sp";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      MyaRecentNamespacesRequestData namespacesRequest = (MyaRecentNamespacesRequestData)oRequestData;
      MyaRecentNamespacesResponseData result;

      try
      {
        HashSet<string> namespaceSet = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        if (!string.IsNullOrEmpty(namespacesRequest.ShopperID))
        {
          DateTime fromDate = DateTime.Now.AddDays(-2); // sets a limit to how far back we can search
          if (namespacesRequest.FromDate > fromDate)
          {
            fromDate = namespacesRequest.FromDate;
          }

          string connectionString = Nimitz.NetConnect.LookupConnectInfo(oConfig);
          using (SqlConnection connection = new SqlConnection(connectionString))
          {
            using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
            {
              command.CommandType = CommandType.StoredProcedure;
              command.Parameters.Add(new SqlParameter("s_shopper_id", namespacesRequest.ShopperID));
              command.Parameters.Add(new SqlParameter("d_lastEntryDate", fromDate));
              command.CommandTimeout = (int)namespacesRequest.RequestTimeout.TotalSeconds;
              connection.Open();
              using (SqlDataReader reader = command.ExecuteReader())
              {
                while (reader.Read())
                {
                  object namespaceValue = reader["nameSpace"];
                  if ((namespaceValue != null) && (namespaceValue.GetType() != typeof(DBNull)))
                  {
                    namespaceSet.Add(namespaceValue.ToString());
                  }
                }
              }
            }
          }
        }

        result = new MyaRecentNamespacesResponseData(namespaceSet);
      }
      catch (Exception ex)
      {
        result = new MyaRecentNamespacesResponseData(ex, oRequestData);
      }

      return result;
    }

    #endregion
  }
}
