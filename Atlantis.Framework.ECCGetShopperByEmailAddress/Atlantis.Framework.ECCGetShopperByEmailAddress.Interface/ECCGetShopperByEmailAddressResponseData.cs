using System;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetShopperByEmailAddress.Interface
{
  public class ECCGetShopperByEmailAddressResponseData: IResponseData
  {

    #region Properties
    private AtlantisException _exception;

    private readonly bool _success;
    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    private readonly string _shopperId;
    public string ShopperID
    {
      get
      {
        return _shopperId;
      }
    }
    
    #endregion
    public ECCGetShopperByEmailAddressResponseData(string shopperId)
    {
      _shopperId = shopperId;
      _success = true;
    }

    public ECCGetShopperByEmailAddressResponseData(RequestData request, Exception ex)
    {
      _exception = new AtlantisException(request, "ECCGetShopperByEmailRequestData", ex.Message, string.Empty, ex);
    }

    public ECCGetShopperByEmailAddressResponseData(AtlantisException ex)
    {
      _exception = ex;
    }

    public string ToXML()
    {
      var sw = new StringWriter();
      var serializer = new XmlSerializer(typeof(string));
      serializer.Serialize(sw, ShopperID);
      return sw.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
