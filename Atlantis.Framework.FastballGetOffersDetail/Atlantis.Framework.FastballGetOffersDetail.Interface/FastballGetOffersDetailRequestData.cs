using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballGetOffersDetail.Interface
{
  public class FastballGetOffersDetailRequestData : RequestData
  {
    readonly TimeSpan TWO_MINUTES = TimeSpan.FromMinutes(2);

    public bool ReturnStubbedData { get; set; }

    private int _privateLabelId;
    private int _appId;
    private string _placement = string.Empty;
    private string _repId;
    private List<int> _fbiOfferIdList;

    public FastballGetOffersDetailRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      int privateLabelId, int applicationId, string placement, IManagerContext managerContext, List<int> fbiOfferIdList)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TWO_MINUTES;
      _privateLabelId = privateLabelId;
      _appId = applicationId;
      _placement = placement;
      _fbiOfferIdList = fbiOfferIdList;

      if (managerContext != null)
      {
        if (managerContext.IsManager)
        {
          _repId = managerContext.ManagerUserId;
        }
      }
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
      get { return string.Join("|", _fbiOfferIdList.ToArray()); }
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
