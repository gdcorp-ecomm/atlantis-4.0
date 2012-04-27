using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CDS.Entities.Attributes
{
  public class SectionContainerAttribute : System.Attribute
  {
    public SectionContainerAttribute(string displayName)
    {
      this.DisplayName = displayName;
      this.OpenMarkup = "<fieldset><legend>" + displayName + "</legend>";
      this.CloseMarkup = "</fieldset>";
    }

    public SectionContainerAttribute(string openMarkup, string closeMarkup)
    {
      this.OpenMarkup = openMarkup;
      this.CloseMarkup = closeMarkup;
    }

    public string DisplayName { get; private set; }
    public string OpenMarkup { get; private set; }
    public string CloseMarkup { get; private set; }
  }
}
