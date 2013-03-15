using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Atlantis.Framework.MSALoginUser.Interface
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

  [DataContract]
  public class LoginResponse
  {
    [DataMember(Name = "response")]
    public LoginData LoginData { get; set; }    
  }
}
