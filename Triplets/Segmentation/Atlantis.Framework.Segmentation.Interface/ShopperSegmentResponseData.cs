using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.Segmentation.Interface
{
  public class ShopperSegmentResponseData : IResponseData, ISessionSerializableResponse
  {
    private AtlantisException _exception = null;

    public ShopperSegmentResponseData()
    {
    }
    
    public ShopperSegmentResponseData(int segmentId)
    {
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
    
    public int SegmentId { get; set; }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof(ShopperSegmentResponseData));
      StringWriter writer = new StringWriter();

      xmlSerializer.Serialize(writer, this);

      string returnValue = writer.ToString();

      return returnValue;
    }


    public string SerializeSessionData()
    {
      return this.ToXML();
    }

    public void DeserializeSessionData(string sessionData)
    {
      if (!string.IsNullOrEmpty(sessionData))
      {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(ShopperSegmentResponseData));
        StringReader reader = new StringReader(sessionData);
        ShopperSegmentResponseData temp = xmlSerializer.Deserialize(reader) as ShopperSegmentResponseData;
        if (!ReferenceEquals(null, temp))
        {
          this.SegmentId = temp.SegmentId;
        }
      }
    }
  }
}
