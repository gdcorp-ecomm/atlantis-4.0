using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.MailApi.Interface
{
  [DataContract]
  public class LoginData
  {

    [DataMember(Name = "hash")]
    public string Hash { get; set; }

    [DataMember(Name = "baseurl")]
    public string BaseUrl { get; set; }

    [DataMember(Name = "clienturl")]
    public string ClientUrl { get; set; }

    public LoginData()
    {
      Hash = "";
      BaseUrl = "";
      ClientUrl = "";
    }
  }
}
