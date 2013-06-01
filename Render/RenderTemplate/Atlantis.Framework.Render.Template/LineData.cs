using System.Collections.Generic;

namespace Atlantis.Framework.Render.Template
{
  internal class LineData
  {
    internal string Text { get; set; }

    internal LineType Type { get; set; }

    internal IDictionary<string, string> Attributes { get; set; } 
  }
}
