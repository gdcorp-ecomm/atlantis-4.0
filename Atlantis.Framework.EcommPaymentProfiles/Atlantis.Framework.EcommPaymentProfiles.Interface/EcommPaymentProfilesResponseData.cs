using System;
using System.Collections.Generic;
using System.Xml;

using Atlantis.Framework.Interface;
using Atlantis.Framework.KiwiLogger.Interface;

namespace Atlantis.Framework.EcommPaymentProfiles.Interface
{
  public class EcommPaymentProfilesResponseData : IResponseData
  {
    #region Properties
    private AtlantisException _exception;
    private string _responseXml = string.Empty;
    private List<PaymentProfile> _profiles = new List<PaymentProfile>();
    private RequestData _request;
    public int PaymentProfileCount { get; private set; }

    public bool IsSuccess { get; private set; }

    #endregion

    public EcommPaymentProfilesResponseData(RequestData request, string responseXml)
    {
      _request = request;
      _responseXml = responseXml;
      PopulateProfiles();
      PaymentProfileCount = _profiles.Count;
      IsSuccess = true;
    }

    public EcommPaymentProfilesResponseData(string responseXml, AtlantisException atlantisException)
    {
      _responseXml = responseXml;
      _exception = atlantisException;
    }

    public EcommPaymentProfilesResponseData(string responseXml, RequestData requestData, Exception exception)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(requestData
        , "EcommPaymentProfilesResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region Populate Profiles
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
    #endregion

    #region Access Payment Profile

    /// <summary>
    /// This function only accesses by index because you're using a triplet that provides a list of profiles.  If you need a specific profile, call EcommPaymentProfile
    /// </summary>
    public PaymentProfile AccessProfile(int index, string shopperId, string managerUserId, string managerName, string sourceFunction)
    {
      PaymentProfile profile = null;
      string user = string.IsNullOrEmpty(managerUserId) ? shopperId : string.Format("{0}-{1}", managerUserId, managerName);

      try
      {
        profile = _profiles[index];

        KiwiLoggerRequestData kiwiRequest = new KiwiLoggerRequestData(shopperId
          , _request.SourceURL
          , _request.OrderID
          , _request.Pathway
          , _request.PageCount);

        List<KiwiLoggerParameters> logParameters = new List<KiwiLoggerParameters>();
        logParameters.Add(new KiwiLoggerParameters("profile", profile.ProfileID));
        logParameters.Add(new KiwiLoggerParameters("user", user));
        logParameters.Add(new KiwiLoggerParameters("origin", string.Format("{0}/{1}", Environment.MachineName, sourceFunction)));

        kiwiRequest.MessagePrefix = "ACCESS(masked) success";
        kiwiRequest.MessageSuffix = string.Empty;
        kiwiRequest.AddItems(logParameters);

        KiwiLoggerResponseData kiwiResponse = (KiwiLoggerResponseData)Engine.Engine.ProcessRequest(kiwiRequest, EngineRequests.KiwiLogger);
      }
      catch (Exception ex)
      {
        _exception = new AtlantisException(_request
          , "EcommPaymentProfilesResponseData::AccessProfile"
          , ex.Message
          , _request.ToXML());
      }

      return profile;
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
