using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Atlantis.Framework.RazorEngine.Compilation
{
  public class TypeContext
  {
    public TypeContext()
    {
      Namespaces = new HashSet<string>();
    }

    public string TemplateKey { get; set; }

    public string ClassName
    {
      get { return string.IsNullOrEmpty(TemplateKey) ? CompilerServices.GenerateClassName() : Regex.Replace(TemplateKey, @"[^A-Za-z]*", string.Empty); }
    }

    public Type ModelType { get; set; }

    public ISet<string> Namespaces { get; private set; }

    public string TemplateContent { get; set; }

    public Type TemplateType { get; set; }

    public string SaveLocation { get; set; }
  }
}