using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorGetPhones.Interface
{
  public class AuthTwoFactorGetPhonesResponseData : IResponseData
  {
    private AtlantisException _aex;
    private string _xml;

    public long StatusCode { get; private set; }

    public HashSet<int> ValidationCodes { get; private set; }

    public string StatusMessage { get; private set; }

    public IList<AuthTwoFactorPhone> Phones { get; private set; }

    private AuthTwoFactorPhone _primaryPhone;
    public AuthTwoFactorPhone PrimaryPhone
    {
      get
      {
        if(_primaryPhone == null)
        {
          foreach (AuthTwoFactorPhone authTwoFactorPhone in Phones)
          {
            if(authTwoFactorPhone.StatusCode == AuthTwoFactorPhoneStatusCodes.Active)
            {
              _primaryPhone = authTwoFactorPhone;
              break;
            }
          }
        }
        return _primaryPhone;
      }
    }

    public AuthTwoFactorGetPhonesResponseData(long statusCode, HashSet<int> validationCodes, string phonesXml, string statusMessage)
    {
      ValidationCodes = validationCodes;
      StatusMessage = statusMessage;
      StatusCode = statusCode;
      _xml = phonesXml;
      Phones = ParsePhonesFromXml(phonesXml);
    }

    public AuthTwoFactorGetPhonesResponseData(Exception ex, RequestData request)
    {
      _aex = new AtlantisException(request, "AuthTwoFactorGetPhonesResponseData", ex.Message, string.Empty, ex);
    }

    private static IList<AuthTwoFactorPhone> ParsePhonesFromXml(string phonesXml)
    {
      IList<AuthTwoFactorPhone> phoneList = new List<AuthTwoFactorPhone>(0);

      if(!string.IsNullOrEmpty(phonesXml))
      {
        try
        {
          XmlDocument phonesXmlDoc = new XmlDocument();
          phonesXmlDoc.LoadXml(phonesXml);

          XmlNodeList phoneNodes = phonesXmlDoc.SelectNodes("//Phone");

          if (phoneNodes != null)
          {
            phoneList = new List<AuthTwoFactorPhone>(phoneNodes.Count);

            foreach (XmlNode phoneNode in phoneNodes)
            {
              AuthTwoFactorPhone phone = new AuthTwoFactorPhone(phoneNode.OuterXml);
              if(!string.IsNullOrEmpty(phone.PhoneNumber) &&
                 !string.IsNullOrEmpty(phone.CarrierId))
              {
                if (phone.StatusCode > -1)
                {
                  phoneList.Add(phone);
                }
              }
            }
          }
        }
        catch
        {
          phoneList = new List<AuthTwoFactorPhone>(0);
        }
      }

      return phoneList;
    }

    public string ToXML()
    {
      return _xml;
    }

    public AtlantisException GetException()
    {
      return _aex;
    }
  }
}
