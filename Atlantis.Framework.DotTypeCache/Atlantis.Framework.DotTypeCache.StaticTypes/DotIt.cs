using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotIt : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 56101, 56102, 56103, 56104, 56105, 56106, 56107, 56108, 56109, 56110 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 56120, 56136, 56142, 56160, 56148, 56166, 56172, 56178, 56184, 56154 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 56121, 56137, 56143, 56161, 56149, 56167, 56173, 56179, 56185, 56155 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 56124, 56138, 56144, 56162, 56150, 56168, 56174, 56180, 56186, 56156 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 56126, 56139, 56145, 56163, 56151, 56169, 56175, 56181, 56187, 56157 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 56128, 56140, 56146, 56164, 56152, 56170, 56176, 56182, 56188, 56158 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 56111, 56112, 56113, 56114, 56115, 56116, 56117, 56118, 56119 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 56129, 56112, 56113, 56114, 56115, 56116, 56117, 56118, 56119 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 56130, 56112, 56113, 56114, 56115, 56116, 56117, 56118, 56119 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 56132, 56112, 56113, 56114, 56115, 56116, 56117, 56118, 56119 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 56133, 56112, 56113, 56114, 56115, 56116, 56117, 56118, 56119 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 56134, 56112, 56113, 56114, 56115, 56116, 56117, 56118, 56119 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 66101, 66102, 66103, 66104, 66105, 66106, 66107, 66108, 66109, 66110 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 66120, 66136, 66142, 66160, 66148, 66166, 66172, 66178, 66184, 66154 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 66121, 66137, 66143, 66161, 66149, 66167, 66173, 66179, 66185, 66155 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 66124, 66138, 66144, 66162, 66150, 66168, 66174, 66180, 66186, 66156 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 66126, 66139, 66145, 66163, 66151, 66169, 66175, 66181, 66187, 66157 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 66128, 66140, 66146, 66164, 66152, 66170, 66176, 66182, 66188, 66158 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "IT"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 1; }
    }

    public override int MaxTransferLength
    {
      get { return 1; }
    }

    public override int MaxRenewalLength
    {
      get { return 1; }
    }

    protected override int MaxRenewalMonthsOut
    {
      get { return 24; }
    }
  }
}
