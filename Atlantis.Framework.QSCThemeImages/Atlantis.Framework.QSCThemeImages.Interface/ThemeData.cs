using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.QSCThemeImages.Interface
{
  [Serializable()]
  public class ThemeData
  {
    public string BackgroundId { get; set; }
    public string Src { get; set; }
    public string ThumbnailSrc { get; set; }

    public ThemeData()
    {
      Src = string.Empty;
      ThumbnailSrc = string.Empty;
    }
    public ThemeData(string backgroundId, string src, string thumbnailSrc)
    {
      BackgroundId = backgroundId;
      Src = src == null ? string.Empty : src;
      ThumbnailSrc = thumbnailSrc == null ? string.Empty : thumbnailSrc;
    }

  }
}
