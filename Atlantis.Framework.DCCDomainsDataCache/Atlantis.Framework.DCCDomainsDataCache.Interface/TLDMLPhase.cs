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

    public Dictionary<string, ITLDLaunchPhase> GetActiveClientRequestPhases()
    {
      var launchPhases = new Dictionary<string, ITLDLaunchPhase>(StringComparer.OrdinalIgnoreCase);
      if (_launchPhases != null)
      {
        foreach (var phase in _launchPhases)
        {
          foreach (var period in phase.Value.Periods)
          {
            if (period.Type.Equals("clientrequest", StringComparison.OrdinalIgnoreCase) && period.IsActive())
            {
              launchPhases[phase.Key] = phase.Value;
              break;
            }
          }
        }
      }
      return launchPhases;
    }

    public ITLDLaunchPhase GetLaunchPhase(PreRegPhases preRegPhase)
    {
      var launchPhase = TldLaunchPhase.NULLPHASE;

      string phaseCode = PhaseHelper.GetPhaseCode(preRegPhase);
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
  }
}