using System;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ShopperIsFlagged.Interface
{
  public class ShopperIsFlaggedResponseData : IResponseData
  {

    #region Properties
    private AtlantisException _exception;

    public bool IsSuccess { get; private set; }
    public bool IsFlagged { get; private set; }

    #endregion

    public ShopperIsFlaggedResponseData(bool isFlagged)
    {
      IsSuccess = true;
      IsFlagged = isFlagged;
    }

    public ShopperIsFlaggedResponseData(AtlantisException aex)
    {
      IsSuccess = false;
      _exception = aex;
    }

    public ShopperIsFlaggedResponseData(RequestData request, Exception ex)
    {
      IsSuccess = false;
      _exception = new AtlantisException(request, "ShopperIsFlaggedResponseData", ex.Message, string.Empty);
    }

    public string ToXML()
    {
      var xml = string.Empty;

      if (IsSuccess)
      {
        var serializer = new XmlSerializer(typeof(ShopperIsFlaggedResponseData));
        var writer = new StringWriter();
        serializer.Serialize(writer, this);
        xml = writer.ToString();
      }

      return xml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
