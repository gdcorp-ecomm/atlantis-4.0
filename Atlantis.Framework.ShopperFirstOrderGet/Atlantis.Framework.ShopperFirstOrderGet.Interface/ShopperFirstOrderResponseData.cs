using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ShopperFirstOrderGet.Interface
{
  public class ShopperFirstOrderGetResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    
    public bool IsSuccess
    {
      get
      {
          return _exception == null;
      }
    }

    public bool IsShopperNew { get; set; }

    public ShopperFirstOrderGetResponseData(bool isShopperNew)
    {
        IsShopperNew = isShopperNew;
    }

     public ShopperFirstOrderGetResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public ShopperFirstOrderGetResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                   "ShopperFirstOrderGetResponseData",
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
        xtwRequest.WriteAttributeString("IsShopperNew", IsShopperNew.ToString());
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
