using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PhotoGetPhotoPageEx.Interface
{
  public class PhotoGetPhotoPageExRequestData : RequestData
  {
    private int _galleryId;
    private int _photosPerPage;
    private int _pageNumber;
    private string _domain;

    public PhotoGetPhotoPageExRequestData(
      string shopperID, string sourceURL, string orderID, string pathway, int pageCount,
      int galleryId, int photosPerPage, int pageNumber, string domain)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      _galleryId = galleryId;
      _photosPerPage = photosPerPage;
      _pageNumber = pageNumber;
      _domain = domain;
      RequestTimeout = new TimeSpan(0, 0, 4);
    }

    public int GalleryId
    {
      get { return _galleryId; }
      set { _galleryId = value; }
    }

    public int PhotosPerPage
    {
      get { return _photosPerPage; }
      set { _photosPerPage = value; }
    }

    public int PageNumber
    {
      get { return _pageNumber; }
      set { _pageNumber = value; }
    }

    public string Domain
    {
      get { return _domain; }
      set { _domain = value; }
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(_galleryId + ":" + _photosPerPage + ":" + _pageNumber + ":" + _domain);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }
  }
}
