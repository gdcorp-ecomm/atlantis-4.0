using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPaymentProfileUnique.Interface
{
  public class EcommPaymentProfileUniqueResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private bool _isUniquePaymentProfile = false;
    private bool _usesNonUniquePaymentType = false;
    private string _errorXmlString;

    public bool IsSuccess
    {
        get
        {
            return _exception == null && String.IsNullOrEmpty(_errorXmlString);
        }
    }

      public bool UsesNonUniquePaymentType
      {
          get { return _usesNonUniquePaymentType; }
      }

      public bool IsUniquePaymentProfile
      {
          get { return _isUniquePaymentProfile; }
      }

      public EcommPaymentProfileUniqueResponseData(bool isUniquePaymentProfile, bool bUsesNonUniquePaymentType, string sErrorXmlstring)
    {
        _isUniquePaymentProfile = isUniquePaymentProfile;
        _usesNonUniquePaymentType = bUsesNonUniquePaymentType;
        _errorXmlString = sErrorXmlstring;
    }

     public EcommPaymentProfileUniqueResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public EcommPaymentProfileUniqueResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "EcommPaymentProfileUniqueResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
         StringBuilder sbResult = new StringBuilder();
        XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbResult));

        xtwRequest.WriteStartElement("response");
        xtwRequest.WriteAttributeString("isUniquePaymentProfile", _isUniquePaymentProfile.ToString());
        xtwRequest.WriteAttributeString("usesNonUniquePaymentType", _usesNonUniquePaymentType.ToString());
        xtwRequest.WriteAttributeString("errorXmlString", _errorXmlString.ToString());
        xtwRequest.WriteEndElement();

        return sbResult.ToString();
     
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
