using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ShopperPrefGetModDate.Interface
{
  public class ShopperPrefGetModDateResponseData : IResponseData
  {
    private bool _success = false;
    private string _shopperId = string.Empty;
    private DateTime _modifyDate = DateTime.MinValue;
    private AtlantisException _exception = null;

    public string ShopperId
    {
      get { return _shopperId; }
    }

    public DateTime ModifyDate
    {
      get { return _modifyDate; }
    }

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public ShopperPrefGetModDateResponseData()
    {
    }

    public ShopperPrefGetModDateResponseData(string shopperId, DateTime modDate)
    {
      _shopperId = shopperId;
      _modifyDate = modDate;
      _success = true;
    }

    public ShopperPrefGetModDateResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public ShopperPrefGetModDateResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "ShopperPrefGetModDateResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      StringBuilder sbResult = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbResult));

      xtwRequest.WriteStartElement("response");

      xtwRequest.WriteAttributeString("shopper_id", _shopperId);
      xtwRequest.WriteAttributeString("modifyDate", _modifyDate.ToString());
      xtwRequest.WriteAttributeString("success", _success.ToString());
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
