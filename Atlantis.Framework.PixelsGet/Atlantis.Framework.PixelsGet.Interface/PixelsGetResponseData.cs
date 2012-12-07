using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PixelsGet.Interface.PixelObjects;

namespace Atlantis.Framework.PixelsGet.Interface
{
  public class PixelsGetResponseData: IResponseData
  {

    private readonly AtlantisException _aex;
    public bool IsSuccess
    {
      get { return _aex == null; }
    }

    public List<Pixel> Pixels { get; set; }

    public PixelsGetResponseData(List<Pixel> pixels)
    {
      Pixels = pixels;
    }
    
    public PixelsGetResponseData(AtlantisException aex)
    {
      _aex = aex;
    }

    public PixelsGetResponseData(RequestData requestData, Exception ex)
    {
      _aex = new AtlantisException(requestData, "PixelsGetResponseData", ex.Message, ex.StackTrace);
    }

    public string ToXML()
    {
      var xmlValue = string.Empty;
      try
      {
        var writer = new StringWriter();
        var xmlSerializer = new XmlSerializer(typeof(Pixel));
        xmlSerializer.Serialize(writer, this.Pixels);
        xmlValue = writer.ToString();
      }
      catch (Exception) { }

      return xmlValue;
    }

    public AtlantisException GetException()
    {
      return _aex;
    }
  }
}
