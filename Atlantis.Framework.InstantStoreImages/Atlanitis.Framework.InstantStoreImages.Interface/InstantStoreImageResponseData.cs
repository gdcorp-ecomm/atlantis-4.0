using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using System.Xml;
using System.IO;

namespace Atlantis.Framework.InstantStoreImages.Interface
{
  public class InstantStoreImageResponseData:IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;
    private List<ImageData> _imageResults = new List<ImageData>();
    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public List<ImageData> BackgroundImages
    {
      get
      {
        return _imageResults;
      }
    }

    public InstantStoreImageResponseData(List<ImageData> results)
    {
      _imageResults = results;
      _success = true;
    }

    public InstantStoreImageResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public InstantStoreImageResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "InstantStoreImageResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("INFO");
      foreach (ImageData currentImage in _imageResults)
      {
        xtwRequest.WriteStartElement("BackgroundImages");
        xtwRequest.WriteAttributeString("BackgroundID", currentImage.BackgroundId.ToString());
        xtwRequest.WriteAttributeString("SrcURL", currentImage.Src);
        xtwRequest.WriteAttributeString("ThumbnailURL", currentImage.ThumbnailSrc);
        xtwRequest.WriteEndElement();
      }
      xtwRequest.WriteEndElement();
      return sbRequest.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
