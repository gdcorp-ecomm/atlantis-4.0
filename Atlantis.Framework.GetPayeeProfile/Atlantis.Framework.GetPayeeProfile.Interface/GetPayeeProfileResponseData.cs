using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetPayeeProfile.Interface
{
  public class GetPayeeProfileResponseData : IResponseData
  {

    private AtlantisException _exception = null;
    private string _responseXml;
    private PayeeProfile _profile = new PayeeProfile();
    bool _isSuccess = false;

    public GetPayeeProfileResponseData(string responseXml)
    {
      _responseXml = responseXml;
      PopulateProfile();
      _isSuccess = true;
    }

    public GetPayeeProfileResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
    }

    public GetPayeeProfileResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(oRequestData, oRequestData.GetType().ToString(), ex.Message, ex.StackTrace, ex);
    }

    public PayeeProfile Profile
    {
      get
      {
        return _profile;
      }
    }

    private void PopulateProfile()
    {
      XmlDocument oDoc = new XmlDocument();
      oDoc.LoadXml(_responseXml);

      XmlNodeList dataNodes = oDoc.SelectNodes("//ACCOUNT");
      foreach (XmlNode dataNode in dataNodes)
      {
        XmlElement dataElement = dataNode as XmlElement;
        if (dataElement != null)
        {
          PayeeProfile profile = new PayeeProfile();
          foreach (XmlAttribute currentAtt in dataElement.Attributes)
          {
            profile[currentAtt.Name.ToLowerInvariant()] = currentAtt.Value;
          }

          XmlNodeList achNodes = dataNode.SelectNodes("//ACH");
          foreach(XmlNode achNode in achNodes)
          {
            XmlElement achElement = achNode as XmlElement;
            PayeeProfile.ACHClass achProfile = new PayeeProfile.ACHClass();
            foreach (XmlAttribute currentAtt in achElement.Attributes)
            {
              achProfile[currentAtt.Name.ToLowerInvariant()] = currentAtt.Value;
            }
            profile.ACH.Add(achProfile);
          }

          XmlNodeList addressNodes = dataNode.SelectNodes("//ADDRESS");
          foreach (XmlNode addressNode in addressNodes)
          {
            XmlElement addressElement = addressNode as XmlElement;
            PayeeProfile.AddressClass addressProfile = new PayeeProfile.AddressClass();
            foreach (XmlAttribute currentAtt in addressElement.Attributes)
            {
              addressProfile[currentAtt.Name.ToLowerInvariant()] = currentAtt.Value;
            }
            profile.Address.Add(addressProfile);
          }

          _profile = profile;
        }
      }
    }

    public bool IsSuccess
    {
      get
      {
        return _isSuccess;
      }
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _responseXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
