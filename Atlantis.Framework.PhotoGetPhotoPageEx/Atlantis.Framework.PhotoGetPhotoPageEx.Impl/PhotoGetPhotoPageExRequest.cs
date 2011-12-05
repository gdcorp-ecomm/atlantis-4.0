using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PhotoGetPhotoPageEx.Impl.PhotoServiceWS;
using Atlantis.Framework.PhotoGetPhotoPageEx.Interface;

namespace Atlantis.Framework.PhotoGetPhotoPageEx.Impl
{
  public class PhotoGetPhotoPageExRequest : IRequest
  {

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      PhotoGetPhotoPageExResponseData responseData = null;
      PhotoServiceWS.PhotoServiceResponse outResponse = new PhotoServiceWS.PhotoServiceResponse();
      PhotoServiceWS.GalleryPhotoPage returnedPhotoPage = null;
      List<PhotoItem> responsePhotos = new List<PhotoItem>();

      PhotoServiceWS.PhotoService serviceWS = null;
      try
      {
        PhotoGetPhotoPageExRequestData requestData = (PhotoGetPhotoPageExRequestData)oRequestData;
        serviceWS = new PhotoServiceWS.PhotoService();
        serviceWS.Url = ((WsConfigElement)oConfig).WSURL;
        serviceWS.Timeout = (int)Math.Truncate(requestData.RequestTimeout.TotalMilliseconds);
        returnedPhotoPage = serviceWS.GetPhotoPageEx(requestData.GalleryId, requestData.PhotosPerPage, requestData.PageNumber, requestData.Domain, out outResponse);

        if ((returnedPhotoPage.TotalPhotos > 0) && (outResponse.ResponseCode == 0))
        {
          foreach (GalleryPhoto photo in returnedPhotoPage.Photos)
          {
            PhotoItem data = new PhotoItem(
              photo.PhotoId,
              photo.Title,
              photo.Notes,
              photo.UploadedDate,
              photo.Width,
              photo.Height,
              photo.Url,
              photo.UrlSmall,
              photo.UrlLarge,
              photo.UrlThumbnail,
              photo.SSLUrl,
              photo.SSLUrlSmall,
              photo.SSLUrlLarge,
              photo.SSLUrlThumbnail
            );
            responsePhotos.Add(data);
          }
        }

        responseData = new PhotoGetPhotoPageExResponseData(returnedPhotoPage.PageNumber, returnedPhotoPage.TotalPhotos, returnedPhotoPage.TotalPages, responsePhotos, outResponse.ResponseCode);

      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new PhotoGetPhotoPageExResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new PhotoGetPhotoPageExResponseData(responsePhotos, oRequestData, ex);
      }
      finally
      {
        if (serviceWS != null)
        {
          serviceWS.Dispose();
        }
      }
      return responseData;
    }
  }
}
