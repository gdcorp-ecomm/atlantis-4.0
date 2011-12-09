using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.FastballProduct.Interface
{
  public class FastballProductRequestData : RequestData
  {
    public string Placement { get; private set; }
    static TimeSpan _twoSeconds = TimeSpan.FromSeconds(2.0);
    public int ApplicationId { get; private set; }
    public string SpoofOfferId { get; set; }
    public int PrivateLabelId { get; private set; }

    public FastballProductRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string placement, int applicationId, int privateLabelId)
      :base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Placement = placement;
      RequestTimeout = _twoSeconds;
      ApplicationId = applicationId;
      SpoofOfferId = string.Empty;
      PrivateLabelId = privateLabelId;
    }

    /// <summary>
    /// This is a very unique caching rule.  To avoid a situation where someone is somehow getting new guids on every request
    /// we will only key the data by the placement.  So if there is some kind of visit guid generation issue, only 1
    /// placement will be available for the users session.
    /// </summary>
    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(Placement + ":" + ApplicationId.ToString() + ":" + PrivateLabelId.ToString() + ":" + SpoofOfferId);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }

    public string GetChannelRequestXml()
    {
      XElement clientData = new XElement("ClientData", new XAttribute("AppID", ApplicationId.ToString()), new XAttribute("Placement", Placement));
      XElement contextData = new XElement("ContextData", new XAttribute("VisitUID", Pathway ?? string.Empty), new XAttribute("PageCount", PageCount.ToString()));
      XElement requestXml = new XElement("RequestXml", clientData, contextData);

      return requestXml.ToString(SaveOptions.DisableFormatting);
    }

    public string GetCandidateRequestXml()
    {
      XElement candidateData = new XElement("CandidateData", new XAttribute("PrivateLabelID", PrivateLabelId.ToString()));

      if (!string.IsNullOrEmpty(SpoofOfferId))
      {
        XElement spoofData = new XElement("SpoofData",
          new XElement("SpoofDataItem", new XAttribute("Name", "RandomSelection"),
            new XElement("CandidateDataXml",
              new XElement("KeyValueCollection",
                new XElement("Items",
                  new XElement("Item", new XAttribute("key", "fbiOfferID"), new XAttribute("value", SpoofOfferId))
                  )))));
        candidateData.Add(spoofData);
      }

      return candidateData.ToString(SaveOptions.DisableFormatting);
    }
  }
}
