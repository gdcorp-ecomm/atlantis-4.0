using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.Personalization.Interface
{
  public class TargetedMessagesRequestData : RequestData, IEquatable<TargetedMessagesRequestData>, IEqualityComparer<TargetedMessagesRequestData>
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
    
    public bool Equals(TargetedMessagesRequestData other)
    {
      bool equal =   (this.PrivateLabel == other.PrivateLabel) 
                  && (this.AppId == other.AppId) 
                  && (this.InteractionPoint == other.InteractionPoint) 
                  && (this.Isc == other.Isc);

      if (this.IsAnonymousShopper && other.IsAnonymousShopper)
      {
        equal = (this.AnonId == other.AnonId) && equal;
      }
      else
      {
        equal = (this.ShopperID == other.ShopperID) && equal;
      }

      return equal;
    }

    public bool Equals(TargetedMessagesRequestData x, TargetedMessagesRequestData y)
    {
      if (Object.ReferenceEquals(x, y)) return true;

      return x.Equals(y);
    }

    public int GetHashCode(TargetedMessagesRequestData obj)
    {
      return obj.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj is TargetedMessagesRequestData)
        return this.Equals(obj as TargetedMessagesRequestData);
      else
        return false;
    }

    public override int GetHashCode()
    {
      if (this.IsAnonymousShopper)
      {
        return AnonId.GetHashCode() ^ PrivateLabel.GetHashCode() ^  AppId.GetHashCode() ^  InteractionPoint.GetHashCode() ^  Isc.GetHashCode();
      }
      else
      {
        return ShopperID.GetHashCode() ^ PrivateLabel.GetHashCode() ^ AppId.GetHashCode() ^ InteractionPoint.GetHashCode() ^ Isc.GetHashCode();
      }
    }
  }
}

