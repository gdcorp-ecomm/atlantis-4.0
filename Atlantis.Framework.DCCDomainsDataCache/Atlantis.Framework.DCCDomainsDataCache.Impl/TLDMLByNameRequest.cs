using Atlantis.Framework.DCCDomainsDataCache.Interface;
using Atlantis.Framework.Interface;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.DCCDomainsDataCache.Impl
{
  public class TLDMLByNameRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      const string _DATAVALUEELEMENTNAME = "datavalue";
      const string _TLDMLROOTELEMENTNAME = "tldmldoc";

      IResponseData result = null;

      try
      {
        string tld = ((TLDMLByNameRequestData)requestData).TLD;
        if (string.IsNullOrEmpty(tld))
        {
          throw new ArgumentException("TLD cannot be empty or null.");
        }

        string responseXml;

        using (var domainDataCache = new DomainsDataCacheWS.DomainsDataCache())
        {
          domainDataCache.Url = ((WsConfigElement)config).WSURL;
          domainDataCache.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
          responseXml = domainDataCache.GetTldmlByTldName(tld);
        }

        XDocument tldmlDoc = XDocument.Parse(responseXml);
        XElement tldmlElement = tldmlDoc.Root;
        if (tldmlElement.Name.ToString() != _TLDMLROOTELEMENTNAME)
        {
          tldmlElement = tldmlDoc.Descendants(_TLDMLROOTELEMENTNAME).FirstOrDefault();
        }

        if (tldmlElement != null)
        {
          XAttribute resultsAttribute = tldmlElement.Attribute("result");
          if (resultsAttribute.Value.Equals("success", StringComparison.OrdinalIgnoreCase))
          {
            XElement dataValueElement = tldmlDoc.Descendants(_DATAVALUEELEMENTNAME).FirstOrDefault();
            if (dataValueElement != null)
            {
              XAttribute valueAttribute = dataValueElement.Attribute("value");
              if (valueAttribute != null)
              {
                XDocument tldmlValue = XDocument.Parse(valueAttribute.Value);
                result = TLDMLByNameResponseData.FromXDocument(tldmlValue);
              }
            }
          }
        }

        if (result == null)
        {
          throw new ApplicationException("TLDML failure: " + responseXml);
        }
      }
      catch (Exception ex)
      {
        result = TLDMLByNameResponseData.FromException(requestData, ex);

      }

      return result;
    }
  }
}
