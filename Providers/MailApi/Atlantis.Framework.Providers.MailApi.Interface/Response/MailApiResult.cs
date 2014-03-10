using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.MailApi.Interface.Response
{
  public interface MailApiResult
  {
    string Session { get; set; }

    bool IsMailApiFault { get; set; }
  }
}
