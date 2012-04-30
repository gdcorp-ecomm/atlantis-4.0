using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CDS.Entities.Attributes
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
  public sealed class FriendlyNameAttribute : System.Attribute
  {
    public FriendlyNameAttribute(string friendlyName)
    {
      this.FriendlyName = friendlyName;
    }

    public string FriendlyName { get; private set; }
  }
}
