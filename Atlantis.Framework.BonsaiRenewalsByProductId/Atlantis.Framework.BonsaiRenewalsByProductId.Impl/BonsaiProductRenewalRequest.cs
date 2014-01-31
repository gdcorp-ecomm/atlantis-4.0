using Atlantis.Framework.BonsaiRenewalsByProductId.Interface;
using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.BonsaiRenewalsByProductId.Impl
{
  public class BonsaiProductRenewalRequest : IRequest
  {
    private const string CONFIG_STORED_PROCEDURE = "bonsai_renewal_pf_idGet_sp";
    private const string PRODUCT_ID_PARAM = "@gdshop_product_unifiedProductID";
    private const string PRIVATE_LABEL_ID_PARAM = "@PrivateLabelID";
    
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      BonsaiProductRenewalResponseData response;
      var request = (BonsaiProductRenewalRequestData)requestData;

      try
      {
        var renewalProductId = 0;
        using (var cn = new SqlConnection(NetConnect.LookupConnectInfo(config)))
        {
          cn.Open();

          using (var cmd = new SqlCommand(CONFIG_STORED_PROCEDURE, cn))
          {
            cmd.CommandTimeout = (int) request.RequestTimeout.TotalSeconds;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(SetSqlParameters(request));

            using (var reader = cmd.ExecuteReader())
            {
              if (reader.Read())
              {
                renewalProductId = reader.GetInt32(0);
              }
            }
          }
          cn.Close();
        }
        response = new BonsaiProductRenewalResponseData(renewalProductId);
      }
      catch (AtlantisException atlEx)
      {
        response = new BonsaiProductRenewalResponseData(atlEx);
      }
      catch (Exception ex)
      {
        var atlEx = new AtlantisException("BonsaiProductRenewalRequest.RequestHandler", 1, ex.Message,
          string.Format("UnifiedProductId={0}, PrivateLableId={1}", request.UnifiedProductId, request.PrivateLabelId));
        response = new BonsaiProductRenewalResponseData(atlEx);
      }

      return response;
    }

    #region SQL Parameter Handling

    private SqlParameter[] SetSqlParameters(BonsaiProductRenewalRequestData request)
    {
      var paramColl = new List<SqlParameter>
                        {
                          new SqlParameter(PRODUCT_ID_PARAM, request.UnifiedProductId),
                          new SqlParameter(PRIVATE_LABEL_ID_PARAM, request.PrivateLabelId),
                        };

      var paramArray = new SqlParameter[paramColl.Count];
      paramColl.CopyTo(paramArray, 0);

      return paramArray;
    }

    #endregion

  }
}
