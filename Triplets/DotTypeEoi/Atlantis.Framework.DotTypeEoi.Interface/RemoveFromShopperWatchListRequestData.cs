using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  public class RemoveFromShopperWatchListRequestData : RequestData
  {
    private readonly string _shopperId;
    public readonly string GTldsXml;

    public RemoveFromShopperWatchListRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string gTldsXml)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      if (string.IsNullOrEmpty(gTldsXml))
      {
        throw new ArgumentException("gTldsXml cannot be null or empty.");
      }

      _shopperId = shopperId;
      GTldsXml = gTldsXml;
      
      RequestTimeout = TimeSpan.FromSeconds(10);
    }

    public override string ToXML()
    {
      var result = new XElement("request");
      result.Add(new XAttribute("shopperId", _shopperId));

      var gTlds = new XElement("gTldsXml") { Value = GTldsXml };
      result.Add(gTlds);

      return result.ToString(SaveOptions.DisableFormatting);
    }

    public override string GetCacheMD5()
    {
      throw new Exception("RemoveFromShopperWatchList is not a cacheable request.");
    }
  }
}
