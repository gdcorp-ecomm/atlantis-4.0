using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMGetCustomerSummary.Interface
{
  public class EEMGetCustomerSummaryRequestData : RequestData
  {
    #region Properties
    private List<int> CustomerIds { get; set; }
    private int _eemGetCustomerSummaryRequestType = 458;
    public int EEMGetCustomerSummaryRequestType
    {
      get { return _eemGetCustomerSummaryRequestType; }
      set { _eemGetCustomerSummaryRequestType = value; }
    }
    public string CustomerXml
    {
      get
      {
        XElement customers = new XElement("Customers");
        foreach (int customerId in CustomerIds)
        {
          XElement customer = new XElement("Customer",
            new XElement("customer_id", customerId.ToString()));
          customers.Add(customer);
        }
        return customers.ToString();
      }
    }
    #endregion

    public EEMGetCustomerSummaryRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , List<int> customerIds)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      CustomerIds = customerIds;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in EEMGetCustomerSummaryRequestData");     
    }
  }
}
