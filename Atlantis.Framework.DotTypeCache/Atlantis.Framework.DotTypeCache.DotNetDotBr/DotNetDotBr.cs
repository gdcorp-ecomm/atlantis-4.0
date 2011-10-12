using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotNetDotBr
{
  public class DotNetDotBr : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 57101, 57102, 57103, 57104, 57105, 57106, 57107, 57108, 57109, 57110 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 57120, 57136, 57142, 57160, 57148, 57166, 57172, 57178, 57184, 57154 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 57121, 57137, 57143, 57161, 57149, 57167, 57173, 57179, 57185, 57155 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 57124, 57138, 57144, 57162, 57150, 57168, 57174, 57180, 57186, 57156 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 57126, 57139, 57145, 57163, 57151, 57169, 57175, 57181, 57187, 57157 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 57128, 57140, 57146, 57164, 57152, 57170, 57176, 57182, 57188, 57158 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 57111, 57112, 57113, 57114, 57115, 57116, 57117, 57118, 57119 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 57129 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 57130 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 57132 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 57133 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 57134 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0 });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 67101, 67102, 67103, 67104, 67105, 67106, 67107, 67108, 67109, 67110 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 67120, 67136, 67142, 67160, 67148, 67166, 67172, 67178, 67184, 67154 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 67121, 67137, 67143, 67161, 67149, 67167, 67173, 67179, 67185, 67155 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 67124, 67138, 67144, 67162, 67150, 67168, 67174, 67180, 67186, 67156 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 67126, 67139, 67145, 67163, 67151, 67169, 67175, 67181, 67187, 67157 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 67128, 67140, 67146, 67164, 67152, 67170, 67176, 67182, 67188, 67158 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override int MaxRegistrationLength
    {
      get { return 1; }
    }

    public override int MaxRenewalLength
    {
      get { return 1; }
    }

    public override string DotType
    {
      get { return "NET.BR"; }
    }
  }
}
