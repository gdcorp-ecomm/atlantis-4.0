using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.VerifyMobileToken.Interface
{
  public class VerifyMobileTokenRequestData : RequestData
  {
    private string _userName = string.Empty;
    private string _deviceGUID = string.Empty;
    private string _sessionToken = string.Empty;

    public string UserName
    {
      get { return _userName; }
      set { _userName = value; }
    }
    public string DeviceGUID
    {
      get { return _deviceGUID; }
      set { _deviceGUID = value; }
    }
    public string SessionToken
    {
      get { return _sessionToken; }
      set { _sessionToken = value; }
    }
    
    #region NimtizProperties

    private string _dsn = string.Empty;
    private string _appName = string.Empty;
    private string _certName = string.Empty;

    public string DataSourceName
    {
      get { return _dsn; }
      set { _dsn = value; }
    }

    public string ApplicationName
    {
      get { return _appName; }
      set { _appName = value; }
    }

    public string CertificateName
    {
      get { return _certName; }
      set { _certName = value; }
    }

    #endregion

    public VerifyMobileTokenRequestData(string sShopperID,
                  string sSourceURL,
                  string sOrderID,
                  string sPathway,
                  int iPageCount, 
                  string userName, 
                  string deviceGUID,
                  string sessionToken)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      _userName = userName;
      _deviceGUID = deviceGUID;
      _sessionToken = sessionToken;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public XmlNode GetRequestXML()
    {
      StringBuilder sbResult = new StringBuilder();
      XmlTextWriter writer = new XmlTextWriter(new StringWriter(sbResult));
      
      writer.WriteStartElement("Credentials");
      writer.WriteAttributeString("Username", _userName ?? string.Empty );
      writer.WriteAttributeString("Validate", "1");
      writer.WriteAttributeString("DeviceID", _deviceGUID ?? string.Empty );
      writer.WriteAttributeString("SessionToken", _sessionToken ?? string.Empty);
      writer.WriteEndElement();
      
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(sbResult.ToString());

      return doc;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("VerifyMobileToken is not a chacheable request.");
    }
  }
}
