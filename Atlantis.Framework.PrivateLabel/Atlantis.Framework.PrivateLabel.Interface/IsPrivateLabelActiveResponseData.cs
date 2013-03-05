using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.PrivateLabel.Interface
{
  public class IsPrivateLabelActiveResponseData : IResponseData
  {
    private AtlantisException Exception { get; set; }
    public bool IsActive { get; private set; }

    private IsPrivateLabelActiveResponseData()
    {
      IsActive = false;
      Exception = null;
    }

    public static IsPrivateLabelActiveResponseData FromException(AtlantisException exception)
    {
      IsPrivateLabelActiveResponseData result = new IsPrivateLabelActiveResponseData();
      result.Exception = exception;
      return result;
    }

    public static IsPrivateLabelActiveResponseData FromIsActive(bool isActive)
    {
      IsPrivateLabelActiveResponseData result = new IsPrivateLabelActiveResponseData();
      result.IsActive = isActive;
      return result;
    }

    public string ToXML()
    {
      XElement element = new XElement("IsPrivateLabelActiveResponseData");
      element.Add(new XAttribute("isactive", IsActive.ToString().ToLowerInvariant()));
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return Exception;
    }
  }
}
