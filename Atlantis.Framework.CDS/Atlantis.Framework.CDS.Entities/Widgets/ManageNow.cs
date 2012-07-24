using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class ManageNow : IWidgetModel
  {
    private string _linkText;
    [DisplayName("Link Text")]
    public string LinkText 
    { 
      get
      {
        if (_linkText == default(string))
        {
          _linkText = "Manage Now";
        }
        return _linkText;
      }
      set
      {
        _linkText = value;
      }
    }

    private string _description;
    public string Description
    {
      get
      {
        if (_description == default(string))
        {
          _description = "Already own this product?";
        }
        return _description;
      }
      set
      {
        _description = value;
      }
    }

    [DisplayName("Product Name")]
    public string ProductName { get; set; }
    [DisplayName("CI Code")]
    [Required(ErrorMessage="CI Code is required.")]
    [RegularExpression(@"^\d+$", ErrorMessage="CI code must contain only digits.")]
    public string CiCode { get; set; }
    [DisplayName("Link Type")]
    public string LinkType { get; set; }
    [DisplayName("Relative Url")]
    [Required(ErrorMessage="Relative Url is required. Use \"default.aspx\" for the MYA home page.")]
    [RegularExpression(@"^([A-Za-z0-9_\-]+/?)*\.[A-Za-z]+$", ErrorMessage="Invalid Relative Url. Relative Url can contain letters, digits, underscores, dashes, forward slashes as directory separators, and a dot followed by a file extension")]
    public string RelativeUrl { get; set; }
    [DisplayName("QueryParamMode Value")]
    public string QueryParamModeValue { get; set; }
    public bool Secure { get; set; }
    public int Group { get; set; }
    [DisplayName("Account Id")]
    public int AccId { get; set; }
  }
}
