using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RegCAValidateProfileData.Interface
{
  public class RegCAValidateProfileDataRequestData : RequestData
  {
    private string _ciraProfileId;
    private string _ciraRegistrantName;

    public RegCAValidateProfileDataRequestData(
      string shopperID,
      string sourceURL,
      string orderID,
      string pathway,
      int pageCount)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(4);
    }

    public RegCAValidateProfileDataRequestData(
      string shopperID,
      string sourceURL,
      string orderID,
      string pathway,
      int pageCount,
      string ciraRegistrantName,
      string ciraProfileId)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      _ciraRegistrantName = ciraRegistrantName;
      _ciraProfileId = ciraProfileId;

      RequestTimeout = TimeSpan.FromSeconds(4);
    }

    #region RequestData Members

    public override string GetCacheMD5()
    {
      return string.Empty;
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("profile");
      xtwRequest.WriteAttributeString("caprofileid", _ciraProfileId);
      xtwRequest.WriteAttributeString("caregistrantname", _ciraRegistrantName);
      xtwRequest.WriteEndElement();

      return sbRequest.ToString();
    }

    #endregion
  }
}
