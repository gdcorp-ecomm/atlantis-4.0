using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MarketingPublication.Interface
{
  public class MktgGetShopperPreferencesResponseData:IResponseData
  {
    private AtlantisException _exception = null;
    private bool _isSuccess = false;
    private string _responseXml = string.Empty;
    public Dictionary<int, PreferenceInfo> CommunicationPreferences = new Dictionary<int, PreferenceInfo>();
    public Dictionary<int,PreferenceInfo> InterestPreferences = new Dictionary<int,PreferenceInfo>();

    public MktgGetShopperPreferencesResponseData(string responseXml)
    {
      _responseXml = responseXml;
      _isSuccess = ParseResponse();
    }

    private bool ParseResponse()
    {
      bool result = false;
      try
      {
        if (!string.IsNullOrEmpty(_responseXml))
        {
          XmlDocument _xmlDoc = new XmlDocument();
          _xmlDoc.LoadXml(_responseXml);
          if (_xmlDoc.HasChildNodes)
          {
            if (_xmlDoc.FirstChild.HasChildNodes)
            {
              result = true;              
              foreach (XmlNode preferences in _xmlDoc.FirstChild.ChildNodes)
              {
                PreferenceInfo currentPreference = new PreferenceInfo();
                System.Diagnostics.Debug.WriteLine(preferences.Name);
                foreach (XmlAttribute currentAttribute in preferences.Attributes)
                {
                  currentPreference[currentAttribute.Name] = currentAttribute.Value;
                }
                if (currentPreference.InterestTypeID == -1)
                {
                  CommunicationPreferences.Add(currentPreference.CommTypeID, currentPreference);
                }
                else
                {
                  InterestPreferences.Add(currentPreference.InterestTypeID, currentPreference);
                }
              }
            }
          }
        }
      }
      catch (System.Exception ex)
      {
        result = false;
        _exception = new AtlantisException("ParseResults", string.Empty, "0", "ErrorParsingResults", ex.Message + Environment.NewLine + ex.StackTrace, string.Empty, string.Empty, string.Empty, string.Empty, 0);
      }
      return result;
    }

    public MktgGetShopperPreferencesResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public MktgGetShopperPreferencesResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, "MktgGetShopperPreferencesResponseData", ex.Message, ex.StackTrace);
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
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
