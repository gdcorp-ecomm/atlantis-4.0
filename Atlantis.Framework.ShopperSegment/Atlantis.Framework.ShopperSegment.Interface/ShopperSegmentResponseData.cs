using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ShopperSegment.Interface
{
  public class ShopperSegmentResponseData : IResponseData
  {
    private AtlantisException _exception = null;

    public ShopperSegmentResponseData()
    {
    }
    
    public ShopperSegmentResponseData(string shopperId, int privateLabelId, int segmentId)
    {
      this.ShopperId = shopperId;
      this.PrivateLabelId = privateLabelId;
      this.SegmentId = segmentId;
    }

    public ShopperSegmentResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public ShopperSegmentResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData, "ShopperSegmentResponseData", exception.Message, requestData.ToXML());
    }

    public bool IsSuccess
    {
      get
      {
        return null == _exception;
      }
    }

    public int PrivateLabelId { get; set; }

    public int SegmentId { get; set; }

    public string ShopperId { get; set; }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof(ShopperSegmentResponseData));
      StringWriter writer = new StringWriter();

      xmlSerializer.Serialize(writer, this);

      return writer.ToString();
    }

  }
}
