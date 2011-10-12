using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotOrgDotIn
{
  public class DotOrgDotIn : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 41500, 41501, 41502, 41503, 41504, 41505, 41506, 41507, 41508, 41509 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 41530, 41539, 41545, 41563, 41551, 41569, 41575, 41581, 41587, 41557 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 41531, 41540, 41546, 41564, 41552, 41570, 41576, 41582, 41588, 41558 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 41521, 41541, 41547, 41565, 41553, 41571, 41577, 41583, 41589, 41559 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 41523, 41542, 41548, 41566, 41554, 41572, 41578, 41584, 41590, 41560 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 41525, 41543, 41549, 41567, 41555, 41573, 41579, 41585, 41591, 41561 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 41510, 41511, 41512, 41513, 41514, 41515, 41516, 41517, 41518 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 41532, 41511, 41512, 41513, 41514, 41515, 41516, 41517, 41518 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 41533, 41511, 41512, 41513, 41514, 41515, 41516, 41517, 41518 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 41527, 41511, 41512, 41513, 41514, 41515, 41516, 41517, 41518 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 41528, 41511, 41512, 41513, 41514, 41515, 41516, 41517, 41518 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 41529, 41511, 41512, 41513, 41514, 41515, 41516, 41517, 41518 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 51500, 51501, 51502, 51503, 51504, 51505, 51506, 51507, 51508, 51509 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 51530, 51539, 51545, 51563, 51551, 51569, 51575, 51581, 51587, 51557 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 51531, 51540, 51546, 51564, 51552, 51570, 51576, 51582, 51588, 51558 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 51521, 51541, 51547, 51565, 51553, 51571, 51577, 51583, 51589, 51559 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 51523, 51542, 51548, 51566, 51554, 51572, 51578, 51584, 51590, 51560 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 51525, 51543, 51549, 51567, 51555, 51573, 51579, 51585, 51591, 51561 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ORG.IN"; }
    }
  }
}
