using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotOrgDotUk
{
  public class DotOrgDotUk : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 13501, 13502, 13503, 13504, 13505, 13506, 13507, 13508, 13509, 13510 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 13501, 13502, 13503, 13504, 13505, 13506, 13507, 13508, 13509, 13510 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 13501, 13502, 13503, 13504, 13505, 13506, 13507, 13508, 13509, 13510 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 13501, 13502, 13503, 13504, 13505, 13506, 13507, 13508, 13509, 13510 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 13501, 13502, 13503, 13504, 13505, 13506, 13507, 13508, 13509, 13510 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 13501, 13502, 13503, 13504, 13505, 13506, 13507, 13508, 13509, 13510 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 13511, 13522, 13523, 13524, 13525, 13526, 13527, 13528, 13529 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 13511, 13522, 13523, 13524, 13525, 13526, 13527, 13528, 13529 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 13511, 13522, 13523, 13524, 13525, 13526, 13527, 13528, 13529 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 13541, 13522, 13523, 13524, 13525, 13526, 13527, 13528, 13529 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 13542, 13522, 13523, 13524, 13525, 13526, 13527, 13528, 13529 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 13543, 13522, 13523, 13524, 13525, 13526, 13527, 13528, 13529 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 13512, 13513, 13514, 13515, 13516, 13517, 13518, 13519, 13520, 13521 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 13512, 13513, 13514, 13515, 13516, 13517, 13518, 13519, 13520, 13521 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 13512, 13513, 13514, 13515, 13516, 13517, 13518, 13519, 13520, 13521 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 23532, 13513, 13514, 13515, 13516, 13517, 13518, 13519, 13520, 13521 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 23534, 13513, 13514, 13515, 13516, 13517, 13518, 13519, 13520, 13521 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 23536, 13513, 13514, 13515, 13516, 13517, 13518, 13519, 13520, 13521 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ORG.UK"; }
    }

    public override int MinRegistrationLength
    {
      get { return 2; }
    }

    public override int MaxRegistrationLength
    {
      get { return 2; }
    }

    public override int MinRenewalLength
    {
      get { return 2; }
    }

    public override int MaxRenewalLength
    {
      get { return 2; }
    }
  }
}
