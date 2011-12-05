using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PhotoGetGalleryListEx.Impl.PhotoServiceWS;
using Atlantis.Framework.PhotoGetGalleryListEx.Interface;

namespace Atlantis.Framework.PhotoGetGalleryListEx.Impl
{
  public class PhotoGetGalleryListExRequest : IRequest
  {

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      PhotoGetGalleryListExResponseData responseData = null;
      PhotoServiceWS.PhotoServiceResponse outResponse = new PhotoServiceWS.PhotoServiceResponse();
      PhotoServiceWS.Gallery[] returnedGallery = null;
      List<GalleryItem> responseGallery = new List<GalleryItem>();

      PhotoServiceWS.PhotoService serviceWS = null;
      try
      {
        PhotoGetGalleryListExRequestData requestData = (PhotoGetGalleryListExRequestData)oRequestData;
        serviceWS = new PhotoServiceWS.PhotoService();
        serviceWS.Url = ((WsConfigElement)oConfig).WSURL;
        serviceWS.Timeout = (int)Math.Truncate(requestData.RequestTimeout.TotalMilliseconds);
        returnedGallery = serviceWS.GetGalleryListEx(requestData.Domain, out outResponse);

        if ((returnedGallery.Length > 0) && (outResponse.ResponseCode == 0))
        {
          foreach (Gallery gallery in returnedGallery)
          {
            GalleryItem data = new GalleryItem(gallery.GalleryId, gallery.Title, gallery.Notes, gallery.TotalPhotos);
            responseGallery.Add(data);
          }
        }

        responseData = new PhotoGetGalleryListExResponseData(responseGallery, outResponse.ResponseCode);

      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new PhotoGetGalleryListExResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new PhotoGetGalleryListExResponseData(responseGallery, oRequestData, ex);
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
