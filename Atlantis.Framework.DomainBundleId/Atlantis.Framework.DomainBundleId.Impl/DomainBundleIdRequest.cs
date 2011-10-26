using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.DomainBundleId.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.DomainBundleId.Impl
{
  public class DomainBundleIdRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DomainBundleIdResponseData responseData = null;
      int? resourceId = null;
      int? pfId = null;

      try
      {
        DomainBundleIdRequestData domainBundleRequestData = (DomainBundleIdRequestData)requestData;
        string procName = "dbo.gdshop_billingDomainGetSupplementalBundleID_sp";

        using (SqlConnection cn = new SqlConnection(LookupConnectionString(domainBundleRequestData, config)))
        {
          using (SqlCommand cmd = new SqlCommand(procName, cn))
          {
            cmd.CommandTimeout = (int)domainBundleRequestData.RequestTimeout.TotalSeconds;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@domainID", domainBundleRequestData.DomainId));
            cn.Open();
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
              if (dr.Read())
              {
                resourceId = dr["resource_id"] != DBNull.Value ? (int?)Convert.ToInt32(dr["resource_id"]) : null;
                pfId = dr["pf_id"] != DBNull.Value ? (int?)Convert.ToInt32(dr["pf_id"]) : null;

                responseData = new DomainBundleIdResponseData(resourceId, pfId);
              }
            }
          }
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DomainBundleIdResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new DomainBundleIdResponseData(requestData, ex);
      }

      if (responseData == null)
      {
        responseData = new DomainBundleIdResponseData(resourceId, pfId);
      }

      return responseData;
    }

    private string LookupConnectionString(DomainBundleIdRequestData requestData, ConfigElement config)
    {
      string result = string.Empty;

      result = NetConnect.LookupConnectInfo(config.GetConfigValue("DataSourceName"), config.GetConfigValue("CertificateName"), config.GetConfigValue("ApplicationName"), "DomainBundleIdRequest.LookupConnectionString",
                                            ConnectLookupType.NetConnectionString);

      if (string.IsNullOrEmpty(result) || result.Length <= 2)
      {
        throw new Exception("Invalid Connection String");
      }
      return result;
    }
  }
}
