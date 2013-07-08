using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Personalization.Interface
{
  public class TargetedMessagesRequestData : RequestData
  {
    private const int DEFAULT_TIMEOUT = 2;
    public string AppId { get; set; }
    public string InteractionPoint { get; set; }
    public string PrivateLabel { get; set; }

    public TargetedMessagesRequestData(string shopperId, string privateLabel, string appId, string interactionPoint)
    {
      AppId = appId;
      InteractionPoint = interactionPoint;
      ShopperID = shopperId;
      PrivateLabel = privateLabel;

      RequestTimeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT);
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(ShopperID, PrivateLabel, AppId, InteractionPoint);
    }

  }
}

