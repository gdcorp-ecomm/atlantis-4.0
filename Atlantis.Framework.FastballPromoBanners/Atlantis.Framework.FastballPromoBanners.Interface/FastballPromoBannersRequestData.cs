using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballPromoBanners.Interface
{
  public class FastballPromoBannersRequestData : RequestData
  {
    private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(6);

    public int ApplicationId { get; private set; }

    public string Placement { get; private set; }

    public string Isc { get; private set; }

    public int PrivateLabelId { get; private set; }

    /// <summary>
    /// Optional.  C3 Rep Id.
    /// </summary>
    public string RepId { get; set; }
    
    private string _candidateRequest;
    public string CandidateRequestXml
    {
      get
      {
        if (_candidateRequest == null)
        {
          XElement candidateData = new XElement("CandidateData");

          if(!string.IsNullOrEmpty(ShopperID))
          {
            candidateData.SetAttributeValue("ShopperID", ShopperID);
            candidateData.SetAttributeValue("PrivateLabelID", PrivateLabelId);
          }
          candidateData.SetAttributeValue("ISC", Isc);

          _candidateRequest = candidateData.ToString(SaveOptions.DisableFormatting);
        }
        return _candidateRequest;
      }
    }

    private string _channelRequest;
    public string ChannelRequestXml
    {
      get
      {
        if (_channelRequest == null)
        {
          XElement request = new XElement("RequestXml");

          XElement clientData = new XElement("ClientData");
          clientData.SetAttributeValue("AppID", ApplicationId);
          clientData.SetAttributeValue("Placement", Placement);

          XElement contextData = new XElement("ContextData");
          contextData.SetAttributeValue("VisitUID", Pathway);
          contextData.SetAttributeValue("PageCount", PageCount);
          if (!string.IsNullOrEmpty(RepId))
          {
            contextData.SetAttributeValue("RepID", RepId);
          }

          request.Add(clientData);
          request.Add(contextData);

          _channelRequest = request.ToString(SaveOptions.DisableFormatting);
        }
        return _channelRequest;
      }
    }

    public FastballPromoBannersRequestData(int applicationId,
                                           string placement,
                                           string isc,
                                           string shopperId,
                                           int privateLabelId,
                                           string sourceUrl,
                                           string orderId,
                                           string pathway,
                                           int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ApplicationId = applicationId;
      Placement = placement;
      Isc = isc;
      PrivateLabelId = privateLabelId;
      RequestTimeout = _defaultTimeout;
    }

    public override string GetCacheMD5()
    {
      MD5 md5 = MD5.Create();
      byte[] inputBytes = Encoding.ASCII.GetBytes(string.Format("{0}:{1}:{2}:{3}", ShopperID, Isc, ApplicationId, Placement));
      byte[] hashBytes = md5.ComputeHash(inputBytes);

      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < hashBytes.Length; i++)
      {
        sb.Append(hashBytes[i].ToString("X2"));
      }
      return sb.ToString();
    }
  }
}
