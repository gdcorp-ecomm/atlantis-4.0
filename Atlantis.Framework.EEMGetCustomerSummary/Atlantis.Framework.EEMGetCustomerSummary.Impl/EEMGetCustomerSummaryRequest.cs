﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.EEMGetCustomerSummary.Impl.CampaignBlazerWS;
using Atlantis.Framework.EEMGetCustomerSummary.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMGetCustomerSummary.Impl
{
  public class EEMGetCustomerSummaryRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EEMGetCustomerSummaryResponseData responseData = null;
      Dictionary<int, string> replacementDataDictionary = new Dictionary<int, string>();

      try
      {
        var request = (EEMGetCustomerSummaryRequestData)requestData;

        using (CampaignBlazer eemWs = new CampaignBlazer())
        {
          eemWs.Url = ((WsConfigElement)config).WSURL;
          eemWs.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          string xmlData = eemWs.GetCustomerSummary(request.CustomerXml);

          if (!string.IsNullOrWhiteSpace(xmlData))
          {
            XDocument xDoc = new XDocument();
            xDoc = XDocument.Parse(xmlData);

            foreach (XElement customer in xDoc.Element("Customers").Elements("Customer"))
            {
              string name = string.IsNullOrWhiteSpace(customer.Element("company_name").Value.ToString()) ? "New Account" : customer.Element("company_name").Value.ToString();
              replacementDataDictionary.Add(Convert.ToInt32(customer.Element("customer_id").Value), name);
            }
          }
        }
        responseData = new EEMGetCustomerSummaryResponseData(replacementDataDictionary);
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new EEMGetCustomerSummaryResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new EEMGetCustomerSummaryResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
