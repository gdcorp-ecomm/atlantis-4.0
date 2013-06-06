using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  public class ShopperWatchListJsonRequestData : RequestData
  {
    public string LanguageCode { get; set; }

    public ShopperWatchListJsonRequestData(string shopperId, string languageCode)
    {
      ShopperID = shopperId;
      LanguageCode = languageCode;

      RequestTimeout = TimeSpan.FromSeconds(10);
    }
  }
}
