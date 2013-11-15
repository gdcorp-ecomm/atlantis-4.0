using Atlantis.Framework.TLDDataCache.Interface;

namespace Atlantis.Framework.Providers.DotTypeRegistration
{
    public static class DotTypeRegistrationEngineRequests
    {
      static DotTypeRegistrationEngineRequests()
      {
        DotTypeFormsXmlRequest = 689;
        DotTypeValidationRequest = 695;
        DotTypeFormsHtmlRequest = 709;
        DotTypeClaimsRequest = 710;
        ValidDotTypesRequest = 667;
      }

      public static int DotTypeFormsXmlRequest { get; set; }
      public static int DotTypeValidationRequest { get; set; }
      public static int DotTypeFormsHtmlRequest { get; set; }
      public static int DotTypeClaimsRequest { get; set; }
      public static int ValidDotTypesRequest { get; set; }
    }
}
