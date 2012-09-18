using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballGetOffersMsgData.Interface
{
  public class FastballGetOffersMsgDataRequestData : RequestData
  {
    private int _privateLabelId;
    private int _appId;
    private string _placement = string.Empty;
    private string _repId;

    public FastballGetOffersMsgDataRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      int privateLabelId, int applicationId, string placement, IManagerContext managerContext, String isc)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(6);
      _privateLabelId = privateLabelId;
      _appId = applicationId;
      _placement = placement;
      _isc = isc;

      if (managerContext != null)
      {
        if (managerContext.IsManager)
        {
          _repId = managerContext.ManagerUserId;
        }
      }
    }

    private string _isc;
    public string ISC
    {
      get { return _isc; }
    }

    private string _candidateRequest;
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

    private string _channelRequest;
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

          _channelRequest = request.ToString(SaveOptions.DisableFormatting);
        }
        return _channelRequest;
      }
    }

    private string CacheKey
    {
      get { return _appId + ":" + _placement + ":" + _repId + ":" + ShopperID; }
    }

    public override string GetCacheMD5()
    {
      MD5 oMd5 = new MD5CryptoServiceProvider();
      oMd5.Initialize();
      byte[] stringBytes = Encoding.ASCII.GetBytes(CacheKey);
      byte[] md5Bytes = oMd5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }
  }
}
