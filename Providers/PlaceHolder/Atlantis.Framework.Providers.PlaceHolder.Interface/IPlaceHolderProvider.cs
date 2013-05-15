using System;

namespace Atlantis.Framework.Providers.PlaceHolder.Interface
{
  public interface IPlaceHolderProvider
  {
    IPlaceHolderData GetPlaceHolderData(string type);

    string ReplacePlaceHolders(string content);

    [Obsolete("Only used in the legacy CDS widget context, use ReplacePlaceHolder(string content) instead.")]
    string ReplacePlaceHolders(string content, IPlaceHolderEncoding placeHolderEncoding);
  }
}
