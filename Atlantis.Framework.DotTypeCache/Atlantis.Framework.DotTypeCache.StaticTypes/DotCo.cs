using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotCo : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 56401, 56402, 56403, 56404, 56405, 56406, 56407, 56408, 56409, 56410 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 56420, 56436, 56442, 56460, 56448, 56466, 56472, 56478, 56484, 56454 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 56421, 56437, 56443, 56461, 56449, 56467, 56473, 56479, 56485, 56455 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 56424, 56438, 56444, 56462, 56450, 56468, 56474, 56480, 56486, 56456 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 56426, 56439, 56445, 56463, 56451, 56469, 56475, 56481, 56487, 56457 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 56428, 56440, 56446, 56464, 56452, 56470, 56476, 56482, 56488, 56458 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 56411, 56412, 56413, 56414, 56415, 56416, 56417, 56418, 56419 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 56429, 56412, 56413, 56414, 56415, 56416, 56417, 56418, 56419 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 56430, 56412, 56413, 56414, 56415, 56416, 56417, 56418, 56419 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 56432, 56412, 56413, 56414, 56415, 56416, 56417, 56418, 56419 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 56433, 56412, 56413, 56414, 56415, 56416, 56417, 56418, 56419 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 56434, 56412, 56413, 56414, 56415, 56416, 56417, 56418, 56419 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 66401, 66402, 66403, 66404, 66405, 66406, 66407, 66408, 66409, 66410 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 66420, 66436, 66442, 66460, 66448, 66466, 66472, 66478, 66484, 66454 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 66421, 66437, 66443, 66461, 66449, 66467, 66473, 66479, 66485, 66455 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 66424, 66438, 66444, 66462, 66450, 66468, 66474, 66480, 66486, 66456 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 66426, 66439, 66445, 66463, 66451, 66469, 66475, 66481, 66487, 66457 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 66428, 66440, 66446, 66464, 66452, 66470, 66476, 66482, 66488, 66458 });
      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializePreRegistrationProductIds()
    {
      return new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { 
        new StaticDotTypeTier(0, new int[]{56392, 56395, 56396, 56397, 56398}),
        new StaticDotTypeTier(6, new int[]{56800, 56813, 56819, 56397, 56825}),
        new StaticDotTypeTier(21, new int[]{56801, 56814, 56820, 56397, 56826}),
        new StaticDotTypeTier(50, new int[]{56803, 56815, 56821, 56397, 56827}),
        new StaticDotTypeTier(101, new int[]{56804, 56816, 56822, 56397, 56828}),
        new StaticDotTypeTier(201, new int[]{56805, 56817, 56823, 56397, 56829})
      });
    }

    public override string DotType
    {
      get { return "CO"; }
    }

    public override int MinRegistrationLength
    {
      get { return 1; }
    }

    public override int MaxRegistrationLength
    {
      get { return 5; }
    }

    public override int MinRenewalLength
    {
      get { return 1; }
    }

    public override int MaxRenewalLength
    {
      get { return 5; }
    }

    public override int MaxTransferLength
    {
      get { return 4; }
    }

    protected override int MaxRenewalMonthsOut
    {
      get { return 60; }
    }
  }
}
