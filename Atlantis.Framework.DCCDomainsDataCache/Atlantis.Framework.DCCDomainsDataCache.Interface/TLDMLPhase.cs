using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TLDMLPhase : TLDMLNamespaceElement, ITLDPhase
  {
    protected override string Namespace
    {
      get { return "urn:godaddy:ns:phase"; }
    }

    protected override string LocalName
    {
      get { return "phase"; }
    }

    private readonly IList<ITLDLaunchPhase> _launchPhases;

    public TLDMLPhase(XDocument tldmlDoc) : base(tldmlDoc)
    {
      _launchPhases = Load();
    }

    private IList<ITLDLaunchPhase> Load()
    {
      var result = new List<ITLDLaunchPhase>(32);

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
              result.Add(TldLaunchPhase.FromPhaseElement(launchPhase));
            }
          }
        }
      }

      return result;
    }

    public IList<ITLDLaunchPhase> GetAllLaunchPhases(bool activeOnly = true)
    {
      var allPhases = new List<ITLDLaunchPhase>(_launchPhases.Count);

      foreach (var launchPhase in _launchPhases)
      {
        if ((launchPhase.PreRegistrationPeriod.IsActive || launchPhase.LivePeriod.IsActive) || !activeOnly)
        {
          allPhases.Add(launchPhase);
        }
      }

      return allPhases;
    }

    public ITLDLaunchPhase GetLaunchPhase(string launchPhaseCode)
    {
      var foundPhase = TldLaunchPhase.NullPhase;

      if (!string.IsNullOrEmpty(launchPhaseCode))
      {
        foreach (TldLaunchPhase tldLaunchPhase in _launchPhases)
        {
          if (tldLaunchPhase.Code.Equals(launchPhaseCode, StringComparison.OrdinalIgnoreCase))
          {
            foundPhase = tldLaunchPhase;
            break;
          }
        }
      }

      return foundPhase;
    }

    public bool IsPreRegPhaseActive 
    {
      get
      {
        bool isPreRegPhaseActive = false;

        foreach (var launchphase in _launchPhases)
        {
          bool isGeneralAvailability = launchphase.Code.Equals("GA", StringComparison.OrdinalIgnoreCase);

          if (isGeneralAvailability)
          {
            isPreRegPhaseActive = launchphase.PreRegistrationPeriod.IsActive;
          }
          else if (launchphase.LivePeriod.IsActive || launchphase.PreRegistrationPeriod.IsActive)
          {
            isPreRegPhaseActive = true;
          }

          if (isPreRegPhaseActive)
          {
            break;
          }
        }

        return isPreRegPhaseActive;
      }
    }
  }
}