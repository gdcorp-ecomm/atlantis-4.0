using System;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TldLaunchPhase : ITLDLaunchPhase
  {
    private static readonly ITLDLaunchPhase _nullLaunchPhase;
    private static readonly ITLDLaunchPhase _generalAvailabilityActiveLaunchPhase = new TldLaunchPhase { Code = "GA", 
                                                                                                         ClaimPeriod = null,
                                                                                                         LivePeriod = new TldLaunchPhasePeriod { StartDateUtc = DateTime.UtcNow.AddYears(-1), StopDateUtc = DateTime.MaxValue },
                                                                                                         PreRegistrationPeriod = new TldLaunchPhasePeriod { StartDateUtc = DateTime.UtcNow.AddYears(-3), StopDateUtc = DateTime.UtcNow.AddYears(-2) },
                                                                                                         PrivacyEnabled = false,
                                                                                                         RefundsEnabled = true,
                                                                                                         Type = "General Availability",
                                                                                                         UpdatesEnabled = true,
                                                                                                         Value = "General Availability" };

    static TldLaunchPhase()
    {
      _nullLaunchPhase = FromNothing();
    }

    private TldLaunchPhase()
    {
    }

    private TldLaunchPhase(XElement phaseElement)
    {
      Code = phaseElement.Attribute("code").Value;
      Type = phaseElement.Attribute("type").Value;
      Value = phaseElement.Attribute("value").Value;

      ParsePeriods(phaseElement);
    }

    public static ITLDLaunchPhase NullPhase
    {
      get { return _nullLaunchPhase; }
    }

    public static ITLDLaunchPhase FromPhaseElement(XElement phaseElement)
    {
      return new TldLaunchPhase(phaseElement);
    }

    public static ITLDLaunchPhase FromNothing()
    {
      return null;
    }

    public static ITLDLaunchPhase GeneralAvailabilityActive()
    {
      return _generalAvailabilityActiveLaunchPhase;
    }

    public string Code { get; private set; }

    public string Type { get; private set; }

    public string Value { get; private set; }

    private ITLDLaunchPhasePeriod ClaimPeriod { get; set; }

    public ITLDLaunchPhasePeriod PreRegistrationPeriod { get; private set; }

    public ITLDLaunchPhasePeriod LivePeriod { get; private set; }

    public bool UpdatesEnabled { get; private set; }

    public bool RefundsEnabled { get; private set; }

    public bool PrivacyEnabled { get; private set; }

    public bool NeedsClaimCheck
    {
      get 
      { 
        bool needsClaimCheck = false;
        
        if (ClaimPeriod != null)
        {
          DateTime utcNow = DateTime.UtcNow;

          needsClaimCheck = utcNow >= ClaimPeriod.StartDateUtc && utcNow <= ClaimPeriod.StopDateUtc;
        }

        return needsClaimCheck;
      }
    }

    public bool AppFeeRefundsEnabled { get; private set; }

    private void ParsePeriods(XElement phaseElement)
    {
      DateTime preRegStart = DateTime.MinValue;
      DateTime preRegEnd = DateTime.MinValue;
      DateTime liveStart = DateTime.UtcNow.AddHours(-1);
      DateTime liveEnd = DateTime.MaxValue;

      foreach (var period in phaseElement.Descendants("launchphaseperiod"))
      {
        string periodType = period.Attribute("type").Value;

        if (periodType.Equals("clientrequest"))
        {
          preRegStart = DateTime.Parse(period.Attribute("utcstartdate").Value);
        }

        if (periodType.Equals("clientsubmission"))
        {
          preRegEnd = DateTime.Parse(period.Attribute("utcstartdate").Value);
          liveStart = DateTime.Parse(period.Attribute("utcstartdate").Value);
          liveEnd = DateTime.Parse(period.Attribute("utcstopdate").Value);
        }

        PreRegistrationPeriod = new TldLaunchPhasePeriod { StartDateUtc = preRegStart, StopDateUtc = preRegEnd };
        LivePeriod = new TldLaunchPhasePeriod { StartDateUtc = liveStart, StopDateUtc = liveEnd };

        if (periodType.Equals("claimsacceptance"))
        {
          DateTime claimStart = DateTime.Parse(period.Attribute("utcstartdate").Value);
          DateTime claimEnd = DateTime.Parse(period.Attribute("utcstopdate").Value);

          ClaimPeriod = new TldLaunchPhasePeriod { StartDateUtc = claimStart, StopDateUtc = claimEnd };
        }
      }

      var appFeeRefundElement = phaseElement.Element("launchphaserefunds");
      if (appFeeRefundElement != null)
      {
        AppFeeRefundsEnabled = appFeeRefundElement.IsEnabled();
      }
    }
  }
}