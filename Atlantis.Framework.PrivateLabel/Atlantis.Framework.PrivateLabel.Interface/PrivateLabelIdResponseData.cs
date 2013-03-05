using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.PrivateLabel.Interface
{
  public class PrivateLabelIdResponseData : IResponseData
  {
    private AtlantisException Exception { get; set; }
    public int PrivateLabelId { get; private set; }

    private PrivateLabelIdResponseData()
    {
      PrivateLabelId = 0;
      Exception = null;
    }

    public static PrivateLabelIdResponseData FromException(AtlantisException exception)
    {
      PrivateLabelIdResponseData result = new PrivateLabelIdResponseData();
      result.Exception = exception;
      return result;
    }

    public static PrivateLabelIdResponseData FromPrivateLabelId(int privateLabelId)
    {
      PrivateLabelIdResponseData result = new PrivateLabelIdResponseData();
      result.PrivateLabelId = privateLabelId;
      return result;
    }

    public string ToXML()
    {
      XElement element = new XElement("PrivateLabelIdResponseData");
      element.Add(new XAttribute("privatelabelid", PrivateLabelId.ToString()));
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return Exception;
    }

  }
}
