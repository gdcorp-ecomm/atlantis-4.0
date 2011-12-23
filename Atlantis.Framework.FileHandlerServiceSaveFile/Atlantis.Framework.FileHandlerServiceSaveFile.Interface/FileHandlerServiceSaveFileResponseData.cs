using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FileHandlerServiceSaveFile.Interface
{
  public class FileHandlerServiceSaveFileResponseData : IResponseData
  {
    private AtlantisException _ex;
    private string _outputMessage = string.Empty;
    private bool _isSuccess = false;

    public FileHandlerServiceSaveFileResponseData(string outputMessage, bool success)
    {
      if (success)
      {
        _isSuccess = true;
      }
      _outputMessage = outputMessage;
    }

    public string OutputMessage
    {
      get { return _outputMessage; }
    }

    public bool Success
    {
      get { return _isSuccess; }
    }


    public FileHandlerServiceSaveFileResponseData(AtlantisException ex)
    {
      _isSuccess = false;
      _ex = ex;
    }

    public FileHandlerServiceSaveFileResponseData(RequestData oRequestData, Exception ex)
    {
      _isSuccess = false;
      _ex = new AtlantisException(oRequestData, "FileHandlerServiceSaveFileResponseData", ex.Message, oRequestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }
    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion


  }
}

