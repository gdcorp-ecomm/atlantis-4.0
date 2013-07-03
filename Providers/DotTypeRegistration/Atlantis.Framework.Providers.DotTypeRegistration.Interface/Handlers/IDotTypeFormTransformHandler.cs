using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface.Handlers
{
  public interface IDotTypeFormTransformHandler
  {
    bool TransformFormToHtml(IDotTypeFormsSchema formsSchema, string[] domains, IProviderContainer providerContainer, out string formSchemaHtml);
  }
}
