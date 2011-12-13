using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.InstantStoreImages.Interface
{
  public class ImageData
  {
    public long BackgroundId { get; set; }
    public string Src { get; set; }
    public string ThumbnailSrc { get; set; }

    public string DefaultTitle { get; set; }
    public string DefaultDescription { get; set; }
    public string[] Categories { get; set; }

    public ImageData()
    {
      Src = string.Empty;
      ThumbnailSrc = string.Empty;
    }
    public ImageData(long backgroundId, string src, string thumbnailSrc)
    {
      BackgroundId = backgroundId;
      Src = src == null ? string.Empty : src;
      ThumbnailSrc = thumbnailSrc == null ? string.Empty : thumbnailSrc;
    }

    public ImageData(long backgroundId, string src, string thumbnailSrc, string defaultTitle, string defaultDescription, string[] categoriesField)
    {
      BackgroundId = backgroundId;
      Src = src == null ? string.Empty : src;
      ThumbnailSrc = thumbnailSrc == null ? string.Empty : thumbnailSrc;
      DefaultTitle = defaultTitle == null ? string.Empty : defaultTitle;
      DefaultDescription = defaultDescription == null ? string.Empty : defaultDescription;
      Categories = categoriesField;
    }

  }
}
