using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public interface IDotTypeFormFieldsByDomain
  {
    IDictionary<string, IList<IList<IFormField>>> FormFieldsByDomain { get; set; }
    IFormItems FormItems { get; }

    string ToJson { get; }
  }
}
