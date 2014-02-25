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
    public MailApiResponseState State { get; set; }

    public string JsonResponseData { get; set; }

    public static LoginResponseData FromJsonData(string jsonString)
    {
    
      MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

      DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(LoginResponseData));

      LoginResponseData result = (LoginResponseData)serializer.ReadObject(stream);
      
      result.JsonResponseData = jsonString;
      
      return result;
    }

  }
}
