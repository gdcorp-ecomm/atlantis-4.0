using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.PrivateLabel.Interface
{
  public class PrivateLabelTypeResponseData : IResponseData
  {
    private AtlantisException Exception { get; set; }
    public int PrivateLabelType { get; private set; }

    private PrivateLabelTypeResponseData()
    {
      PrivateLabelType = 0;
      Exception = null;
    }

    public static PrivateLabelTypeResponseData FromException(AtlantisException exception)
    {
      PrivateLabelTypeResponseData result = new PrivateLabelTypeResponseData();
      result.Exception = exception;
      return result;
    }

    public static PrivateLabelTypeResponseData FromPrivateLabelType(int privateLabelType)
    {
      PrivateLabelTypeResponseData result = new PrivateLabelTypeResponseData();
      result.PrivateLabelType = privateLabelType;
      return result;
    }

    public string ToXML()
    {
      XElement element = new XElement("PrivateLabelTypeResponseData");
      element.Add(new XAttribute("privatelabeltype", PrivateLabelType));
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return Exception;
    }

  }
}
