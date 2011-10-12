using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotComDotBr
{
  public class DotComDotBr : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 57201, 57202, 57203, 57204, 57205, 57206, 57207, 57208, 57209, 57210 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 57220, 57236, 57242, 57260, 57248, 57266, 57272, 57278, 57284, 57254 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 57221, 57237, 57243, 57261, 57249, 57267, 57273, 57279, 57285, 57255 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 57224, 57238, 57244, 57262, 57250, 57268, 57274, 57280, 57286, 57256 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 57226, 57239, 57245, 57263, 57251, 57269, 57275, 57281, 57287, 57257 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 57228, 57240, 57246, 57264, 57252, 57270, 57276, 57282, 57288, 57258 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 57211, 57212, 57213, 57214, 57215, 57216, 57217, 57218, 57219 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 57229 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 57230 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 57232 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 57233 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 57234 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0 });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 67201, 67202, 67203, 67204, 67205, 67206, 67207, 67208, 67209, 67210 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 67220, 67236, 67242, 67260, 67248, 67266, 67272, 67278, 67284, 67254 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 67221, 67237, 67243, 67261, 67249, 67267, 67273, 67279, 67285, 67255 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 67224, 67238, 67244, 67262, 67250, 67268, 67274, 67280, 67286, 67256 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 67226, 67239, 67245, 67263, 67251, 67269, 67275, 67281, 67287, 67257 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 67228, 67240, 67246, 67264, 67252, 67270, 67276, 67282, 67288, 67258 });

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
      get { return "COM.BR"; }
    }

  }
}
