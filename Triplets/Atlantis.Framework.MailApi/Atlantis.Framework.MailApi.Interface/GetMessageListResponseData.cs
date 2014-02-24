using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.MailApi.Interface
{
  public class GetMessageListResponseData : IResponseData
  {
    // Sample static method for creating the response (ref Clean Code)
    public static GetMessageListResponseData FromJsonData(string jsonString)
    {
      return new GetMessageListResponseData();
    }

    public string ToXML()
    {
      // Use this method to output small debug xml.
      XElement element = new XElement("GetMessageListResponseData");
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      // Use an AtlantisException member variable if your triplet request needs to create a response
      // with an exception 
      return null;
    }
  }
}
