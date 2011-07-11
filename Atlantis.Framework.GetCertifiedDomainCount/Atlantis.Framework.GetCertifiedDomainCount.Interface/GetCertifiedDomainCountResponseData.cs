using System;
using System.IO;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.GetCertifiedDomainCount.Interface
{
  public class GetCertifiedDomainCountResponseData : IResponseData, ISessionSerializableResponse
  {
    #region Properties

    private AtlantisException _exception = null;
    private int _certifiedDomainCount = 0;

    private bool _success = false;
    public bool IsSuccess
    {
      get { return _success; }
    }

    public int CertifiedDomainsCount
    {
      get { return _certifiedDomainCount; }
    }
    #endregion

    public GetCertifiedDomainCountResponseData()
    { }

    public GetCertifiedDomainCountResponseData(int certifiedDomains)
    {
      _certifiedDomainCount = certifiedDomains;
      _success = true;
    }

    public GetCertifiedDomainCountResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public GetCertifiedDomainCountResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                   "GetCertifiedDomainCountResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      return string.Format("<CertifiedDomainsCount>{0}</CertifiedDomainsCount>", _certifiedDomainCount);
    }


    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

    #region ISessionSerializableResponse Members

    public string SerializeSessionData()
    {
      return ToXML();
    }

    public void DeserializeSessionData(string sessionData)
    {
      if (!string.IsNullOrEmpty(sessionData))
      {
        using (XmlReader reader = XmlReader.Create(new StringReader(sessionData)))
        {
          if (reader.Read())
          {
            reader.ReadStartElement("CertifiedDomainsCount");
            if (int.TryParse(reader.Value, out _certifiedDomainCount))
            {
              _success = true;
            }
          }
        }
      }
    }
    #endregion
  }
}
