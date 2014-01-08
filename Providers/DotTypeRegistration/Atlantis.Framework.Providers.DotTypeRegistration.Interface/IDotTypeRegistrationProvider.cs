using System.Collections.Generic;
using Atlantis.Framework.DotTypeValidation.Interface;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public interface IDotTypeRegistrationProvider
  {
    bool GetDotTypeForms(IDotTypeFormLookup dotTypeFormsLookup, out string dotTypeFormsHtml);

    bool GetDotTypeFormSchemas(IDotTypeFormSchemaLookup dotTypeFormsLookup, string[] domains, out IDotTypeFormFieldsByDomain dotTypeFormFieldsByDomain);

    bool ValidateData(string clientApplication, string tld, string phase, Dictionary<string, string> fields,
                               out DotTypeValidationResponseData validationResponseData);
  }
}
