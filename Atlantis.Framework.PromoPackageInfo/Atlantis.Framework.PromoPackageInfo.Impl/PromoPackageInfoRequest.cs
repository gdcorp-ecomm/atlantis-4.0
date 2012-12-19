using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.PromoPackageInfo.Interface;

namespace Atlantis.Framework.PromoPackageInfo.Impl
{
  public class PromoPackageInfoRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      string procName = "prs_packageInfoGet_sp";
      PromoPackageInfoResponseData returnValue = new PromoPackageInfoResponseData(new List<PackageItem>(5));
      try
      {
        List<PackageItem> products = new List<PackageItem>(5);

        PromoPackageInfoRequestData request = requestData as PromoPackageInfoRequestData;
        if (request != null)
        {
          string connectionString = NetConnect.LookupConnectInfo(config);
          using (var connection = new SqlConnection(connectionString))
          {
            using (var cmd = new SqlCommand(procName, connection))
            {
              cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
              cmd.CommandType = CommandType.StoredProcedure;

              cmd.Parameters.Add(new SqlParameter("@n_prs_packageID", request.PackageId));

              connection.Open();

              using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
              {
                if (reader.HasRows)
                {
                  int packageDescriptionOrdinal = reader.GetOrdinal("packageDescription");
                  int injectedAmountOrdinal = reader.GetOrdinal("injectedAmount");
                  int includeFeesOrdinal = reader.GetOrdinal("includeFees");
                  int packageGroupIdOrdinal = reader.GetOrdinal("prs_packageGroupID");
                  int packageGroupDescriptionOrdinal = reader.GetOrdinal("packageGroupDescription");
                  int groupQuantityAllowedOrdinal = reader.GetOrdinal("groupQuantityAllowed");
                  int groupDisplayOrderOrdinal = reader.GetOrdinal("groupDisplayOrder");
                  int pfIdOrdinal = reader.GetOrdinal("catalog_productUnifiedProductID");
                  int quantityOrdinal = reader.GetOrdinal("quantity");
                  int durationOrdinal = reader.GetOrdinal("duration");

                  while (reader.Read())
                  {
                    PackageItem product = new PackageItem(
                      !reader.IsDBNull(packageDescriptionOrdinal) ? reader.GetString(packageDescriptionOrdinal) : string.Empty,
                      !reader.IsDBNull(injectedAmountOrdinal) ? reader.GetInt32(injectedAmountOrdinal) : 0,
                      !reader.IsDBNull(includeFeesOrdinal) ? reader.GetBoolean(includeFeesOrdinal) : false,
                      !reader.IsDBNull(packageGroupIdOrdinal) ? reader.GetInt32(packageGroupIdOrdinal) : 0,
                      !reader.IsDBNull(packageGroupDescriptionOrdinal) ? reader.GetString(packageGroupDescriptionOrdinal) : string.Empty,
                      !reader.IsDBNull(groupQuantityAllowedOrdinal) ? reader.GetInt32(groupQuantityAllowedOrdinal) : 0,
                      !reader.IsDBNull(groupDisplayOrderOrdinal) ? reader.GetInt32(groupDisplayOrderOrdinal) : 0,
                      !reader.IsDBNull(pfIdOrdinal) ? reader.GetInt32(pfIdOrdinal) : 0,
                      !reader.IsDBNull(quantityOrdinal) ? reader.GetInt32(quantityOrdinal) : 0,
                      !reader.IsDBNull(durationOrdinal) ? reader.GetDecimal(durationOrdinal) : 1
                      );
                    products.Add(product);
                  }
                }
                reader.Close();
              }
            }
          }
        }

        returnValue = new PromoPackageInfoResponseData(products);
      }
      catch (AtlantisException aex)
      {
        returnValue = new PromoPackageInfoResponseData(aex);
      }
      catch (Exception ex)
      {
        returnValue = new PromoPackageInfoResponseData(requestData, ex);
      }

      return returnValue;
    }
  }
}