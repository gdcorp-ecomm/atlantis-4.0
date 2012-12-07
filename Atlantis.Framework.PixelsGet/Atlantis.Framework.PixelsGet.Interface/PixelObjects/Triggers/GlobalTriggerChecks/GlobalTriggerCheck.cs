using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlantis.Framework.PixelsGet.Interface.PixelObjects.Triggers.GlobalTriggerChecks
{
  public abstract  class GlobalTriggerCheck
  {

    public virtual string PixelXmlName
    {
      get { throw new NotImplementedException(); }
    }
    
    public virtual bool CeaseFiringTrigger(string actualValue)
    {
      throw new NotImplementedException();
    }
  }
}
