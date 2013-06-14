using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public interface IDotTypeRegistrationProvider
  {
    bool GetDotTypeFormsSchema(int tldId, string placement, string phase, string language, out IDotTypeFormsSchema dotTypeFormsSchema);

    bool GetClaimsSchema(string[] domains, out IDotTypeClaimsSchema claimsResponseData);

    bool ValidateTrademarkData(string clientApplication, string serverName, int tldId, string phase, string category, Dictionary<string, string> fields, 
                                out bool hasErrors, out Dictionary<string, string> validationErrors, out string token);

    bool ValidateClaimData(string clientApplication, string serverName, int tldId, string phase, string category, string noticeXml, 
                            out bool hasErrors, out Dictionary<string, string> validationErrors, out string token);
  }
}
