using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsRAA.Interface.DomainsRAAResend
{
  public class DomainsRAAResendResponseData : IResponseData
  {
    private readonly string _responseXml = string.Empty;

    private DomainsRAAResendResponseData(AtlantisException exception)
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

    private DomainsRAAResendResponseData(string responseXml)
    {
      _responseXml = responseXml;
      try
      {
        var responseElement = XElement.Parse(_responseXml);
        
        var errorsElement = responseElement.Element("errors");
        if (responseElement.Name == "response")
        {
          IsSuccess = responseElement.Attribute("queuedsuccessfully").Value == "1" && (errorsElement == null || !errorsElement.HasElements);
        }

        if (!IsSuccess && errorsElement != null)
        {
          _errorCodes = new List<DomainsRAAErrorCodes>(errorsElement.Elements("error").Count());

          foreach (var errorElement in errorsElement.Elements("error"))
          {
            DomainsRAAErrorCodes raaErrorCode;

            if (Enum.TryParse(errorElement.Attribute("code").Value, out raaErrorCode))
            {
              _errorCodes.Add(raaErrorCode);
            }
          }
        }
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException("DomainsRAAResendResponseData", "0", ex.ToString(), responseXml, null, null);
        Engine.Engine.LogAtlantisException(aex);
      }

    }

    public bool IsSuccess { get; private set; }
    
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

    public bool HasErrorCodes
    {
      get
      {
        return _errorCodes != null && _errorCodes.Count > 0;
      }
    }

    public static DomainsRAAResendResponseData FromData(string responseXml)
    {
      return new DomainsRAAResendResponseData(responseXml);
    }

    public static DomainsRAAResendResponseData FromData(AtlantisException exception)
    {
      return new DomainsRAAResendResponseData(exception);
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
