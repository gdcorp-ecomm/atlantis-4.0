using System;
using System.Runtime.Serialization;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPrepPayPalExpRedirect.Interface
{
  [DataContract]
  public class EcommPrepPayPalExpRedirectResponseData : IResponseData
  {
    private readonly AtlantisException _exception;

    public EcommPrepPayPalExpRedirectResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public EcommPrepPayPalExpRedirectResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData, exception.Source, exception.Message, exception.StackTrace, exception);
    }

    public EcommPrepPayPalExpRedirectResponseData(string redirectUrl, string errorDescription, string token)
    {
      ErrorDesciption = errorDescription;
      RedirectURL = redirectUrl;
      Token = token;
    }

    [DataMember]
    public string RedirectURL { get; set; }

    [DataMember]
    public string ErrorDesciption { get; set; }

    [DataMember]
    public string Token { get; set; }

    #region Implementation of IResponseData

    public string ToXML()
    {
      string xml;
      try
      {
        var serializer = new DataContractSerializer(GetType());
        using (var backing = new System.IO.StringWriter())
        using (var writer = new XmlTextWriter(backing))
        {
          serializer.WriteObject(writer, this);
          xml = backing.ToString();
        }
      }
      catch (Exception)
      {
        xml = string.Empty;
      }
      return xml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
