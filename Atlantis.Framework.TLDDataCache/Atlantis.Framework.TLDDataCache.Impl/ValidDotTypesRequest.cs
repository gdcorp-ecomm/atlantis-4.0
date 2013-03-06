﻿using Atlantis.Framework.Interface;
using Atlantis.Framework.TLDDataCache.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Impl
{
  public class ValidDotTypesRequest : IRequest
  {
    private const string _TLDINFOREQUESTFORMAT = "<GetTLDInfo><param name=\"tldIdOrName\" value=\"{0}\"/></GetTLDInfo>";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        string tld = ((ValidDotTypesRequestData)requestData).TLD;
        if (string.IsNullOrEmpty(tld))
        {
          throw new ArgumentException("TLD cannot be empty or null.");
        }

        string requestXml = string.Format(_TLDINFOREQUESTFORMAT, tld);
        string responseXml = DataCache.DataCache.GetCacheData(requestXml);
        XElement tldElements = XElement.Parse(responseXml);
        result = ValidDotTypesResponseData.FromDataCacheElement(tldElements);
      }
      catch (Exception ex)
      {
        result = ValidDotTypesResponseData.FromException(requestData, ex);
      }

      return result;
    }
  }
}