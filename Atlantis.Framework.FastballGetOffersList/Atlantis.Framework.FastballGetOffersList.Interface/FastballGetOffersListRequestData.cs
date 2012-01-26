using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballGetOffersList.Interface
{
  public class FastballGetOffersListRequestData : RequestData
  {
    readonly TimeSpan TWO_MINUTES = TimeSpan.FromMinutes(2);

    public bool ReturnStubbedData { get; set; }

    public int PrivateLabelId { get; set; }
    public int AppId { get; set; }
    public string Placement { get; set; }
    public string RepId { get; set; }

    public FastballGetOffersListRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      int privateLabelId, int applicationId, string placement, IManagerContext managerContext)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TWO_MINUTES;
      PrivateLabelId = privateLabelId;
      AppId = applicationId;
      Placement = placement;

      if (managerContext != null)
      {
        if (managerContext.IsManager)
        {
          RepId = managerContext.ManagerUserId;
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
          candidateData.SetAttributeValue("PrivateLabelID", PrivateLabelId.ToString());
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
          clientData.SetAttributeValue("AppID", AppId.ToString());
          clientData.SetAttributeValue("Placement", Placement);
          request.Add(clientData);

          _channelRequest = request.ToString(SaveOptions.DisableFormatting);
        }
        return _channelRequest;
      }
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("This data is not intended to be cached");
    }
  }
}
