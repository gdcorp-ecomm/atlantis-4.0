using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.PrivateLabel.Interface
{
  public class PrivateLabelIdRequestData : RequestData
  {
    public string ProgId { get; private set; }

    [Obsolete("Please use the simpler constructor.")]
    public PrivateLabelIdRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string progId)
      : this(progId)
    {
    }

    public PrivateLabelIdRequestData(string progId)
    {
      ProgId = progId ?? string.Empty;
    }

    public override string GetCacheMD5()
    {
      return ProgId.ToLowerInvariant();
    }

    public override string ToXML()
    {
      XElement element = new XElement("PrivateLabelId");
      element.Add(new XAttribute("progid", ProgId));
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
