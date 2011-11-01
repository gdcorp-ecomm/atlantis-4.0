using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RegGetDotTypeStackInfo.Interface
{
  public class RegGetDotTypeStackInfoRequestData : RequestData
  {
    private int _privateLabelId;
    public int PrivateLabelId
    {
      get
      {
        return _privateLabelId;
      }
    }

    private string _currencyType = string.Empty;
    public string CurrencyType
    {
      get
      {
        return _currencyType;
      }
    }
            
    public RegGetDotTypeStackInfoRequestData(string shopperId, string sourceUrl
                              , string orderId, string pathway
                              , int pageCount, int privateLabelId
                              , string currencyType) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _privateLabelId = privateLabelId;
      _currencyType = currencyType;
    }


    #region RequestData Members

    public override string ToXML()
    {
      StringBuilder sbResult = new StringBuilder();
      XmlTextWriter xtwResult = new XmlTextWriter(new StringWriter(sbResult));

      xtwResult.WriteStartElement("RegGetDotTypeStackInfo");

      xtwResult.WriteStartElement("param");
      xtwResult.WriteAttributeString("name", "privateLabelId");
      xtwResult.WriteAttributeString("value", _privateLabelId.ToString());
      xtwResult.WriteEndElement();

      xtwResult.WriteStartElement("param");
      xtwResult.WriteAttributeString("name", "currencyType");
      xtwResult.WriteAttributeString("value", _currencyType);
      xtwResult.WriteEndElement();

      xtwResult.WriteEndElement();

      return sbResult.ToString();
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(_currencyType + "|" + _privateLabelId.ToString());
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }

    #endregion
  }
}
