using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.RegDotTypeProductIds.Interface
{
    public class ProductIdListRequestData : RequestData
    {
      string _dotType;

      public ProductIdListRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string dotType)
        : base(shopperId, sourceURL, orderId, pathway, pageCount)
      {
        if (string.IsNullOrEmpty(dotType))
        {
          throw new ArgumentException("Dottype cannot be null or empty.");
        }

        _dotType = dotType.ToUpperInvariant();
        RequestTimeout = TimeSpan.FromSeconds(10);
      }

      public string DotType
      {
        get { return _dotType; }
      }

      public override string GetCacheMD5()
      {
        return _dotType;
      }

      public override string ToXML()
      {
        XElement requestElement = new XElement("request");
        requestElement.Add(new XAttribute("tldname", _dotType));
        requestElement.Add(new XAttribute("plgrouptype", "0"));
        return requestElement.ToString(SaveOptions.DisableFormatting);
      }
    }
}
