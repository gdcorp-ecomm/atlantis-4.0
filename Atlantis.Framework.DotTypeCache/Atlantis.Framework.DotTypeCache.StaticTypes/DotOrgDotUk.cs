using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotOrgDotUk : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 13501, 13502, 13503, 13504, 13505, 13506, 13507, 13508, 13509, 13510 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 13501, 13502, 13503, 13504, 13505, 13506, 13507, 13508, 13509, 13510 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 13501, 13502, 13503, 13504, 13505, 13506, 13507, 13508, 13509, 13510 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 13501, 13502, 13503, 13504, 13505, 13506, 13507, 13508, 13509, 13510 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 13501, 13502, 13503, 13504, 13505, 13506, 13507, 13508, 13509, 13510 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 13501, 13502, 13503, 13504, 13505, 13506, 13507, 13508, 13509, 13510 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 13511, 13522, 13523, 13524, 13525, 13526, 13527, 13528, 13529 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 13511, 13522, 13523, 13524, 13525, 13526, 13527, 13528, 13529 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 13511, 13522, 13523, 13524, 13525, 13526, 13527, 13528, 13529 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 13541, 13522, 13523, 13524, 13525, 13526, 13527, 13528, 13529 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 13542, 13522, 13523, 13524, 13525, 13526, 13527, 13528, 13529 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 13543, 13522, 13523, 13524, 13525, 13526, 13527, 13528, 13529 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 13512, 13513, 13514, 13515, 13516, 13517, 13518, 13519, 13520, 13521 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 13512, 13513, 13514, 13515, 13516, 13517, 13518, 13519, 13520, 13521 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 13512, 13513, 13514, 13515, 13516, 13517, 13518, 13519, 13520, 13521 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 23532, 13513, 13514, 13515, 13516, 13517, 13518, 13519, 13520, 13521 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 23534, 13513, 13514, 13515, 13516, 13517, 13518, 13519, 13520, 13521 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 23536, 13513, 13514, 13515, 13516, 13517, 13518, 13519, 13520, 13521 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ORG.UK"; }
    }

    public override int MaxTransferLength
    {
      get { return 1; }
    }
  }
}
