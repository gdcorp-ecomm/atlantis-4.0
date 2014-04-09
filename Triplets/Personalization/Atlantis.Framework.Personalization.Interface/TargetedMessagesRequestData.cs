using System.Collections.Generic;
using System.Linq;
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
    public Dictionary<string,string> ChannelSessionData { get; set; }

    private bool IsAnonymousShopper { get { return string.IsNullOrEmpty(ShopperID); } }

    public TargetedMessagesRequestData(string shopperId, string privateLabel, string appId, string interactionPoint, string anonId, Dictionary<string,string> channelSessionData) 
    {
      AppId = appId;
      InteractionPoint = interactionPoint;
      ShopperID = shopperId;
      PrivateLabel = privateLabel;
      AnonId = anonId;
      ChannelSessionData = channelSessionData;

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
      if ((ChannelSessionData != null) && (ChannelSessionData.Any()))
      {
        XElement tagDataXml = new XElement("tagData");

        XElement channelDataXml;
        tagDataXml.Add(channelDataXml = new XElement("section",
          new XAttribute("name", "Channel")));

        XElement channelSessionDataXml;
        channelDataXml.Add(channelSessionDataXml = new XElement("category",
          new XAttribute("name", "session")));

        foreach (var item in ChannelSessionData)
        {
          channelSessionDataXml.Add(new XElement("attribute",
            new XAttribute("name", item.Key),
            new XElement("value", item.Value ?? string.Empty)));
        }

        return tagDataXml.ToString();
      }

      return String.Empty;
    }

    public override string GetCacheMD5()
    {
      string channelSessionData = ((ChannelSessionData != null) && (ChannelSessionData.Any())) ?
        String.Join(",", from item in ChannelSessionData
                         select String.Join("=", item.Key, item.Value ?? String.Empty)) : String.Empty;

      if (this.IsAnonymousShopper)
      {
        return BuildHashFromStrings(AnonId, PrivateLabel, AppId, InteractionPoint, channelSessionData);
      }
      else
      {
        return BuildHashFromStrings(ShopperID, PrivateLabel, AppId, InteractionPoint, channelSessionData);
      }
    }

  }
}

