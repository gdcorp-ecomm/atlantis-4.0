using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Atlantis.Framework.Providers.Interface.Currency;

namespace Atlantis.Framework.TH.Products
{
  public class HtmlTagWrapFormatter : ISymbolFormatter
  {
    private readonly string _formatString = "{0}";
    public HtmlTagWrapFormatter(string tagName)
      : this(tagName, string.Empty)
    {
    }

    public HtmlTagWrapFormatter(string tagName, string className)
    {
      if (!string.IsNullOrEmpty(tagName))
      {
        string start = String.Format("<{{0}}{0}", (string.IsNullOrEmpty(className) ? ">" : " class=\"{1}\">"));
        _formatString = string.Format(string.Format("{0}{{{{0}}}}</{{0}}>", start), tagName, className);
      }
    }

    protected HtmlTagWrapFormatter()
      : this(string.Empty, string.Empty)
    {
      
    }

    public object GetFormat(Type formatType)
    {
      return formatType == typeof(ICustomFormatter) ? this : null;
    }

    public string Format(string fmt, object arg, IFormatProvider formatProvider)
    {
      string returnValue = string.Empty;
      
      returnValue = string.Format(_formatString, arg);

      return returnValue;
    }
  }
}
