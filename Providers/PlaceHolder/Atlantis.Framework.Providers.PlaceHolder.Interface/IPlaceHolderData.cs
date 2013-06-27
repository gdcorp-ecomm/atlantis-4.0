using System.Collections.Generic;

namespace Atlantis.Framework.Providers.PlaceHolder.Interface
{
    public interface IPlaceHolderData
    {
        string App { get; set; }

        string Location { get; }

        IDictionary<string, IPlaceHolderParameter> Parameters { get; }
    }
}
