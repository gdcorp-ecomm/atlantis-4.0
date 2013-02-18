using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PromoReferISCAvailCheck.Interface
{
  public class PromoReferISCAvailCheckResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    
    public bool IsSuccess
    {
      get
      {
          return _exception == null;
      }
    }

    private int _isAvailable;

    public bool IsPromoAvailable { get { return _isAvailable == 1? true: false; } }

    public PromoReferISCAvailCheckResponseData(int isAvailable)
    {
      _isAvailable = isAvailable;
    }

     public PromoReferISCAvailCheckResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public PromoReferISCAvailCheckResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                   "PromoReferISCAvailCheckResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
        StringBuilder sbResult = new StringBuilder();
        XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbResult));

        xtwRequest.WriteStartElement("response");
        xtwRequest.WriteAttributeString("success", IsSuccess.ToString());
        xtwRequest.WriteAttributeString("IsPromoAvailable", IsPromoAvailable.ToString());
        xtwRequest.WriteEndElement();

        return sbResult.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
