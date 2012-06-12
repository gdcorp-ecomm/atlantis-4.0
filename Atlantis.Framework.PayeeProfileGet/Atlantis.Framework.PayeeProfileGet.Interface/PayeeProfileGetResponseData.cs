using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PayeeProfileClass.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Atlantis.Framework.PayeeProfileGet.Interface
{
  public class PayeeProfileGetResponseData : IResponseData
  {
    #region Properties

    private AtlantisException _exception = null;
    private string _responseXml;
    private PayeeProfile _profile = new PayeeProfile();

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
      PayeeProfile profile = new PayeeProfile();

      XDocument xDoc = XDocument.Parse(_responseXml);
      XElement account = xDoc.Element("RESPONSE").Element("ACCOUNT");
      XElement ach = xDoc.Element("RESPONSE").Element("ACCOUNT").Element("ACH");
      XElement paypal = xDoc.Element("RESPONSE").Element("ACCOUNT").Element("PayPal");
      XElement gag = xDoc.Element("RESPONSE").Element("ACCOUNT").Element("GAG");
      List<XElement> addresses = xDoc.Element("RESPONSE").Element("ACCOUNT").Elements("ADDRESS").ToList<XElement>();

      if (account != null)
      {
        foreach (XAttribute attr in account.Attributes())
        {
          profile[attr.Name.ToString().ToLowerInvariant()] = attr.Value;
        }
      }

      if (ach != null)
      {
        PayeeProfile.ACHClass achProfile = new PayeeProfile.ACHClass();
        foreach (XAttribute attr in ach.Attributes())
        {
          achProfile[attr.Name.ToString().ToLowerInvariant()] = attr.Value;
        }
        profile.ACH = achProfile;
      }

      if (paypal != null)
      {
        PayeeProfile.PayPalClass paypalProfile = new PayeeProfile.PayPalClass();
        foreach (XAttribute attr in paypal.Attributes())
        {
          paypalProfile[attr.Name.ToString().ToLowerInvariant()] = attr.Value;
        }
        profile.PayPal = paypalProfile;
      }

      if (gag != null)
      {
        PayeeProfile.GAGClass gagProfile = new PayeeProfile.GAGClass();
        foreach (XAttribute attr in gag.Attributes())
        {
          gagProfile[attr.Name.ToString().ToLowerInvariant()] = attr.Value;
        }
        profile.GAG = gagProfile;
      }

      if (addresses != null)
      {
        foreach (XElement address in addresses)
        {
          PayeeProfile.AddressClass addressProfile = new PayeeProfile.AddressClass();
          foreach (XAttribute attr in address.Attributes())
          {
            addressProfile[attr.Name.ToString().ToLowerInvariant()] = attr.Value;
          }
          profile.Address.Add(addressProfile);
        }
      }

      _profile = profile;
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
