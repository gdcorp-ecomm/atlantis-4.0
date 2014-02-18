using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.MailApi.Interface
{
  public class LoginResponseData : IResponseData
  {
    // Sample static method for creating the response (ref Clean Code)
    public static LoginResponseData FromData(object data)
    {
      return new LoginResponseData(data);
    }

    // Sample private constructor
    private LoginResponseData(object data)
    {
      // Sample: Create member data as necessary
    }

    public string ToXML()
    {
      // Use this method to output small debug xml.
      XElement element = new XElement("LoginResponseData");
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
