using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Atlantis.Framework.MailApi.Interface
{
  [DataContract]
  public class GetMessageListResponseData : MailApiResponseBase
  {
    [DataMember(Name = "response")]
    public GetMessageListData MessageListData { get; set; }

    [DataMember(Name = "state")]
    public MailApiResponseState State { get; set; }

    public GetMessageListRequestData request { get; set; }

    public static GetMessageListResponseData FromJsonData(string jsonString)
    {
      MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

      DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GetMessageListResponseData));

      GetMessageListResponseData result = (GetMessageListResponseData)serializer.ReadObject(stream);

      return result;
    }

  }
}
