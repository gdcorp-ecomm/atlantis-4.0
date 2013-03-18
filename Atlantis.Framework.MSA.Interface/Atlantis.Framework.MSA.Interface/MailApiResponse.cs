using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.MSA.Interface
{
  public interface MailApiResponse
  {

    bool isJsoapFault();

    string getJsoapFaultMessage();

  }
}
