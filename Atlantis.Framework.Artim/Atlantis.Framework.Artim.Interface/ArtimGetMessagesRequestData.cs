using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Artim.Interface
{
  public class ArtimGetMessagesRequestData : RequestData
  {
    private const int DEFAULT_TIMEOUT = 10;
    public string AppId { get; set; }
    public string InteractionPoint { get; set; }
    public string ContextData { get; set; }
    public string ShopperData { get; set; }
    public string SpoofData { get; set; }
    public string InteractionData { get; set; }

    public ArtimGetMessagesRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string appID, string interactionPoint, 
                                      Dictionary<string, string> contextData, Dictionary<string, string> shopperData, Dictionary<string, string> spoofData, 
                                      Dictionary<string, string> interactionData) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      AppId = appID;
      InteractionPoint = interactionPoint;
      ContextData = ConvertRequestDataDictionary(contextData);
      ShopperData = ConvertRequestDataDictionary(shopperData);
      SpoofData = ConvertRequestDataDictionary(spoofData);
      InteractionData = ConvertRequestDataDictionary(interactionData);

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
      throw new NotImplementedException("GetCacheMD5 not implemented in ArtimGetMessagesRequestData");     
    }


  }
}

