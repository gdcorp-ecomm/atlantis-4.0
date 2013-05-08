using System.Collections.Generic;

namespace Atlantis.Framework.Providers.PlaceHolder.Interface
{
  public interface IPlaceHolderControl
  {
    IDictionary<string, string> Parameters { get; }

    bool ValidateParameters(out string errorLogMessage);
  }
}
