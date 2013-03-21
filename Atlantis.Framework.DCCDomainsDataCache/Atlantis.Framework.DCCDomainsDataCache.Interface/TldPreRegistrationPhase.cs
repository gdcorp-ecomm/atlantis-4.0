using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TldPreRegistrationPhase : ITLDPreRegistrationPhase
  {
    private static readonly TldPreRegistrationPhase _nullPreRegPhase;

    static TldPreRegistrationPhase()
    {
      _nullPreRegPhase = FromNothing();
    }

    public static TldPreRegistrationPhase NULLPHASE
    {
      get { return _nullPreRegPhase; }
    }

    public static TldPreRegistrationPhase FromPhaseElement(XElement phaseElement)
    {
      return new TldPreRegistrationPhase(phaseElement);
    }

    public static TldPreRegistrationPhase FromNothing()
    {
      return null;
    }

    public string Type { get; private set; }
    public string SubType { get; private set; }
    public string Description { get; private set; }

    private TldPreRegistrationPhase(XElement phaseElement)
    {
      Type = phaseElement.Attribute("type").Value;
      SubType = phaseElement.Attribute("subtype").Value;
      Description = phaseElement.Attribute("description").Value;
    }
  }
}
