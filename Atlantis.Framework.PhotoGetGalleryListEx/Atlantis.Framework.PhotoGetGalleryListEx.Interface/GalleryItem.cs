
namespace Atlantis.Framework.PhotoGetGalleryListEx.Interface
{
  public class GalleryItem
  {
    private int _galleryId;
    private string _title;
    private string _notes;
    private int _totalPhotos;

    public int GalleryId
    {
      get { return _galleryId; }
    }

    public string Title
    {
      get { return _title; }
    }

    public string Notes
    {
      get { return _notes; }
    }

    public int TotalPhotos
    {
      get { return _totalPhotos; }
    }

    public GalleryItem(int galleryId, string title, string notes, int totalPhotos)
    {
      _galleryId = galleryId;
      _title = title;
      _notes = notes;
      _totalPhotos = totalPhotos;
    }
  }
}
