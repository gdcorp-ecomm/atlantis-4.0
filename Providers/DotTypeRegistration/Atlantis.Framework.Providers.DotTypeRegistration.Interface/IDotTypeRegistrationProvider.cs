using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public interface IDotTypeRegistrationProvider
  {
    bool GetDotTypeForms(string formType, int tldId, string placement, string phase, out string dotTypeFormsHtml);

    bool GetDotTypeFormSchemas(string formType, int tldId, string placement, string phase, string[] domains, out IDictionary<string, IList<IList<IFormField>>> formFieldsByDomain);

    bool ValidateData(string clientApplication, string serverName, int tldId, string phase, string category, Dictionary<string, string> fields, 
                               out bool hasErrors, out Dictionary<string, string> validationErrors, out string token);
  }
}
