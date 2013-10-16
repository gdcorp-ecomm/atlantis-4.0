using System;
using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CH.DotTypeCache
{
  public class TldPhaseActiveAny : IConditionHandler
  {
    public string ConditionName { get { return "tldPhaseActiveAny"; } }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      bool isAnyPhaseActive = false;

      if (parameters.Count >= 2)
      {
        try
        {
          var dotTypeProvider = providerContainer.Resolve<IDotTypeProvider>();
          var formatDotType = parameters[0].ToUpperInvariant();
          var dotType = dotTypeProvider.GetDotTypeInfo(formatDotType);

          if (!(dotType is InvalidDotType))
          {
            ITLDLaunchPhaseGroupCollection launchPhaseGroupCollection = dotType.GetAllLaunchPhaseGroups();

            for (int i = 1; i < parameters.Count; i++)
            {
              isAnyPhaseActive = IsPhaseActive(parameters[i], launchPhaseGroupCollection);

              if (isAnyPhaseActive)
              {
                break;
              }
            }
          }
          else
          {
            var aex = new AtlantisException("tldPhaseActiveAny EvaluateCondition", "0", "first parameter must be a TLD", string.Empty, null, null);
            Engine.Engine.LogAtlantisException(aex);
          }
        }
        catch (Exception ex)
        {
          var aex = new AtlantisException("tldPhaseActiveAny EvaluateCondition", "0", ex.Message + Environment.NewLine + ex.StackTrace, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(aex);
        }
      }
      else
      {
        var aex = new AtlantisException("tldPhaseActiveAny EvaluateCondition", "0", "must have at least two parameters", string.Empty, null, null);
        Engine.Engine.LogAtlantisException(aex);
      }

      return isAnyPhaseActive;
    }

    private bool IsPhaseActive(string phase, ITLDLaunchPhaseGroupCollection launchPhaseGroupCollection)
    {
      bool isPhaseActive = false;

      ITLDLaunchPhaseGroup launchPhaseGroup;

      switch (phase.ToUpperInvariant())
      {
        case TldPhaseActiveAnyPhaseTypes.GeneralAvailability:
          if (launchPhaseGroupCollection.TryGetGroup(LaunchPhaseGroupTypes.GeneralAvailability, out launchPhaseGroup))
          {
            isPhaseActive = launchPhaseGroup.Phases[0].LivePeriod.IsActive;
          }
          break;
        case TldPhaseActiveAnyPhaseTypes.PreRegGeneralAvailability:
          if (launchPhaseGroupCollection.TryGetGroup(LaunchPhaseGroupTypes.GeneralAvailability, out launchPhaseGroup))
          {
            isPhaseActive = launchPhaseGroup.Phases[0].PreRegistrationPeriod.IsActive;
          }
          break;
        case TldPhaseActiveAnyPhaseTypes.Landrush:
          isPhaseActive = IsLaunchPhaseGroupActive(LaunchPhaseGroupTypes.Landrush, launchPhaseGroupCollection);
          break;
        case TldPhaseActiveAnyPhaseTypes.EarlyRegistration:
          isPhaseActive = IsLaunchPhaseGroupActive(LaunchPhaseGroupTypes.EarlyRegistration, launchPhaseGroupCollection);
          break;
        case TldPhaseActiveAnyPhaseTypes.Sunrise:
          isPhaseActive = IsLaunchPhaseGroupActive(LaunchPhaseGroupTypes.Sunrise, launchPhaseGroupCollection);
          break;
      }

      return isPhaseActive;
    }

    private bool IsLaunchPhaseGroupActive(LaunchPhaseGroupTypes groupType, ITLDLaunchPhaseGroupCollection launchPhaseGroupCollection)
    {
      bool isActive = false;

      ITLDLaunchPhaseGroup group;

      if (launchPhaseGroupCollection.TryGetGroup(groupType, out group))
      {
        isActive = group.Phases[0].PreRegistrationPeriod.IsActive || group.Phases[0].LivePeriod.IsActive;
      }

      return isActive;
    }
  }
}