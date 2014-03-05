using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Atlantis.Framework.MailApi.Interface
{
  [DataContract]
  public class GetFolderResponseData : MailApiResponseBase
  {
    [DataMember(Name = "response")]
    public MailFolder MailFolder { get; set; }

    public GetFolderRequestData request { get; set; }

    public static GetFolderResponseData FromJsonData(string jsonString)
    {
      var resultFolder = new GetFolderResponseData(){MailFolder = new MailFolder()};
      if (string.IsNullOrEmpty(jsonString)) return resultFolder;

      var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
      var serializer = new DataContractJsonSerializer(typeof(GetFolderResponseData));
      resultFolder = (GetFolderResponseData)serializer.ReadObject(stream);
      
      return resultFolder;
    }

  }
}
