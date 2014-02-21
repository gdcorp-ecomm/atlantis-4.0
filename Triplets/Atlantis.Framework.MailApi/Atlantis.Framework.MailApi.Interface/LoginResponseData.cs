using Atlantis.Framework.Interface;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;

namespace Atlantis.Framework.MailApi.Interface
{
  [DataContract]
  public class LoginResponseData : IResponseData
  {
    [DataMember(Name = "response")]
    public LoginData LoginData { get; set; }

    [DataMember(Name = "state")]
    public MailApiResponseState State { get; set; }


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
