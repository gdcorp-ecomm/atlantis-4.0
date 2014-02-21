using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Atlantis.Framework.MailApi.Interface
{
  [DataContract]
  public class MailApiResponseState // encaspulates the "state" portion of mailapi responses
  {
    [DataMember(Name = "app_key")]
    public string AppKey { get; set; }
  }
}
