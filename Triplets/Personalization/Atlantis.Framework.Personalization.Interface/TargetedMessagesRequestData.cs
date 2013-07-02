using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Personalization.Interface
{
  public class TargetedMessagesRequestData : RequestData
  {
    private const int DEFAULT_TIMEOUT = 10;
    public string AppId { get; set; }
    public string InteractionPoint { get; set; }
    public string ContextData { get; set; }
    public string ShopperData { get; set; }

    public TargetedMessagesRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string appID, string interactionPoint, 
                                      Dictionary<string, string> contextData, Dictionary<string, string> shopperData) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      AppId = appID;
      InteractionPoint = interactionPoint;
      ContextData = ConvertRequestDataDictionary(contextData);
      ShopperData = ConvertRequestDataDictionary(shopperData);

      RequestTimeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT);
    }

    private string ConvertRequestDataDictionary(Dictionary<string, string> requestDataDictionary)
    {
      string dictionaryData = String.Empty;
      if (requestDataDictionary != null)
      {
        foreach (KeyValuePair<string, string> dataPair in requestDataDictionary)
        {
          dictionaryData += String.Format("{0}={1}|", dataPair.Key, dataPair.Value);
        }
        dictionaryData = dictionaryData.Remove(dictionaryData.LastIndexOf('|'));
      }
      return dictionaryData;
    }

    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();
      md5.Initialize();
      var requestXml = ToXML();
      var data = Encoding.UTF8.GetBytes(requestXml);
      var hash = md5.ComputeHash(data);
      var result = Encoding.UTF8.GetString(hash);
      return result;
    }

  }
}

