using System;
using System.Collections.Generic;
using Atlantis.Framework.BonsaiRenewalsByProductId.Interface;
using Atlantis.Framework.BonsaiRenewalsByProductId.Interface.Types;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BonsaiRenewalsByProductId.Impl
{
  public class BonsaiRenewalsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      BonsaiRenewalsResponseData response;
      var request = (BonsaiRenewalsRequestData) requestData;

      try
      {
        using (var service = new BonsaiRenewals.Service())
        {
          service.Url = ((WsConfigElement)config).WSURL;
          service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

          var renewalsResponse = service.GetRenewalOptionsByUnifiedProductId(request.UnifiedProductId, request.PrivateLabelId);

          response = BuildRenewalsResponse(renewalsResponse);
        }
      }
      catch (AtlantisException atlEx)
      {
        response = new BonsaiRenewalsResponseData(atlEx);
      }
      catch (Exception ex)
      {
        var atlEx = new AtlantisException(requestData, "BonsaiRenewalsRequest.RequestHandler", ex.Message,
                                          string.Format("UnifiedProductId={0}, PrivateLableId={1}", request.UnifiedProductId, request.PrivateLabelId), ex);
        response = new BonsaiRenewalsResponseData(atlEx);
      }

      return response;
    }

    private static BonsaiRenewalsResponseData BuildRenewalsResponse(BonsaiRenewals.RenewalResponse renewalResponse)
    {
      if (renewalResponse == null || renewalResponse.ResultCode != 0)
        throw new Exception("Unable to retrieve renewal options from Bonsai");

      if (renewalResponse.RenewalOptions == null)
        return null;
      
      var renewals = new List<BonsaiRenewalOption>();

      foreach (var renewalOption in renewalResponse.RenewalOptions)
      {
        renewals.Add(new BonsaiRenewalOption(renewalOption.UnifiedProductID,
                                             renewalOption.RenewalLength,
                                             renewalOption.RenewalType,
                                             renewalOption.MinRenewalPeriods,
                                             renewalOption.MaxRenewalPeriods));
      }

      return new BonsaiRenewalsResponseData(renewals);
    }
  }
}
