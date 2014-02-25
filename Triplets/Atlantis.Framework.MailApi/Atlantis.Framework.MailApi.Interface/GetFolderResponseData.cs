using Atlantis.Framework.Interface;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;

namespace Atlantis.Framework.MailApi.Interface
{
  [DataContract]
  public class GetFolderResponseData : IResponseData
  {
    [DataMember(Name = "response")]
    public MailFolder MailFolder { get; set; }

    public static GetFolderResponseData FromJsonData(string jsonString)
    {
      var resultFolder = new GetFolderResponseData(){MailFolder = new MailFolder()};
      if (string.IsNullOrEmpty(jsonString)) return resultFolder;

      var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
      var serializer = new DataContractJsonSerializer(typeof(GetFolderResponseData));
      resultFolder = (GetFolderResponseData)serializer.ReadObject(stream);
      
      return resultFolder;
    }

    public string ToXML()
    {
      return new XElement((XName)this.GetType().FullName).ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return (AtlantisException)null;
    }
  }
}
