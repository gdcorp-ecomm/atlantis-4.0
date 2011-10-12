using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotMe
{
  public class DotMe : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 9140, 9141, 9142, 9143, 9144, 9145, 9146, 9147, 9148, 9149 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 9159, 9175, 9181, 9199, 9187, 9205, 9211, 9217, 9223, 9193 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 9160, 9176, 9182, 9200, 9188, 9206, 9212, 9218, 9224, 9194 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 9163, 9177, 9183, 9201, 9189, 9207, 9213, 9219, 9225, 9195 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 9165, 9178, 9184, 9202, 9190, 9208, 9214, 9220, 9226, 9196 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 9167, 9179, 9185, 9203, 9191, 9209, 9215, 9221, 9227, 9197 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 9150, 9151, 9152, 9153, 9154, 9155, 9156, 9157, 9158 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 9168, 9151, 9152, 9153, 9154, 9155, 9156, 9157, 9158 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 9169, 9151, 9152, 9153, 9154, 9155, 9156, 9157, 9158 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 9171, 9151, 9152, 9153, 9154, 9155, 9156, 9157, 9158 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 9172, 9151, 9152, 9153, 9154, 9155, 9156, 9157, 9158 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 9173, 9151, 9152, 9153, 9154, 9155, 9156, 9157, 9158 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 19140, 19141, 19142, 19143, 19144, 19145, 19146, 19147, 19148, 19149 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 19159, 19175, 19181, 19199, 19187, 19205, 19211, 19217, 19223, 19193 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 19160, 19176, 19182, 19200, 19188, 19206, 19212, 19218, 19224, 19194 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 19163, 19177, 19183, 19201, 19189, 19207, 19213, 19219, 19225, 19195 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 19165, 19178, 19184, 19202, 19190, 19208, 19214, 19220, 19226, 19196 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 19167, 19179, 19185, 19203, 19191, 19209, 19215, 19221, 19227, 19197 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ME"; }
    }
  }
}
