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
      }

      public static int DotTypeFormsXmlRequest { get; set; }
      public static int DotTypeValidationRequest { get; set; }
      public static int DotTypeFormsHtmlRequest { get; set; }
      public static int DotTypeClaimsRequest { get; set; }
    }
}
