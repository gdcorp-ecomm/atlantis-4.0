using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPricingEstimate.Interface
{
  public class EcommPricingEstimateResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;

    public EcommPricingEstimateResponseData()
    {
      Items = new List<PriceEstimateItem>();
    }

    public EcommPricingEstimateResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public EcommPricingEstimateResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "EcommPricingEstimateResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    public bool IsSuccess
    {
      get
      {
        return (_exception == null && Items != null);
      }
    }
    public int MembershipLevel { get; set; }
    public int PrivateLabelID { get; set; }
    public string TransactionCurrency { get; set; }
    public List<PriceEstimateItem> Items { get; set; }

    #region IResponseData Members
    public string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      if (!string.IsNullOrEmpty(_resultXML))
      {
        XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

        xtwRequest.WriteStartElement(PriceEstimateXmlNames.PriceEstimate);
        xtwRequest.WriteAttributeString(PriceEstimateXmlNames.membershipLevel, MembershipLevel.ToString());
        xtwRequest.WriteAttributeString(PriceEstimateXmlNames.privateLabelID, PrivateLabelID.ToString());
        xtwRequest.WriteAttributeString(PriceEstimateXmlNames.transactionCurrency, TransactionCurrency);
        foreach (PriceEstimateItem pei in Items)
        {
          xtwRequest.WriteStartElement(PriceEstimateXmlNames.Item);
          xtwRequest.WriteAttributeString(PriceEstimateXmlNames.pf_id, pei.pf_id.ToString());
          xtwRequest.WriteAttributeString(PriceEstimateXmlNames.name, pei.name);
          xtwRequest.WriteAttributeString(PriceEstimateXmlNames.discount_code, pei.discount_code);
          xtwRequest.WriteAttributeString(PriceEstimateXmlNames.list_price, pei.list_price.ToString());
          xtwRequest.WriteAttributeString(PriceEstimateXmlNames._oadjust_adjustedprice, pei.adjusted_price.ToString());
          xtwRequest.WriteAttributeString(PriceEstimateXmlNames._icann_fee_adjusted, pei.icann_fee.ToString());
          xtwRequest.WriteEndElement();
        }
        xtwRequest.WriteEndElement();
      }
      return sbRequest.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
