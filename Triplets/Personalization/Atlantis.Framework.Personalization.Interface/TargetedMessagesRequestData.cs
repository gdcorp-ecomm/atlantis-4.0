using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.Personalization.Interface
{
  public class TargetedMessagesRequestData : RequestData
  {
    private const int DEFAULT_TIMEOUT = 2;
    public string AppId { get; set; }
    public string InteractionPoint { get; set; }
    public string PrivateLabel { get; set; }
    public string AnonId { get; set; }
    public string Isc { get; set; }

    private bool IsAnonymousShopper { get { return string.IsNullOrEmpty(ShopperID); } }

    public TargetedMessagesRequestData(string shopperId, string privateLabel, string appId, string interactionPoint, string anonId, string isc)
    {
      AppId = appId;
      InteractionPoint = interactionPoint;
      ShopperID = shopperId;
      PrivateLabel = privateLabel;
      AnonId = anonId;
      Isc = isc;

      if (IsAnonymousShopper && string.IsNullOrEmpty(AnonId))
      {
        AnonId = Guid.NewGuid().ToString();
      }

      RequestTimeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT);
    }

    public string GetWebServicePath()
    {
      string path = string.Empty;
      if (IsAnonymousShopper)
      {
        path = String.Format("{0}/{1}?shopperData=anonID={2}|privateLabelID={3}", AppId, InteractionPoint, AnonId, PrivateLabel);
      }
      else
      {
        path = String.Format("{0}/{1}?shopperData=shopperID={2}|privateLabelID={3}", AppId, InteractionPoint, ShopperID, PrivateLabel);
      }
      return path;
    }

    public string GetPostData()
    {
      var tagData = new XElement("tagData",
                      new XElement("section",
                      new XAttribute("name", "Channel"),
                          new XElement("category",
                          new XAttribute("name", "session"),
                            new XElement("attribute",
                            new XAttribute("name", "isc"),
                                new XElement("value", Isc ?? string.Empty)))));

      return tagData.ToString();
    }

    public override string GetCacheMD5()
    {
      if (this.IsAnonymousShopper)
      {
        return BuildHashFromStrings(AnonId, PrivateLabel, AppId, InteractionPoint, Isc);
      }
      else
      {
        return BuildHashFromStrings(ShopperID, PrivateLabel, AppId, InteractionPoint, Isc);
      }
    }
    
  }
}

