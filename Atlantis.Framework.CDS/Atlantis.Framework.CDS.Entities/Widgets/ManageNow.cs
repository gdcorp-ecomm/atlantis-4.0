using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class ManageNow : IWidgetModel
  {
    public string LinkText { get; set; }
    public string Description { get; set; }
    public string ProductName { get; set; }
    public string CiCode { get; set; }
    public string LinkType { get; set; }
    public string RelativeUrl { get; set; }
    public string QueryParamModeValue { get; set; }
    public bool Secure { get; set; }
    public int Group { get; set; }
    public int AccId { get; set; }
  }
}
