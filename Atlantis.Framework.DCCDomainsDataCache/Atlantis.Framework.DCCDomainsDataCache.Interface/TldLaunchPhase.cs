using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TldLaunchPhase : ITLDLaunchPhase
  {
    private static readonly TldLaunchPhase _nullLaunchPhase;

    static TldLaunchPhase()
    {
      _nullLaunchPhase = FromNothing();
    }

    public static TldLaunchPhase NULLPHASE
    {
      get { return _nullLaunchPhase; }
    }

    public static TldLaunchPhase FromPhaseElement(XElement phaseElement)
    {
      return new TldLaunchPhase(phaseElement);
    }

    public static TldLaunchPhase FromNothing()
    {
      return null;
    }

    public string Code { get; private set; }
    public string Type { get; private set; }
    public string Value { get; private set; }

    private readonly HashSet<TldLaunchPhasePeriod> _periods;
    public IEnumerable<ITLDLaunchPhasePeriod> Periods
    {
      get { return _periods; }
    }

    private readonly HashSet<string> _duplicates;
    public IEnumerable<string> Duplicates
    {
      get { return _duplicates; }
    }
    
    public bool UpdatesEnabled { get; private set; }
    public bool RefundsEnabled { get; private set; }
    public bool PrivacyEnabled { get; private set; }

    private TldLaunchPhase(XElement phaseElement)
    {
      Code = phaseElement.Attribute("code").Value;
      Type = phaseElement.Attribute("type").Value;
      Value = phaseElement.Attribute("value").Value;

      //populate lauch phase periods
      _periods = new HashSet<TldLaunchPhasePeriod>();
      foreach (var period in phaseElement.Descendants("launchphaseperiod"))
      {
        var phasePeriod = new TldLaunchPhasePeriod
                            {
                              Type = period.Attribute("type").Value,
                              StartDate = DateTime.Parse(period.Attribute("utcstartdate").Value),
                              StopDate = DateTime.Parse(period.Attribute("utcstopdate").Value)
                            };

        XAttribute enabled = period.Attribute("availcheckenabled");
        if (enabled != null)
        {
          phasePeriod.AvailCheck = "true".Equals(enabled.Value, StringComparison.OrdinalIgnoreCase);
        }

        _periods.Add(phasePeriod);
      }

      //populate launch phase duplicates
      _duplicates = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
      foreach (var duplicate in phaseElement.Descendants("launchphaseduplicates"))
      {
        if (duplicate.IsEnabled())
        {
          _duplicates.Add(duplicate.Attribute("type").Value);
        }
      }

      var updateElement = phaseElement.Element("launchphaseupdates");
      if (updateElement != null)
      {
        UpdatesEnabled = updateElement.IsEnabled();
      }

      var refundElement = phaseElement.Element("launchphaserefunds");
      if (refundElement != null)
      {
        RefundsEnabled = refundElement.IsEnabled();
      }

      var privacyElement = phaseElement.Element("launchphaseprivacy");
      if (privacyElement != null)
      {
        PrivacyEnabled = privacyElement.IsEnabled();
      }
    }
  }
}
