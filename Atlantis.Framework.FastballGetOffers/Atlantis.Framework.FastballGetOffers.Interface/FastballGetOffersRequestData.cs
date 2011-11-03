using System;
using System.Security.Cryptography;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballGetOffers.Interface
{
  public class FastballGetOffersRequestData : RequestData
  {
    public TimeSpan RequestTimeout { get; set; }

    private int _privateLabelId = 0;
    private int _appId = 0;
    private string _placement = string.Empty;
    private string _repId = null;

    public FastballGetOffersRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      int privateLabelId, int applicationId, string placement, IManagerContext managerContext)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(6);
      _privateLabelId = privateLabelId;
      _appId = applicationId;
      _placement = placement;

      if (managerContext != null)
      {
        if (managerContext.IsManager)
        {
          _repId = managerContext.ManagerUserId;
        }
      }
    }

    private void ResetRequestXml()
    {
      _candidateRequest = null;
      _channelRequest = null;
    }

    private string _candidateRequest = null;
    public string CandidateRequestXml
    {
      get
      {
        if (_candidateRequest == null)
        {
          XElement candidateData = new XElement("CandidateData");
          candidateData.SetAttributeValue("PrivateLabelID", _privateLabelId.ToString());
          candidateData.SetAttributeValue("ShopperID", ShopperID);
          _candidateRequest = candidateData.ToString(SaveOptions.DisableFormatting);
        }
        return _candidateRequest;
      }
    }

    private string _channelRequest = null;
    public string ChannelRequestXml
    {
      get
      {
        if (_channelRequest == null)
        {
          XElement request = new XElement("RequestXml");

          XElement clientData = new XElement("ClientData");
          clientData.SetAttributeValue("AppID", _appId.ToString());
          clientData.SetAttributeValue("Placement", _placement);
          request.Add(clientData);

          XElement contextData = new XElement("ContextData");
          contextData.SetAttributeValue("VisitUID", Pathway);
          contextData.SetAttributeValue("PageCount", PageCount.ToString());
          if (!string.IsNullOrEmpty(_repId))
          {
            contextData.SetAttributeValue("RepID", _repId);
          }
          request.Add(contextData);

          _channelRequest = request.ToString(SaveOptions.DisableFormatting);
        }
        return _channelRequest;
      }
    }

    private string CacheKey
    {
      get { return _appId.ToString() + ":" + _placement + ":" + _repId + ":" + ShopperID; }
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(CacheKey);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }
  }
}
