using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.PaymentProfileClass.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.KiwiLogger.Interface;

namespace Atlantis.Framework.EcommPaymentProfile.Interface
{
  public class EcommPaymentProfileResponseData : IResponseData
  {
    private AtlantisException _exception;
    private readonly string _responseXml;
    private readonly RequestData _request;
    private PaymentProfile _profile = new PaymentProfile();
    private const int _ENGINE_REQUEST_KIWILOGGER = 53;

    public bool IsSuccess { get; private set; }

    public EcommPaymentProfileResponseData(RequestData request, string responseXml)
    {
      IsSuccess = true;
      _request = request;
      _responseXml = responseXml;
      PopulateProfile();      
    }

    public EcommPaymentProfileResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
    }

    public EcommPaymentProfileResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(oRequestData, oRequestData.GetType().ToString(), ex.Message, ex.StackTrace, ex);
    }

    #region Access Payment Profile

    public PaymentProfile AccessProfile(string shopperId, string managerUserId, string managerName, string sourceFunction)
    {
      PaymentProfile profile = null;
      string user = string.IsNullOrEmpty(managerUserId) ? shopperId : string.Format("{0}-{1}", managerUserId, managerName);

      try
      {
        profile = _profile;

        var kiwiRequest = new KiwiLoggerRequestData(shopperId,
                                                    _request.SourceURL,
                                                    _request.OrderID,
                                                    _request.Pathway,
                                                    _request.PageCount);

        var logParameters = new List<KiwiLoggerParameters>
                              {
                                new KiwiLoggerParameters("profile", profile.ProfileID),
                                new KiwiLoggerParameters("user", user),
                                new KiwiLoggerParameters("origin", string.Format("{0}/{1}", Environment.MachineName, sourceFunction))
                              };

        kiwiRequest.MessagePrefix = "ACCESS(masked) success";
        kiwiRequest.MessageSuffix = string.Empty;
        kiwiRequest.AddItems(logParameters);

        Engine.Engine.ProcessRequest(kiwiRequest, _ENGINE_REQUEST_KIWILOGGER);
      }
      catch (Exception ex)
      {
        _exception = new AtlantisException(_request
          , "EcommPaymentProfileResponseData::AccessProfile"
          , ex.Message
          , _request.ToXML());
      }

      return profile;
    }
    #endregion

    private void PopulateProfile()
    {
      try
      {
        var oDoc = new XmlDocument();
        oDoc.LoadXml(_responseXml);

        XmlNodeList dataNodes = oDoc.SelectNodes("./profiles/profile");

        if (dataNodes == null)
          return;

        foreach (XmlNode dataNode in dataNodes)
        {
          var dataElement = dataNode as XmlElement;
          if (dataElement != null)
          {
            var profile = new PaymentProfile();
            foreach (XmlAttribute currentAtt in dataElement.Attributes)
            {
              profile[currentAtt.Name] = currentAtt.Value;
            }
            _profile = profile;
            break;
          }
        }
      }
      catch (Exception ex)
      {
        _exception = new AtlantisException(_request
          , "EcommPaymentProfileResponseData::Processing Profile"
          , ex.Message
          , _request.ToXML());
        IsSuccess = false;
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
