using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotFirmDotIn
{
  public class DotFirmDotIn : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 41100, 41101, 41102, 41103, 41104, 41105, 41106, 41107, 41108, 41109 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 41130, 41139, 41145, 41163, 41151, 41169, 41175, 41181, 41187, 41157 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 41131, 41140, 41146, 41164, 41152, 41170, 41176, 41182, 41188, 41158 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 41121, 41141, 41147, 41165, 41153, 41171, 41177, 41183, 41189, 41159 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 41123, 41142, 41148, 41166, 41154, 41172, 41178, 41184, 41190, 41160 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 41125, 41143, 41149, 41167, 41155, 41173, 41179, 41185, 41191, 41161 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 41110, 41111, 41112, 41113, 41114, 41115, 41116, 41117, 41118 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 41132, 41111, 41112, 41113, 41114, 41115, 41116, 41117, 41118 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 41133, 41111, 41112, 41113, 41114, 41115, 41116, 41117, 41118 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 41127, 41111, 41112, 41113, 41114, 41115, 41116, 41117, 41118 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 41128, 41111, 41112, 41113, 41114, 41115, 41116, 41117, 41118 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 41129, 41111, 41112, 41113, 41114, 41115, 41116, 41117, 41118 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 51100, 51101, 51102, 51103, 51104, 51105, 51106, 51107, 51108, 51109 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 51130, 51139, 51145, 51163, 51151, 51169, 51175, 51181, 51187, 51157 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 51131, 51140, 51146, 51164, 51152, 51170, 51176, 51182, 51188, 51158 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 51121, 51141, 51147, 51165, 51153, 51171, 51177, 51183, 51189, 51159 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 51123, 51142, 51148, 51166, 51154, 51172, 51178, 51184, 51190, 51160 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 51125, 51143, 51149, 51167, 51155, 51173, 51179, 51185, 51191, 51161 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "FIRM.IN"; }
    }
  }
}
