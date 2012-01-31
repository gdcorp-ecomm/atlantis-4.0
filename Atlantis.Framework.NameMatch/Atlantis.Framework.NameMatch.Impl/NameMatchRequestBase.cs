using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.NameMatch.Impl.ModelExt;
using Atlantis.Framework.NameMatch.Interface;
using Service = Atlantis.Framework.NameMatch.Impl.NameMatchService;

namespace Atlantis.Framework.NameMatch.Impl
{
  public class NameMatchRequestBase
  {
    public string GetRequestXML(Interface.NameMatchRequestData reqData)
    {
      StringBuilder reqXml = new StringBuilder("<gdavailabledomains ");

      reqXml.Append("key=\"" + reqData.SearchKey + "\"");
      reqXml.Append(" tlds=\"" + GetTLDs(reqData.TLDs) + "\"");
      reqXml.Append(" limit=\"" + reqData.MaxResults.ToString() + "\"");
      reqXml.Append(" removeKeys=\"" + reqData.RemoveKeys + "\"");
      reqXml.Append(" filters=\"" + reqData.Filters + "\"");
      reqXml.Append(" targets=\"" + reqData.Targets + "\"");
      reqXml.Append(" supportedLanguages=\"" + reqData.SupportedLang + "\"");
      reqXml.Append(" adddashes=\"" + reqData.AddDashes + "\"");
      reqXml.Append(" addrelated=\"" + reqData.AddRelated + "\"");
      reqXml.Append(" prefixes=\"" + reqData.Prefixes + "\"");
      reqXml.Append(" suffixes=\"" + reqData.Suffixes + "\"");
      reqXml.Append(" shopperid=\"" + reqData.ShopperID + "\"");
      reqXml.Append(" shopperauth=\"" + reqData.ShopperAuth + "\"");
      reqXml.Append(" availabledomainsonly=\"" + reqData.AvailableDomainsOnly + "\"");
      reqXml.Append(" pagesequence=\"" + reqData.PageCount.ToString() + "\"");
      reqXml.Append(" requestingserver=\"" + reqData.RequestingServer + "\"");
      reqXml.Append(" customerip=\"" + reqData.CustomerIp + "\"");
      reqXml.Append(" privatelabel=\"" + reqData.PrivateLabelId + "\"");
      reqXml.Append(" sourcecode=\"" + reqData.SourceCode + "\"");
      reqXml.Append(" visitingid=\"" + reqData.Pathway + "\"");
      reqXml.Append(" searchtimeoutmilliseconds=\"" + reqData.RequestTimeout.TotalMilliseconds.ToString() + "\"");
      reqXml.Append(" version=\"" + reqData.ServiceVersion + "\"");
      reqXml.Append(" />");

      return reqXml.ToString();
    }

    internal List<KeyValuePair<string, string>> GetData(Interface.DomainData[] domainData)
    {
      List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();

      foreach (DomainData item in domainData)
      {
        if (item.Data != null)
        {
          data.Add(new KeyValuePair<string, string>(item.Name, item.Data.ToString()));
        }
      }

      return data;
    }

    internal string GetTLDs(System.Collections.Generic.List<string> list)
    {
      return String.Join(", ", list.ToArray());
    }

    internal AvailableDomain[] GetAvailableDomains(Service.AvailableDomain[] availableDomain, RequestData rqData)
    {
      AvailableDomain[] arrayOfDomains = new AvailableDomain[0];

      if (availableDomain != null && availableDomain.Length > 0)
      {
        arrayOfDomains = new AvailableDomain[availableDomain.Length];

        for (int i = 0; i < availableDomain.Length; i++)
        {
          arrayOfDomains[i] = availableDomain[i].ConvertAvailableDomain();
        }
      }

      return arrayOfDomains;
    }
  }
}
