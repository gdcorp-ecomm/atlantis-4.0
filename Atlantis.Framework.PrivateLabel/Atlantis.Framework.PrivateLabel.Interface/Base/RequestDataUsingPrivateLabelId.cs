using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.PrivateLabel.Interface.Base
{
  public abstract class RequestDataUsingPrivateLabelId : RequestData
  {
    public int PrivateLabelId { get; private set; }

    public RequestDataUsingPrivateLabelId(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLabelId)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
    }

    public override string GetCacheMD5()
    {
      return PrivateLabelId.ToString();
    }

    public override string ToXML()
    {
      XElement element = new XElement("PrivateLabel");
      element.Add(new XAttribute("privatelabelid", PrivateLabelId.ToString()));
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
