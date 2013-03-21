using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
// ReSharper disable InconsistentNaming
  public class TLDMLPreRegistration : TLDMLNamespaceElement, ITLDPreRegistration  
// ReSharper restore InconsistentNaming
  {
    protected override string Namespace
    {
      get { return "urn:godaddy:ns:preregistration"; }
    }

    protected override string LocalName
    {
      get { return "preregistration"; }
    }

    private readonly Dictionary<string, TldPreRegistrationPhase> _preregistrationPhases;

    public TLDMLPreRegistration(XDocument tldmlDoc)
      : base(tldmlDoc)
    {
      _preregistrationPhases = Load();
    }

    private Dictionary<string, TldPreRegistrationPhase> Load()
    {
      var result = new Dictionary<string, TldPreRegistrationPhase>(StringComparer.OrdinalIgnoreCase);

      var preregistrationPhaseCollections = NamespaceElement.Descendants("preregistrationphasecollection");
      foreach (XElement preregPhaseCollection in preregistrationPhaseCollections)
      {
        foreach (var preregistrationPhase in preregPhaseCollection.Descendants("preregistrationphase"))
        {
          if (preregistrationPhase.IsEnabled())
          {
            XAttribute typeAtt = preregistrationPhase.Attribute("type");
            XAttribute subtypeAtt = preregistrationPhase.Attribute("subtype");

            if (typeAtt != null && subtypeAtt != null)
            {
              result[typeAtt.Value + "::" + subtypeAtt.Value] = TldPreRegistrationPhase.FromPhaseElement(preregistrationPhase);
            }          
          }
        }
      }

      return result;
    }

    public bool IsValidPreRegistrationPhase(string type, string subType, out ITLDPreRegistrationPhase preRegistrationPhase)
    {
      var found = false;
      preRegistrationPhase = TldPreRegistrationPhase.NULLPHASE;

      TldPreRegistrationPhase result;
      if (_preregistrationPhases.TryGetValue(type + "::" + subType, out result))
      {
        found = true;
        preRegistrationPhase = result;
      }

      return found;
    }
  }
}
