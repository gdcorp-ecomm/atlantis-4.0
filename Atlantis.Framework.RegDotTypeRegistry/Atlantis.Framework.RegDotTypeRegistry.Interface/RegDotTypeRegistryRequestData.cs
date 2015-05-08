using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.RegDotTypeRegistry.Interface
{
  public class RegDotTypeRegistryRequestData : RequestData
  {
    string _dotType;

    public RegDotTypeRegistryRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string dotType)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      if (string.IsNullOrEmpty(dotType))
      {
        throw new ArgumentException("DotType cannot be null or empty.");
      }

      _dotType = dotType.ToLowerInvariant();
      RequestTimeout = TimeSpan.FromSeconds(10);
    }

    public override string ToXML()
    {
      XElement result = new XElement("request");

      XElement register = new XElement("tldname", new XAttribute("type", "registration"));
      register.Value = _dotType;

      XElement transfer = new XElement("tldname", new XAttribute("type", "transfer"));
      transfer.Value = _dotType;

      result.Add(register, transfer);

      return result.ToString(SaveOptions.DisableFormatting);
    }

    public override string GetCacheMD5()
    {
      return _dotType;
    }
  }
}
