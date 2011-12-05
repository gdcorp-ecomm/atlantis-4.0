using System;

namespace Atlantis.Framework.PhotoGetPhotoPageEx.Interface
{
  public class PhotoItem
  {
    private int _photoId;
    private string _title;
    private string _notes;
    private DateTime _uploadedDate;
    private int _width;
    private int _height;
    private string _url;
    private string _urlSmall;
    private string _urlLarge;
    private string _urlThumbnail;
    private string _sslUrl;
    private string _sslUrlSmall;
    private string _sslUrlLarge;
    private string _sslUrlThumbnail;

    public int PhotoId
    {
      get { return _photoId; }
    }

    public string Title
    {
      get { return _title; }
    }

    public string Notes
    {
      get { return _notes; }
    }

    public DateTime UploadedDate
    {
      get { return _uploadedDate; }
    }

    public int Width
    {
      get { return _width; }
    }

    public int Height
    {
      get { return _height; }
    }

    public string Url
    {
      get { return _url; }
    }

    public string UrlSmall
    {
      get { return _urlSmall; }
    }

    public string UrlLarge
    {
      get { return _urlLarge; }
    }

    public string UrlThumbnail
    {
      get { return _urlThumbnail; }
    }

    public string SSLUrl
    {
      get { return _sslUrl; }
    }

    public string SSLUrlSmall
    {
      get { return _sslUrlSmall; }
    }

    public string SSLUrlLarge
    {
      get { return _sslUrlLarge; }
    }

    public string SSLUrlThumbnail
    {
      get { return _sslUrlThumbnail; }
    }

    public PhotoItem(int photoId, string title, string notes, DateTime uploadedDate, int width,
      int height, string url, string urlSmall, string urlLarge, string urlThumbnail,
      string sslUrl, string sslUrlSmall, string sslUrlLarge, string sslUrlThumbnail)
    {
      _photoId = photoId;
      _title = title;
      _notes = notes;
      _uploadedDate = uploadedDate;
      _width = width;
      _height = height;
      _url = url;
      _urlSmall = urlSmall;
      _urlLarge = urlLarge;
      _urlThumbnail = urlThumbnail;
      _sslUrl = sslUrl;
      _sslUrlSmall = sslUrlSmall;
      _sslUrlLarge = sslUrlLarge;
      _sslUrlThumbnail = sslUrlThumbnail;
    }
  }
}
