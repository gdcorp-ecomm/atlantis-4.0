using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetMobileToken.Interface
{
  public class GetMobileTokenRequestData : RequestData
  {
    private string _userName = string.Empty;
    private string _password = string.Empty;
    private int _privateLableId = -1;
    private string _deviceGUID = string.Empty;

    public string UserName
    {
      get { return _userName; }
      set { _userName = value; }
    }
    public string Password
    {
      get { return _password; }
      set { _password = value; }
    }
    public int PrivateLabelId
    {
      get { return _privateLableId; }
      set { _privateLableId = value; }
    }
    public string DeviceGUID
    {
      get { return _deviceGUID; }
      set { _deviceGUID = value; }
    }

    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(5);
    
    public GetMobileTokenRequestData(string sShopperID,
                  string sSourceURL,
                  string sOrderID,
                  string sPathway,
                  int iPageCount, 
                  string userName, 
                  string password,
                  int privateLabelId,
                  string deviceGUID)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      RequestTimeout = _requestTimeout;
      _userName = userName;
      _password = password;
      _privateLableId = privateLabelId;
      _deviceGUID = deviceGUID;
    }

    public XmlNode GetRequestXML()
    {
      StringBuilder sbResult = new StringBuilder();
      XmlTextWriter writer = new XmlTextWriter(new StringWriter(sbResult));
      
      writer.WriteStartElement("Credentials");
      writer.WriteAttributeString("Username", _userName ?? string.Empty );
      writer.WriteAttributeString("Password", _password ?? string.Empty);
      writer.WriteAttributeString("PLID", _privateLableId.ToString());
      writer.WriteAttributeString("DeviceID", _deviceGUID ?? string.Empty );
      writer.WriteEndElement();
      
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(sbResult.ToString());

      return doc;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("GetMobileToken is not a chacheable request.");
    }
  }
}
