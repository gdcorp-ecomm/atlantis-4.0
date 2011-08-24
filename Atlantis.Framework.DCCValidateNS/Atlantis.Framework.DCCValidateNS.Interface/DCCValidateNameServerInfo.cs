using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.DCCValidateNS.Interface
{
  public class DCCValidateNameServerInfo
  {
    public enum NameServerTypeEnum
    {
      THIRDPARTY=0,
      GODADDY,
      PREMIUM,
      VANITY
    }
    public bool Valid { get; set; }
    public string NameServer{get;set;}
    public NameServerTypeEnum NameServerType { get; set; }

  }
}
