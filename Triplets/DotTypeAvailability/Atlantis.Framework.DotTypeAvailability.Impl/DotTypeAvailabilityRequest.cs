using System;
using System.Collections.Generic;
using Atlantis.Framework.DotTypeAvailability.Impl.TldAvailSvc;
using Atlantis.Framework.DotTypeAvailability.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeAvailability.Impl
{
  public class DotTypeAvailabilityRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DotTypeAvailabilityResponseData responseData;

      try
      {
        using (var tldAvailSvc = new BasicHttpBinding_ITldAvailSvc())
        {
          tldAvailSvc.Url = ((WsConfigElement)config).WSURL;
          tldAvailSvc.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
          TldDataResponse responseObject = tldAvailSvc.GetAvailableTldData();

          if (responseObject != null && responseObject.tldData != null && responseObject.tldData.tldList != null &&
              responseObject.tldData.tldList.Length > 0)
          {
            responseData = new DotTypeAvailabilityResponseData(ParseResponse(responseObject.tldData.tldList));
          }
          else
          {
            responseData = new DotTypeAvailabilityResponseData(new AtlantisException(requestData, "DotTypeAvailabilityRequest",
                                                                        "TldList has returned empty", string.Empty));
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new DotTypeAvailabilityResponseData(new AtlantisException(requestData,
                     "DotTypeAvailabilityRequest", ex.Message, string.Empty, ex));
      }

      return responseData;
    }

    private static IDictionary<string, ITldAvailability> ParseResponse(IEnumerable<TldType> tldTypes)
    {
      var tldAvailabilityList = new Dictionary<string, ITldAvailability>();
      foreach (var tldType in tldTypes)
      {
        TldAvailability tldAvailability = new TldAvailability
          {
            HasLeafPage = tldType.leafPage,
            IsVisibleInDomainSpins = tldType.isVisibleInDomainSpins,
            Name = tldType.name
          };

        IList<ITldPhase> tldPhases = new List<ITldPhase>(8);
        foreach (var phase in tldType.phases)
        {
          TldPhase tldPhase = new TldPhase {Name = phase.name, StartDate = phase.startDate, StopDate = phase.stopDate};
          tldPhases.Add(tldPhase);
        }

        tldAvailability.TldPhases = tldPhases;
        tldAvailabilityList.Add(tldAvailability.Name, tldAvailability);
      }

      return tldAvailabilityList;
    }
  }
}
