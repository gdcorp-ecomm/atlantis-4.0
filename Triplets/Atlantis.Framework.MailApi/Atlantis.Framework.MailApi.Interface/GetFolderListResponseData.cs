using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.MailApi.Interface
{
  [DataContract]
  public class GetFolderListResponseData : MailApiResponseBase
  {

    [DataMember(Name = "response")]
    public MailFolderArray MailFolders { get; set; }

    [DataMember(Name = "state")]
    [XmlElement(ElementName = "state")]
    public MailApiResponseState State { get; set; }

    public static GetFolderListResponseData FromJsonData(string jsonString)
    {
      var resultFolderList = new GetFolderListResponseData() {MailFolders = new MailFolderArray()};
      if (string.IsNullOrEmpty(jsonString)) return resultFolderList;

      var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
      var serializer = new DataContractJsonSerializer(typeof(GetFolderListResponseData));
      resultFolderList = (GetFolderListResponseData)serializer.ReadObject(stream);
      resultFolderList.MailApiResponseString = jsonString;
      return resultFolderList;
    }	
  }
}
