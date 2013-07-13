using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface.Handlers
{
  public interface IDotTypeFormFieldTypeHandler
  {
    bool RenderDotTypeFormField(DotTypeFormFieldTypes fieldType, IProviderContainer providerContainer, out IList<IFormField> formFields);
  }
}
