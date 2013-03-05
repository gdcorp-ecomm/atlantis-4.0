using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.PrivateLabel.Interface
{
  public class PrivateLabelIdRequestData : RequestData
  {
    public string ProgId { get; private set; }

    public PrivateLabelIdRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string progId)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
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
