using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotFm
{
  public class DotFm : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 40251, 40252, 40253, 40254, 40255, 40256, 40257, 40258, 40259, 40260 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 40251, 40252, 40253, 40254, 40255, 40256, 40257, 40258, 40259, 40260 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 40251, 40252, 40253, 40254, 40255, 40256, 40257, 40258, 40259, 40260 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 40251, 40252, 40253, 40254, 40255, 40256, 40257, 40258, 40259, 40260 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 40251, 40252, 40253, 40254, 40255, 40256, 40257, 40258, 40259, 40260 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 40251, 40252, 40253, 40254, 40255, 40256, 40257, 40258, 40259, 40260 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      return null;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 50251, 50252, 50253, 50254, 50255, 50256, 50257, 50258, 50259, 50260 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 50270, 50252, 50253, 50254, 50255, 50256, 50257, 50258, 50259, 50260 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 50270, 50252, 50253, 50254, 50255, 50256, 50257, 50258, 50259, 50260 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 50272, 50252, 50253, 50254, 50255, 50256, 50257, 50258, 50259, 50260 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 50274, 50252, 50253, 50254, 50255, 50256, 50257, 50258, 50259, 50260 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 50276, 50252, 50253, 50254, 50255, 50256, 50257, 50258, 50259, 50260 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "FM"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 1; }
    }

    public override int MaxRenewalLength
    {
      get { return 1; }
    }
  }
}
