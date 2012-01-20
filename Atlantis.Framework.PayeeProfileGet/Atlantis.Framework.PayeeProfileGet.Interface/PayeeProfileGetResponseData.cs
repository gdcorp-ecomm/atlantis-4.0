using System;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PayeeProfileClass.Interface;

namespace Atlantis.Framework.PayeeProfileGet.Interface
{
  public class PayeeProfileGetResponseData : IResponseData
  {
    #region Properties

    private AtlantisException _exception = null;
    private string _responseXml;
    private PayeeProfile _profile = new PayeeProfile();
    bool _isSuccess = false;

    public PayeeProfile Profile
    {
      get { return _profile; }
    }

    public bool IsSuccess
    {
      get { return _exception == null; }
    }
    #endregion

    public PayeeProfileGetResponseData(string responseXml)
    {
      _responseXml = responseXml;
      PopulateProfile();
    }

    public PayeeProfileGetResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
    }

    public PayeeProfileGetResponseData(string responseXml, RequestData requestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(requestData, requestData.GetType().ToString(), ex.Message, ex.StackTrace, ex);
    }

    #region Payee Profile Population

    private void PopulateProfile()
    {
      XmlDocument xDoc = new XmlDocument();
      xDoc.LoadXml(_responseXml);

      XmlNodeList dataNodes = xDoc.SelectNodes("//ACCOUNT");
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
    #endregion

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
