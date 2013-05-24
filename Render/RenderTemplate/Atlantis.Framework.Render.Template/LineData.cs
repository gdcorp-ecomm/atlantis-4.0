using System.Collections.Generic;

namespace Atlantis.Framework.Render.Template
{
  internal class LineData
  {
    internal LineType Type { get; set; }

    internal IDictionary<string, string> Attributes { get; set; } 
  }
}
