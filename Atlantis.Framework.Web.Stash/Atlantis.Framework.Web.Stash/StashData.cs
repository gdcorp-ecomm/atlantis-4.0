using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web.UI;

namespace Atlantis.Framework.Web.Stash
{
  internal class StashData
  {
    private HashSet<string> _keysUsed = new HashSet<string>();
    private StringBuilder _html = new StringBuilder(1000);

    public string GetHtml()
    {
      return _html.ToString();
    }

    public void RenderIntoStash(StashContent stashContentControl)
    {
      if ((stashContentControl.Visible) && (!AlreadyRendered(stashContentControl)))
      {
        // Render into the stash
        if (!string.IsNullOrEmpty(stashContentControl.RenderKey))
        {
          _keysUsed.Add(stashContentControl.RenderKey);
        }

        using (TextWriter tw = new StringWriter(_html))
        {
          using (HtmlTextWriter formWriter = new HtmlTextWriter(tw, string.Empty))
          {
            formWriter.Indent = 0;
            stashContentControl.RenderContentsForStash(formWriter);
            tw.Flush();
          }
        }
      }
    }

    private bool AlreadyRendered(StashContent stashContentControl)
    {
      return (!string.IsNullOrEmpty(stashContentControl.RenderKey)) && (_keysUsed.Contains(stashContentControl.RenderKey));
    }

  }
}
