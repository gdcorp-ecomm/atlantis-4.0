using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ShopperPrefGet.Interface
{
  public class ShopperPrefGetResponseData : IResponseData
  {
    private bool _success = false;
    private string _shopperId = string.Empty;
    private Dictionary<string, string> _preferences;
    private DateTime _createDate = DateTime.MinValue;
    private DateTime _lastModifiedDate = DateTime.MinValue;

    private AtlantisException _exception = null;

    public string ShopperId
    {
      get { return _shopperId; }
    }

    public DateTime LastModifiedDate
    {
      get { return _lastModifiedDate; }
    }

    public DateTime CreateDate
    {
      get { return _createDate; }
    }

    public bool IsSuccess
    {
      get { return _success; }
    }

    public ShopperPrefGetResponseData()
    {
    }

    public ShopperPrefGetResponseData(string shopperId, DateTime createDate, DateTime lastModifiedDate, Dictionary<string, string> preferences)
    {
      _shopperId = shopperId;
      _createDate = createDate;
      _lastModifiedDate = lastModifiedDate;
      _preferences = preferences;
      _success = true;
    }

    public ShopperPrefGetResponseData(AtlantisException exAtlantis)
    {
      _exception = exAtlantis;
    }

    public ShopperPrefGetResponseData(RequestData oRequestData, Exception ex)
    {
      string message = ex.Message + ex.StackTrace;
      string data = "sid=" + oRequestData.ShopperID;
      _exception = new AtlantisException(oRequestData, "ShopperPrefGetResponseData", message, data, ex);
    }

    public Dictionary<string, string> Preferences
    {
      get
      {
        return _preferences;
      }
    }

    #region IResponseData Members

    public string ToXML()
    {
      StringBuilder sbResult = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbResult));

      xtwRequest.WriteStartElement("response");
      xtwRequest.WriteAttributeString("shopper_id", _shopperId);

      if (_preferences != null)
      {
        foreach (KeyValuePair<string, string> pref in _preferences)
        {
          xtwRequest.WriteAttributeString(pref.Key, pref.Value);
        }
      }

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
