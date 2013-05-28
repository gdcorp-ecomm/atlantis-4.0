using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public interface IDotTypeRegistrationProvider
  {
    bool GetDotTypeFormsSchema(int tldId, string placement, out IDotTypeFormsSchema dotTypeFormsSchema);

    bool GetDotTypeClaims(int tldId, out string claimsXml);

    bool ValidateDotTypeRegistration(string clientApplication, string serverName, int tldId, string phase,
                              Dictionary<string, string> fields, out bool hasErrors,
                              out Dictionary<string, string> validationErrors, out string token);
  }
}
