using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;
using System.ComponentModel;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class ManageNow : IWidgetModel
  {
    [DisplayName("Link Text")]
    public string LinkText { get; set; }
    public string Description { get; set; }
    [DisplayName("Product Name")]
    public string ProductName { get; set; }
    [DisplayName("CI Code")]
    public string CiCode { get; set; }
    [DisplayName("Link Type")]
    public string LinkType { get; set; }
    [DisplayName("Relative Url")]
    public string RelativeUrl { get; set; }
    [DisplayName("QueryParamMode Value")]
    public string QueryParamModeValue { get; set; }
    public bool Secure { get; set; }
    public int Group { get; set; }
    [DisplayName("Account Id")]
    public int AccId { get; set; }
  }
}
