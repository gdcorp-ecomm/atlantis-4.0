using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public interface IDotTypeRegistrationProvider
  {
    bool GetDotTypeForms(int tldId, string placement, string phase, string language, out string dotTypeFormsHtml);

    bool GetDotTypeFormsSchema(int tldId, string placement, string phase, string language, string[] domains, ViewTypes viewType,
                               out string dotTypeFormsHtml);

    bool GetClaimsSchema(string[] domains, out IDotTypeClaimsSchema claimsResponseData);

    bool ValidateData(string clientApplication, string serverName, int tldId, string phase, string category, Dictionary<string, string> fields, 
                               out bool hasErrors, out Dictionary<string, string> validationErrors, out string token);
  }
}
