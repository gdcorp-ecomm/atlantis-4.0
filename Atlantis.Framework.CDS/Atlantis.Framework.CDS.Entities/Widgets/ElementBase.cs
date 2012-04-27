using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Attributes;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class ElementBase
  {
    public ElementBase()
    {
    }

    public string Text { get; set; }
    // TODO: Update attribute(s) once filtering is in place.
    [HideInManagementUI]
    public string DataCenters { get; set; }
    [HideInManagementUI]
    public string Split { get; set; }
    [HideInManagementUI]
    public int ProductGroupOffered { get; set; }
    [HideInManagementUI]
    public bool ManagerOnly { get; set; }
    [HideInManagementUI]
    public bool? TechSupportIs24X7 { get; set; }
  }
}
