using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.QSCThemeImages.Interface
{
  public class QSCThemeImagesResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;

    private List<ThemeData> _themes = new List<ThemeData>();
    public List<ThemeData> Themes
    {
      get
      {
        return _themes;
      }
      set
      {
        _themes = value;
      }
    }

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public QSCThemeImagesResponseData(List<ThemeData> themes)
    {
      _success = true;
      _themes = themes;
    }

     public QSCThemeImagesResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public QSCThemeImagesResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "QSCThemeImagesResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
