using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public interface IDotTypeFormsSchema
  {
    IList<IDotTypeFormsForm> FormCollection { get; set; }
  }
}
