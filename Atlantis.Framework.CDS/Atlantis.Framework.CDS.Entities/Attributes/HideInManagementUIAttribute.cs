using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CDS.Entities.Attributes
{
  [AttributeUsage(AttributeTargets.Property)]
  public class HideInManagementUIAttribute : System.Attribute
  {
    public HideInManagementUIAttribute()
    {
      this.Hide = true;
    }

    public HideInManagementUIAttribute(bool hide)
    {
      this.Hide = hide;
    }

    public bool Hide { get; private set; }
  }
}
