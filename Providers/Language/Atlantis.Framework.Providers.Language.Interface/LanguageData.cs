using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.Language.Interface
{
  public class LanguageData
  {
    public string FullLanguage { get; set; }
    public string ShortLanguage { get; set; }
    public string DefaultLanguage { get; set; }
    public string CountrySite { get; set; }
    public int ContextId { get; set; }
  }
}
