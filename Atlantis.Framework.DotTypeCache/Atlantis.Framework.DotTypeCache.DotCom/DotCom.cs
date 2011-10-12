using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotCom
{
  public class DotCom : DotTypeStaticBase
  {
    public override string DotType
    {
      get { return "COM"; }
    }

    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      return new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { 
        new DotTypeTier(0, new int[]{101, 102, 103, 104, 105, 106, 107, 108, 109, 110}),
        new DotTypeTier(6, new int[]{966, 60041, 60051, 60371, 60061, 60381, 60391, 60401, 60411, 60071}),
        new DotTypeTier(21, new int[]{967, 60042, 60052, 60372, 60062, 60382, 60392, 60402, 60412, 60072}),
        new DotTypeTier(50, new int[]{162, 60043, 60053, 60373, 60063, 60383, 60393, 60403, 60413, 60073}),
        new DotTypeTier(101, new int[]{164, 60044, 60054, 60374, 60064, 60384, 60394, 60404, 60414, 60074}),
        new DotTypeTier(201, new int[]{166, 60045, 60055, 60375, 60065, 60385, 60395, 60405, 60415, 60075})
      });
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      return new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { 
        new DotTypeTier(0, new int[]{111, 113, 114, 115, 116, 117, 118, 119, 120}),
        new DotTypeTier(6, new int[]{970, 113, 114, 115, 116, 117, 118, 119, 120}),
        new DotTypeTier(21, new int[]{971, 113, 114, 115, 116, 117, 118, 119, 120}),
        new DotTypeTier(50, new int[]{171, 113, 114, 115, 116, 117, 118, 119, 120}),
        new DotTypeTier(101, new int[]{172, 113, 114, 115, 116, 117, 118, 119, 120}),
        new DotTypeTier(201, new int[]{173, 113, 114, 115, 116, 117, 118, 119, 120})
      });
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      return new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { 
        new DotTypeTier(0, new int[]{10101, 10102, 10103, 10104, 10105, 10106, 10107, 10108, 10109, 10110}),
        new DotTypeTier(6, new int[]{968, 70041, 70051, 70371, 70061, 70381, 70391, 70401, 70411, 70071}),
        new DotTypeTier(21, new int[]{969, 70042, 70052, 70372, 70062, 70382, 70392, 70402, 70412, 70072}),
        new DotTypeTier(50, new int[]{10162, 70043, 70053, 70373, 70063, 70383, 70393, 70403, 70413, 70073}),
        new DotTypeTier(101, new int[]{10164, 70044, 70054, 70374, 70064, 70384, 70394, 70404, 70414, 70074}),
        new DotTypeTier(201, new int[]{10166, 70045, 70055, 70375, 70065, 70385, 70395, 70405, 70415, 70075})
      });
    }

    protected override DotTypeProductIds InitializeExpiredAuctionRegProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 19010, 19138, 19139, 0, 19150, 0, 0, 0, 0, 19151 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.ExpiredAuctionReg, new DotTypeTier[] { DotTypeTier0 });
      return result;
    }

  }
}
