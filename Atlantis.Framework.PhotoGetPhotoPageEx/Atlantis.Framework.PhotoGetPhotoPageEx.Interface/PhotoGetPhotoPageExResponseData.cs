using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PhotoGetPhotoPageEx.Interface
{
  public class PhotoGetPhotoPageExResponseData : IResponseData
  {
    private AtlantisException _exAtlantis = null;
    private List<PhotoItem> _photos;

    private bool _success = false;
    public bool IsSuccess { get { return _success; } }

    private int _pageNumber;
    public int PageNumber { get { return _pageNumber; } }

    private int _totalPhotos;
    public int TotalPhotos { get { return _totalPhotos; } }

    private int _totalPages;
    public int TotalPages { get { return _totalPages; } }

    public PhotoGetPhotoPageExResponseData(int pageNumber, int totalPhotos, int totalPages, IEnumerable<PhotoItem> photos, int responseCode)
    {
      _pageNumber = pageNumber;
      _totalPhotos = totalPhotos;
      _totalPages = totalPages;
      _photos = new List<PhotoItem>(photos);
      _success = (responseCode == 0);
    }

    public PhotoGetPhotoPageExResponseData(AtlantisException exAtlantis)
    {
      _exAtlantis = exAtlantis;
    }

    public PhotoGetPhotoPageExResponseData(IEnumerable<PhotoItem> photos, RequestData oRequestData, Exception ex)
    {
      _photos = new List<PhotoItem>(photos);
      _exAtlantis = new AtlantisException(oRequestData, "PhotoGetPhotoPageExResponseData", ex.Message, string.Empty);
    }

    public List<PhotoItem> PhotoList
    {
      get
      {
        return _photos;
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
