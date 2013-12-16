using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.DomainsRAA.Interface.Items;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsRAA.Interface.DomainsRAAStatus
{
  public class DomainsRAAStatusResponseData : IResponseData
  {
    private readonly string _responseXml = string.Empty;

    private DomainsRAAStatusResponseData(AtlantisException exception)
    {
      _responseXml = string.Empty;

      if (!HasErrorCodes)
      {
        _errorCodes = new List<DomainsRAAErrorCodes>(1)
        {
          DomainsRAAErrorCodes.Exception
        };
      }

      Engine.Engine.LogAtlantisException(exception);
    }

    private DomainsRAAStatusResponseData(string responseXml)
    {
      _responseXml = responseXml;

      var response = XElement.Parse(responseXml);

      var errorsElement = response.Element("errors");
      SetErrorCodes(errorsElement);

      var itemsElement = response.Element("items");
      SetVerifyCodes(itemsElement);
    }

    private string GetAttributeValue(string attributeName, XElement element)
    {
      var value = string.Empty;

      if (element != null && element.Attribute(attributeName) != null)
      {
        value = element.Attribute(attributeName).Value;
      }

      return value;
    }

    private void SetVerifyCodes(XElement itemsElement)
    {
      if (itemsElement != null)
      {
        _verifiedResponseItems = new List<VerifiedResponseItem>(itemsElement.Elements("item").Count());

        foreach (var itemElement in itemsElement.Elements("item"))
        {
          var type = GetAttributeValue("type", itemElement);
          var value = GetAttributeValue("value", itemElement);
          var verified = GetAttributeValue("verified", itemElement);
          var validateGuid = GetAttributeValue("validationguid", itemElement);

          DomainsRAAVerifyCode raaVerifyCode;
          if (!Enum.TryParse(verified, out raaVerifyCode))
          {
            raaVerifyCode = DomainsRAAVerifyCode.None;
          }

          var verifyItem = VerifiedResponseItem.Create(type, value, raaVerifyCode, validateGuid);
          _verifiedResponseItems.Add(verifyItem);
        }
      }
    }

    private void SetErrorCodes(XContainer errorsElement)
    {
      if (errorsElement != null)
      {
        _errorCodes = new List<DomainsRAAErrorCodes>(errorsElement.Elements("error").Count());

        foreach (var error in errorsElement.Elements("error"))
        {
          DomainsRAAErrorCodes raaErrorCode;

          if (Enum.TryParse(error.Attribute("code").Value, out raaErrorCode))
          {
            _errorCodes.Add(raaErrorCode);
          }
        }
      }
    }
    
    private IList<VerifiedResponseItem> _verifiedResponseItems;
    public IEnumerable<VerifiedResponseItem> VerifiedResponseItems
    {
      get
      {
        if (_verifiedResponseItems == null)
        {
          _verifiedResponseItems = new List<VerifiedResponseItem>(8);
        }

        return _verifiedResponseItems;
      }
    }

    public bool HasVerifiedResponseCodes
    {
      get
      {
        return _verifiedResponseItems != null && _verifiedResponseItems.Count > 0;
      }
    }

    public bool HasErrorCodes
    {
      get
      {
        return _errorCodes != null && _errorCodes.Count > 0;
      }
    }
    
    private IList<DomainsRAAErrorCodes> _errorCodes;
    public IEnumerable<DomainsRAAErrorCodes> ErrorCodes
    {
      get
      {
        if (_errorCodes == null)
        {
          _errorCodes = new List<DomainsRAAErrorCodes>(8);
        }

        return _errorCodes;
      }
    }

    public static DomainsRAAStatusResponseData FromData(string responseXml)
    {
      return new DomainsRAAStatusResponseData(responseXml);
    }

    public static DomainsRAAStatusResponseData FromData(AtlantisException exception)
    {
      return new DomainsRAAStatusResponseData(exception);
    }

    public string ToXML()
    {
      return _responseXml;
    }

    public AtlantisException GetException()
    {
      return null;
    }
  }
}
