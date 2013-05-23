using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  public class AddToShopperWatchListRequestData : RequestData
  {
    private readonly string _shopperId;

    private readonly string _gTldsXml;
    public string GTldsXml
    {
      get { return _gTldsXml; }
    }

    private readonly string _displayTime;
    public string DisplayTime
    {
      get { return _displayTime; }
    }

    public AddToShopperWatchListRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string displayTime, string gTldsXml)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      if (string.IsNullOrEmpty(displayTime) || string.IsNullOrEmpty(gTldsXml))
      {
        throw new ArgumentException("DisplayTime and gTldsXml cannot be null or empty.");
      }

      _shopperId = shopperId;
      _displayTime = displayTime;
      _gTldsXml = gTldsXml;
      
      RequestTimeout = TimeSpan.FromSeconds(10);
    }

    public override string ToXML()
    {
      var result = new XElement("request");
      result.Add(new XAttribute("shopperId", _shopperId));
      result.Add(new XAttribute("displayTime", _displayTime));

      var gTlds = new XElement("gTldsXml") {Value = _gTldsXml};
      result.Add(gTlds);

      return result.ToString(SaveOptions.DisableFormatting);
    }
  }
}
