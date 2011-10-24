using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetPaymentProfiles.Interface
{
  public class GetPaymentProfilesResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _responseXml;
    private List<PaymentProfile> _profiles = new List<PaymentProfile>();
    bool _isSuccess = false;
    
    public GetPaymentProfilesResponseData(string responseXml)
    {
      _responseXml = responseXml;
      PopulateProfiles();
      _isSuccess = true;
    }

    public GetPaymentProfilesResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
    }

    public GetPaymentProfilesResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(oRequestData, oRequestData.GetType().ToString(), ex.Message, ex.StackTrace, ex);
    }

    public List<PaymentProfile> Profiles
    {
      get
      {
        return _profiles;
      }
    }

    private void PopulateProfiles()
    {
      XmlDocument oDoc = new XmlDocument();
      oDoc.LoadXml(_responseXml);

      XmlNodeList dataNodes = oDoc.SelectNodes("./profiles/profile");
      foreach (XmlNode dataNode in dataNodes)
      {
        XmlElement dataElement = dataNode as XmlElement;
        if (dataElement != null)
        {
          PaymentProfile profile = new PaymentProfile();
          foreach (XmlAttribute currentAtt in dataElement.Attributes)
          {
            profile[currentAtt.Name] = currentAtt.Value;
          }
          _profiles.Add(profile);
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
