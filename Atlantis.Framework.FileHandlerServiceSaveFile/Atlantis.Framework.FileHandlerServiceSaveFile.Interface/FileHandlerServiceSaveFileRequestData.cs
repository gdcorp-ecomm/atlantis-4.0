using System;
using System.IO;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FileHandlerServiceSaveFile.Interface
{
  public class FileHandlerServiceSaveFileRequestData : RequestData
  {
    private string _applicationData = string.Empty;
    private string _applicationKey = string.Empty;
    private string _environment = string.Empty;
    private string _fileNameOnly = null;
    private int _settingId = 0;
    private int _subscriberId = 0;
    private Stream _stream = null;


    public FileHandlerServiceSaveFileRequestData(
      string shopperId,
      string sourceURL,
      string orderId,
      string pathway,
      int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromMinutes(10);
    }


    public string ApplicationData
    {
      get { return _applicationData; }
      set { _applicationData = value; }
    }

    public string ApplicationKey
    {
      get { return _applicationKey; }
      set { _applicationKey = value; }
    }

    public string FileNameOnly
    {
      get { return _fileNameOnly; }
      set { _fileNameOnly = value; }
    }

    public int SettingId
    {
      get { return _settingId; }
      set { _settingId = value; }
    }

    public int SubscriberId
    {
      get { return _subscriberId; }
      set { _subscriberId = value; }
    }

    public string Environment
    {
      get { return _environment; }
      set { _environment = value; }
    }

    public Stream Stream
    {
      get { return _stream; }
      set { _stream = value; }
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("FileHandlerServiceSaveFile is not a cacheable request.");
    }

    public override string ToXML()
    {
      return string.Empty;
    }

  }
}
