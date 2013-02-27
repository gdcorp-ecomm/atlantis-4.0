using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using System.Security.Cryptography;

namespace Atlantis.Framework.EcommPricingEstimate.Interface
{
  public class EcommPricingEstimateRequestData : RequestData
  {
    public EcommPricingEstimateRequestData(string sShopperID,
            string sSourceURL,
            string sOrderID,
            string sPathway,
            int iPageCount,
      int membershipLevel,
      int privateLabelId,
      string transactionCurrency)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      MembershipLevel = membershipLevel;
      PrivateLabelID = privateLabelId;
      TransactionCurrency = transactionCurrency;
      RequestTimeout = TimeSpan.FromSeconds(4);
      Items = new List<PriceEstimateItem>();
    }


    public EcommPricingEstimateRequestData(string sShopperID,
            string sSourceURL,
            string sOrderID,
            string sPathway,
            int iPageCount,
      int membershipLevel,
      int privateLabelId,
      string transactionCurrency, int pf_id, string discount_code)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      MembershipLevel = membershipLevel;
      PrivateLabelID = privateLabelId;
      TransactionCurrency = transactionCurrency;
      RequestTimeout = TimeSpan.FromSeconds(4);
      Items = new List<PriceEstimateItem>();
      Items.Add(new PriceEstimateItem(pf_id, discount_code));
    }

    public int MembershipLevel { get; set; }
    public int PrivateLabelID { get; set; }
    public string TransactionCurrency { get; set; }

    public List<PriceEstimateItem> Items { get; set; }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement(PriceEstimateXmlNames.PriceEstimateRequest);
      xtwRequest.WriteAttributeString(PriceEstimateXmlNames.membershipLevel, MembershipLevel.ToString());
      xtwRequest.WriteAttributeString(PriceEstimateXmlNames.privateLabelID, PrivateLabelID.ToString());
      xtwRequest.WriteAttributeString(PriceEstimateXmlNames.transactionCurrency, TransactionCurrency);
      foreach (PriceEstimateItem pei in Items)
      {
        xtwRequest.WriteStartElement(PriceEstimateXmlNames.Item);
        xtwRequest.WriteAttributeString(PriceEstimateXmlNames.pf_id, pei.pf_id.ToString());
        xtwRequest.WriteAttributeString(PriceEstimateXmlNames.discount_code, pei.discount_code);
        xtwRequest.WriteEndElement();
      }
      xtwRequest.WriteEndElement();
      return sbRequest.ToString();
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      string uniqueKey = MembershipLevel.ToString() + "/" + PrivateLabelID.ToString() + "/" + TransactionCurrency;
      foreach (PriceEstimateItem pei in Items)
      {
        uniqueKey += "/" + pei.pf_id.ToString() + "/" + pei.discount_code;
      }
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(uniqueKey);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }
  }
}
