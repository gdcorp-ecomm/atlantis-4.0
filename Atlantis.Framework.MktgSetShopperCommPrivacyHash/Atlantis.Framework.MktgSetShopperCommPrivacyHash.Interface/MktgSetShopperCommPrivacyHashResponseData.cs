using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MktgSetShopperCommPrivacyHash.Interface
{
  public class MktgSetShopperCommPrivacyHashResponseData : IResponseData
  {
    private bool _success = false;
    private AtlantisException _exception = null;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public string ToXML()
    {
      StringBuilder sbResult = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbResult));

      xtwRequest.WriteStartElement("response");
      xtwRequest.WriteAttributeString("success", _success.ToString());
      xtwRequest.WriteEndElement();

      return sbResult.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public MktgSetShopperCommPrivacyHashResponseData()
    {
      _success = true;
    }

    public MktgSetShopperCommPrivacyHashResponseData(AtlantisException exAtlantis)
    {
      _exception = exAtlantis;
    }

    public MktgSetShopperCommPrivacyHashResponseData(RequestData oRequestData, Exception ex)
    {
      _exception = new AtlantisException(oRequestData, "MktgSetShopperCommPrivacyHashResponseData", ex.Message, string.Empty);
    }

  }
}
