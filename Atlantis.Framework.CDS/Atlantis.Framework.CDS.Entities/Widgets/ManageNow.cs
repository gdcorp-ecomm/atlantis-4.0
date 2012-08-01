using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Atlantis.Framework.Providers.Interface.Links;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class ManageNow : IWidgetModel
  {
    private string _linkText;
    [Required(ErrorMessage="Link Text is required")]
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

    private string _queryParamModeValue;
    public string QueryParamModeValue
    {
      get
      {
        return _queryParamModeValue;
      }
      set
      {
        QueryParamMode result;
        if (Enum.TryParse<QueryParamMode>(value, true, out result))
        {
          _queryParamModeValue = value;
        }
        else
        {
          throw new ArgumentException("Invalid QueryParamMode value");
        }
      }
    }

    public string ProductName { get; set; }
    [Required(ErrorMessage="CI Code is required.")]
    [RegularExpression(@"^\d+$", ErrorMessage="CI code must contain only digits.")]
    public string CiCode { get; set; }
    public string LinkType { get; set; }
    [Required(ErrorMessage="Relative Url is required. Use \"default.aspx\" for the MYA home page.")]
    [RegularExpression(@"^([A-Za-z0-9_\-]+/?)*\.[A-Za-z]+$", ErrorMessage="Invalid Relative Url. Relative Url can contain letters, digits, underscores, dashes, forward slashes as directory separators, and a dot followed by a file extension")]
    public string RelativeUrl { get; set; }
    public bool Secure { get; set; }
    public int Group { get; set; }
    public int AccId { get; set; }
  }
}
