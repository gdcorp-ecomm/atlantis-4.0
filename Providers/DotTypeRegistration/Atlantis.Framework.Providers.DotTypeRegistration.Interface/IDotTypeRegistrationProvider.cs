using System.Collections.Generic;
using Atlantis.Framework.DotTypeValidation.Interface;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public interface IDotTypeRegistrationProvider
  {
    bool GetDotTypeForms(IDotTypeFormLookup dotTypeFormsLookup, out string dotTypeFormsHtml);

    bool GetDotTypeFormSchemas(IDotTypeFormLookup dotTypeFormsLookup, string[] domains, out IDictionary<string, IList<IList<IFormField>>> formFieldsByDomain);

    bool ValidateData(string clientApplication, string serverName, string tld, string phase, string category, Dictionary<string, string> fields,
                               out DotTypeValidationResponseData validationResponseData);
  }
}
