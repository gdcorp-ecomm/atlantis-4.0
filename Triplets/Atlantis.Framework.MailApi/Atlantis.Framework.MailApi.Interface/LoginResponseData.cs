using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.MailApi.Interface
{
  [DataContract]
  public class LoginResponseData : IResponseData
  {

    [DataMember(Name = "hash")]
    public string Hash { get; set; }

    [DataMember(Name = "baseurl")]
    public string BaseUrl { get; set; }

    [DataMember(Name = "clienturl")]
    public string ClientUrl { get; set; }

    public LoginResponseData()
    {
      Hash = "";
      BaseUrl = "";
      ClientUrl = "";
    }

    public static LoginResponseData FromJsonData(string jsonString)
    {

      MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

      DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(LoginResponseData));

      LoginResponseData result = (LoginResponseData)serializer.ReadObject(stream);

      return result;
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
