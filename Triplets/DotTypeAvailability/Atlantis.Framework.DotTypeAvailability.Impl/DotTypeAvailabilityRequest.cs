using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.DotTypeAvailability.Impl.TldAvailSvc;
using Atlantis.Framework.DotTypeAvailability.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeAvailability.Impl
{
  public class DotTypeAvailabilityRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;

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
            responseData = DotTypeAvailabilityResponseData.FromTldAvailabilityDictionary(ParseResponse(responseObject.tldData.tldList));
          }
          else
          {
            var ex = new Exception("DotTypeAvailabilityRequest:TldList has returned empty");
            responseData = DotTypeAvailabilityResponseData.FromException(requestData, ex);
          }
        }
      }
      catch (Exception ex)
      {
        responseData = DotTypeAvailabilityResponseData.FromException(requestData, ex);
      }

      return responseData;
    }

    private static IDictionary<string, ITldAvailability> ParseResponse(IEnumerable<TldType> tldTypes)
    {
      var tldAvailabilityList = new Dictionary<string, ITldAvailability>(StringComparer.OrdinalIgnoreCase);
      foreach (var tldType in tldTypes)
      {
        var tldAvailability = new TldAvailability
          {
            HasLeafPage = tldType.leafPage,
            IsVisibleInDomainSpins = tldType.isVisibleInDomainSpins,
            TldName = tldType.name,
            TldPunyCodeName = tldType.aLabel
          };

        IList<ITldPhase> tldPhases = new List<ITldPhase>(8);
        foreach (var phase in tldType.phases)
        {
          DateTime startDate;
          DateTime.TryParse(phase.startDate, out startDate);

          DateTime stopDate;
          DateTime.TryParse(phase.stopDate, out stopDate);

          var tldPhase = new TldPhase { Name = phase.name };
          if (startDate != DateTime.MinValue)
          {
            tldPhase.StartDate = startDate;
          }
          if (stopDate != DateTime.MinValue)
          {
            tldPhase.StopDate = stopDate;
          }

          tldPhases.Add(tldPhase);
        }

        tldAvailability.TldPhases = tldPhases;

        if (!ContainsUnicodeCharacter(tldAvailability.TldName))
        {
          tldAvailabilityList.Add(tldAvailability.TldName, tldAvailability);
        }
        else
        {
          tldAvailabilityList.Add(tldAvailability.TldPunyCodeName, tldAvailability);
        }
      }

      return tldAvailabilityList;
    }

    private static bool ContainsUnicodeCharacter(string input)
    {
      const int maxAnsiCode = 255;
      return input.Any(c => c > maxAnsiCode);
    }
  }
}
