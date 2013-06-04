using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.PlaceHolder.Interface
{
  public interface IPlaceHolderProvider
  {
    IPlaceHolderData GetPlaceHolderData(string type);

    string GetPlaceHolderMarkup(string type, string location, IDictionary<string, string> parameters);

    string ReplacePlaceHolders(string content);

    [Obsolete("Only used in the legacy CDS widget context, use ReplacePlaceHolder(string content) instead.")]
    string ReplacePlaceHolders(string content, IPlaceHolderEncoding placeHolderEncoding);
  }
}
