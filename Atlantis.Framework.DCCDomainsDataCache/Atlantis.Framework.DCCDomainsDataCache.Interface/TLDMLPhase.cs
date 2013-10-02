using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  // ReSharper disable InconsistentNaming
  public class TLDMLPhase : TLDMLNamespaceElement, ITLDPhase
  // ReSharper restore InconsistentNaming
  {
    protected override string Namespace
    {
      get { return "urn:godaddy:ns:phase"; }
    }

    protected override string LocalName
    {
      get { return "phase"; }
    }

    private readonly Dictionary<string, TldLaunchPhase> _launchPhases;

    public TLDMLPhase(XDocument tldmlDoc)
      : base(tldmlDoc)
    {
      _launchPhases = Load();
    }

    private Dictionary<string, TldLaunchPhase> Load()
    {
      var result = new Dictionary<string, TldLaunchPhase>(StringComparer.OrdinalIgnoreCase);

      var launchPhaseCollections = NamespaceElement.Descendants("launchphasecollection");
      foreach (XElement launchPhaseCollection in launchPhaseCollections)
      {
        foreach (var launchPhase in launchPhaseCollection.Descendants("launchphase"))
        {
          if (launchPhase.IsEnabled())
          {
            XAttribute typeAtt = launchPhase.Attribute("code");

            if (typeAtt != null)
            {
              result[typeAtt.Value] = TldLaunchPhase.FromPhaseElement(launchPhase);
            }
          }
        }
      }

      return result;
    }

    public Dictionary<string, ITLDLaunchPhasePeriod> GetAllLaunchPhases(bool activeOnly = false)
    {
      var allPhases = new Dictionary<string, ITLDLaunchPhasePeriod>(StringComparer.OrdinalIgnoreCase);

      foreach (var launchPhase in _launchPhases)
      {
        ITLDLaunchPhasePeriod launchPhasePeriod;

        if (launchPhase.Value.TryGetLaunchPhasePeriod("clientrequest", out launchPhasePeriod))
        {
          if ( (activeOnly && launchPhasePeriod.IsActive()) || !activeOnly)
          {
            allPhases[launchPhase.Key] = launchPhasePeriod;
          }
        }
      }

      return allPhases;
    }

    public ITLDLaunchPhase GetLaunchPhase(LaunchPhases phase)
    {
      var launchPhase = TldLaunchPhase.NULLPHASE;

      string phaseCode = PhaseHelper.GetPhaseCode(phase);
      if (!string.IsNullOrEmpty(phaseCode))
      {
        TldLaunchPhase result;
        if (_launchPhases.TryGetValue(phaseCode, out result))
        {
          launchPhase = result;
        }
      }

      return launchPhase;
    }

    public bool HasPreRegPhases 
    {
      get
      {
        bool result = false;

        foreach (var launchphase in _launchPhases)
        {
          ITLDLaunchPhasePeriod clientRequestPhasePeriod;
          ITLDLaunchPhasePeriod serverSubmissionPhasePeriod;
          launchphase.Value.TryGetLaunchPhasePeriod("clientrequest", out clientRequestPhasePeriod);
          launchphase.Value.TryGetLaunchPhasePeriod("serversubmission", out serverSubmissionPhasePeriod);

          if (clientRequestPhasePeriod != null && clientRequestPhasePeriod.IsActive())
          {
            if (serverSubmissionPhasePeriod != null && (DateTime.UtcNow < serverSubmissionPhasePeriod.StartDate))
            {
              result = true;
              break;
            }
          }
        }
        return result;
      }
    }
  }
}