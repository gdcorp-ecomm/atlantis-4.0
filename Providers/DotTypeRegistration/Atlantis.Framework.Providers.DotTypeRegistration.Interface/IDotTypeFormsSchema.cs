using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public interface IDotTypeFormsSchema
  {
    int TldId { get; set; }
    string Placement { get; set; }
    IList<IDotTypeFormsForm> FormCollection { get; set; }
  }
}
