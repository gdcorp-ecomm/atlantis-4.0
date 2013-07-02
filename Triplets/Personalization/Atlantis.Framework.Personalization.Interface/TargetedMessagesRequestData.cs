using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Personalization.Interface
{
  public class TargetedMessagesRequestData : RequestData
  {
    private const int DEFAULT_TIMEOUT = 10;
    public string AppId { get; set; }
    public string InteractionPoint { get; set; }

    public TargetedMessagesRequestData(string shopperId, string appId, string interactionPoint)
    {
      AppId = appId;
      InteractionPoint = interactionPoint;
      ShopperID = shopperId;

      RequestTimeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT);
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(ShopperID, AppId, InteractionPoint);
    }

  }
}

