using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.PrivateLabel.Interface
{
  public class ProgIdResponseData : IResponseData
  {
    private AtlantisException Exception { get; set; }
    public string ProgId { get; private set; }

    private ProgIdResponseData()
    {
      ProgId = string.Empty;
      Exception = null;
    }

    public static ProgIdResponseData FromException(AtlantisException exception)
    {
      ProgIdResponseData result = new ProgIdResponseData();
      result.Exception = exception;
      return result;
    }

    public static ProgIdResponseData FromProgId(string progId)
    {
      ProgIdResponseData result = new ProgIdResponseData();
      if (progId != null)
      {
        result.ProgId = progId;
      }
      return result;
    }

    public string ToXML()
    {
      XElement element = new XElement("ProgIdResponseData");
      element.Add(new XAttribute("progid", ProgId));
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return Exception;
    }

  }
}
