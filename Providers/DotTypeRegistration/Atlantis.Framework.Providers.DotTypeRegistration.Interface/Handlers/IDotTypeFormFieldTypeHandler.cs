using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface.Handlers
{
  public interface IDotTypeFormFieldTypeHandler
  {
    bool RenderField(FormFieldTypes fieldType, IProviderContainer providerContainer, out string htmlData);
  }
}
