using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotCa
{
  public class DotCa : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 9405, 9406, 9407, 9408, 9409, 9410, 9411, 9412, 9413, 9414 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 9435, 9444, 9450, 9468, 9456, 9474, 9480, 9486, 9492, 9462 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 9436, 9445, 9451, 9469, 9457, 9475, 9481, 9487, 9493, 9463 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 9426, 9446, 9452, 9470, 9458, 9476, 9482, 9488, 9494, 9464 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 9428, 9447, 9453, 9471, 9459, 9477, 9483, 9489, 9495, 9465 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 9430, 9448, 9454, 9472, 9460, 9478, 9484, 9490, 9496, 9466 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 9415, 9416, 9417, 9418, 9419, 9420, 9421, 9422, 9423 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 9437, 9416, 9417, 9418, 9419, 9420, 9421, 9422, 9423 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 9438, 9416, 9417, 9418, 9419, 9420, 9421, 9422, 9423 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 9432, 9416, 9417, 9418, 9419, 9420, 9421, 9422, 9423 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 9433, 9416, 9417, 9418, 9419, 9420, 9421, 9422, 9423 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 9434, 9416, 9417, 9418, 9419, 9420, 9421, 9422, 9423 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 19405, 19406, 19407, 19408, 19409, 19410, 19411, 19412, 19413, 19414 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 19435, 19444, 19450, 19468, 19456, 19474, 19480, 19486, 19492, 19462 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 19436, 19445, 19451, 19469, 19457, 19475, 19481, 19487, 19493, 19463 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 19426, 19446, 19452, 19470, 19458, 19476, 19482, 19488, 19494, 19464 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 19428, 19447, 19453, 19471, 19459, 19477, 19483, 19489, 19495, 19465 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 19430, 19448, 19454, 19472, 19460, 19478, 19484, 19490, 19496, 19466 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "CA"; }
    }
  }
}
