using System.Xml.Serialization;
using Atlantis.Framework.Interface;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Atlantis.Framework.MailApi.Interface
{
  [DataContract]
  public class LoginResponseData : MailApiResponseBase
  {
    [DataMember(Name = "response")]
    public LoginData LoginData { get; set; }

    [DataMember(Name = "state")]
    [XmlElement(ElementName = "state")]
    public MailApiResponseState State { get; set; }

    public static LoginResponseData FromJsonData(string jsonString)
    {
      MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

      DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(LoginResponseData));

      LoginResponseData result = (LoginResponseData)serializer.ReadObject(stream);
      result.MailApiResponseString = jsonString;
      return result;
    }

  }
}
