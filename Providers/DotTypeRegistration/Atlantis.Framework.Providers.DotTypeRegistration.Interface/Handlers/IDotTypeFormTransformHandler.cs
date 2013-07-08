using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface.Handlers
{
  public interface IDotTypeFormTransformHandler
  {
    bool TransformFormToHtml(IDotTypeFormsSchema formSchema, string[] domains, IProviderContainer providerContainer, out Dictionary<string, string> formSchemasHtml);
  }
}
