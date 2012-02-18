using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using System.Xml;

namespace Atlantis.Framework.FastballGetOffersList.Interface
{
  public class FastballGetOffersListRequestData : RequestData
  {
    readonly TimeSpan TWO_MINUTES = TimeSpan.FromMinutes(2);

    public OapiStandardParams OAPIParams { get; private set; }

    public FastballGetOffersListRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      OapiStandardParams oapiParams)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TWO_MINUTES;
      OAPIParams = oapiParams;
    }

    private string _candidateRequest;
    public string CandidateRequestXml
    {
      get
      {
        if (_candidateRequest == null)
        {
          string iscCode = !string.IsNullOrEmpty(OAPIParams.OptionalIscCode) ? string.Format("ISC=\"{0}\" ", OAPIParams.OptionalIscCode) : string.Empty;
          string dispCurrency = !string.IsNullOrEmpty(OAPIParams.DisplayCurrency) ? string.Format("DisplayCurrency=\"{0}\" ", OAPIParams.DisplayCurrency) : string.Empty;
          string tranCurrency = !string.IsNullOrEmpty(OAPIParams.TransactionalCurrency) ? string.Format("TransactionalCurrency=\"{0}\" ", OAPIParams.TransactionalCurrency) : string.Empty;

          //CRM Deal of the Day
          if (!string.IsNullOrEmpty(OAPIParams.RepVersion)) tranCurrency = string.Format("TransactionalCurrency=\"{0}\" ", OAPIParams.TransactionalCurrency);

          _candidateRequest = string.Format("<CandidateData PrivateLabelID=\"{0}\" ShopperID=\"{1}\" {2} {3} {4}", OAPIParams.PrivateLabelId, OAPIParams.ShopperId, iscCode, dispCurrency, tranCurrency);
          _candidateRequest += ((!OAPIParams.QaSpoofDate.HasValue) ? @"/>" : string.Format("><SpoofData><SpoofDataItem Name=\"qaspoofdate\" Value=\"{0}\" /></SpoofData></CandidateData>", XmlConvert.ToString(OAPIParams.QaSpoofDate.Value, XmlDateTimeSerializationMode.Unspecified)));
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
          _channelRequest = string.Format("<RequestXml><ClientData AppID=\"{0}\" Placement=\"{1}\" /></RequestXml>", OAPIParams.ApplicationId, OAPIParams.Placement);
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
