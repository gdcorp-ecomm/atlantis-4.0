using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Support.Interface
{
  public class SupportPhoneResponseData : IResponseData
  {
    private static readonly ISupportPhoneData _emptySupportPhoneData = new SupportPhoneData(string.Empty, false);

    private readonly AtlantisException _exception;

    public bool TryGetSupportData(string country, out ISupportPhoneData supportPhoneData)
    {
      bool result = false;
      supportPhoneData = _emptySupportPhoneData;

      if (!string.IsNullOrEmpty(country))
      {
        if (_countriesSupportData.TryGetValue(country, out supportPhoneData))
        {
          result = true;
        }
      }

      return result;
    }

    private SupportPhoneResponseData(XElement responseXml)
    {
      ParseResponseXml(responseXml);
    }

    private SupportPhoneResponseData(AtlantisException ex)
    {
      _exception = ex;
    }

    public static SupportPhoneResponseData FromException(AtlantisException ex)
    {
      return new SupportPhoneResponseData(ex);
    }

    public static SupportPhoneResponseData FromResponseXml(XElement responseXml)
    {
      return new SupportPhoneResponseData(responseXml);
    }

    public string ToXML()
    {
      string result = "<exception/>";
      if (_xmlData != null)
      {
        result = _xmlData;
      }
      return result;
    }

    private Dictionary<string, ISupportPhoneData> _countriesSupportData;
    private string _xmlData;

    private void ParseResponseXml(XElement responseXml)
    {
      _xmlData = responseXml.ToString();
      _countriesSupportData = new Dictionary<string, ISupportPhoneData>(StringComparer.OrdinalIgnoreCase);

      foreach (XElement itemElement in responseXml.Descendants("item"))
      {
        try
        {
          var isActive = itemElement.Attribute("isActive").Value;
          if (!string.IsNullOrEmpty(isActive) && isActive.Equals("1"))
          {
            var country = itemElement.Attribute("flagCode").Value;
            var phoneNumber = itemElement.Attribute("supportPhone").Value;
            _countriesSupportData[country] = new SupportPhoneData(phoneNumber, !country.Equals("us", StringComparison.OrdinalIgnoreCase));
          }
        }
        catch (Exception ex)
        {
          string message = ex.Message + ex.StackTrace;
          var aex = new AtlantisException("SupportPhoneResponseData.ctor", "0", message, itemElement.ToString(), null, null);
          Engine.Engine.LogAtlantisException(aex);
        }
      }
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
