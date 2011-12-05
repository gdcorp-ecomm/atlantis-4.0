using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PhotoGetGalleryListEx.Interface
{
  public class PhotoGetGalleryListExResponseData : IResponseData
  {
    private AtlantisException _exAtlantis = null;
    private List<GalleryItem> _gallery;

    private bool _success = false;
    public bool IsSuccess { get { return _success; } }

    public PhotoGetGalleryListExResponseData(IEnumerable<GalleryItem> gallery, int responseCode)
    {
      _gallery = new List<GalleryItem>(gallery);
      _success = (responseCode == 0);
    }

    public PhotoGetGalleryListExResponseData(AtlantisException exAtlantis)
    {
      _exAtlantis = exAtlantis;
    }

    public PhotoGetGalleryListExResponseData(IEnumerable<GalleryItem> gallery, RequestData oRequestData, Exception ex)
    {
      _gallery = new List<GalleryItem>(gallery);
      _exAtlantis = new AtlantisException(oRequestData, "PhotoGetGalleryListExResponseData", ex.Message, string.Empty);
    }

    public List<GalleryItem> GalleryList
    {
      get
      {
        return _gallery;
      }
    }

    public int GalleryCount
    {
      get
      {
        return _gallery.Count;
      }
    }


    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exAtlantis;
    }

    #endregion
  }

}
