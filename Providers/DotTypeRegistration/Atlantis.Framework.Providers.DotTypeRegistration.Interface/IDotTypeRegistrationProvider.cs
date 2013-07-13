using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public interface IDotTypeRegistrationProvider
  {
    bool GetDotTypeForms(int tldId, string placement, string phase, string language, out string dotTypeFormsHtml);

    bool GetDotTypeFormSchemas(int tldId, string placement, string phase, string language, string[] domains, out IDictionary<string, IList<IFormField>> formFieldsByDomain);

    bool ValidateData(string clientApplication, string serverName, int tldId, string phase, string category, Dictionary<string, string> fields, 
                               out bool hasErrors, out Dictionary<string, string> validationErrors, out string token);
  }
}
