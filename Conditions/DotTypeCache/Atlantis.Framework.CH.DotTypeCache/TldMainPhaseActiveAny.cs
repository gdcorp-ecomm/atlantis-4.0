using System;
using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using System.Linq;

namespace Atlantis.Framework.CH.DotTypeCache
{
  public static class MainPhaseToken
  {
    public const string GeneralAvailability = "General";
    public const string PreRegistration = "PreReg";
    public const string Watchlist = "WatchList";
  }

  public enum MainPhases
  {
    General,
    PreReg,
    WatchList
  }

  public class TldMainPhaseActiveAny : IConditionHandler
  {
    public string ConditionName
    {
      get { return "tldMainPhaseActiveAny"; }
    }

    private static bool IsPreRegistration(IDotTypeInfo dotTypeInfo)
    {
      bool returnValue = false;
      if (!ReferenceEquals(null, dotTypeInfo) && !(dotTypeInfo is InvalidDotType))
      {
        foreach (LaunchPhases launchPhase in Enum.GetValues(typeof(LaunchPhases)))
        {
          returnValue = dotTypeInfo.IsLivePhase(launchPhase);
          if (returnValue)
          {
            break;
          }
        }
      }
      return returnValue;
    }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      bool returnValue = false;

      //Notes from discussions with Michael Melamud, Shaun Stair, Arvind Gurumurthi:
      //1. Check DotTypeCache
      //1.1 If Exists and is offered, it is General Availability
      //2. Check for any Pre-Registration Status (LA, GA, SR)
      //2.1 If it has a Pre-Registration Status, it's Pre-Registration
      //3. If It's not General Availability or Pre-Registration, it's Watchlist
      //3.1 Handler from Arvind will determine if the TLD is whitelisted.

      if (string.Equals(this.ConditionName, conditionName, StringComparison.OrdinalIgnoreCase) && !ReferenceEquals(null, parameters) && 2 <= parameters.Count)
      {
        string tld = parameters[0];

        if (!String.IsNullOrEmpty(tld))
        {
          tld = tld.Replace("'", string.Empty).Replace(@"""", string.Empty).ToUpperInvariant();

          IDotTypeProvider dotTypeProvider = providerContainer.Resolve<IDotTypeProvider>();
          IDotTypeInfo dotType =  dotTypeProvider.GetDotTypeInfo(tld);
          ITLDDataImpl tldData =  dotTypeProvider.GetTLDDataForRegistration;
          var phaseParams = parameters.Skip(1);

          bool isPreReg = IsPreRegistration(dotType);
          bool isGA = !ReferenceEquals(null, tldData) && tldData.IsOffered(tld) && !isPreReg;
          foreach (string phase in phaseParams)
          {
            MainPhases parsed;
            
            if (Enum.TryParse(phase, true, out parsed))
            {
              switch (parsed)
              {
                case MainPhases.General:
                  returnValue = isGA;
                  break;
                case MainPhases.PreReg:
                  returnValue = isPreReg;
                  break;
                case MainPhases.WatchList:
                  returnValue = !isPreReg && !isGA;
                  break;
              }

            }

            if (returnValue)
            {
              break;
            }
          }

        }
      }

      return returnValue;
    }
  }
}
