using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.LogDomainSearchResults.Interface
{
  public class LogDomainSearchResultsRequestData : RequestData
  {
    private readonly List<SuggestedDomain> _suggestedDomains = new List<SuggestedDomain>();
    public string Domain { get; private set; }
    public int Availability { get; private set; }

    public HashSet<int> AreasToLog { get; private set; }

    public LogDomainSearchResultsRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string domain, int availability) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Domain = domain;
      Availability = availability;
      AreasToLog = new HashSet<int>();
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public void AddSuggestedDomain(string domain, int registrationType, int area, int position, string price, int isPriceDisplayed, int spunName, int available)
    {
      _suggestedDomains.Add(new SuggestedDomain(domain, registrationType, area, position, price, isPriceDisplayed, spunName, available));
      AreasToLog.Add(area);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("LogDomainSearchResults is not a cachable Request");
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("domainSearch");
      xtwRequest.WriteStartElement("search");
      xtwRequest.WriteAttributeString("visitGuid", Pathway);
      xtwRequest.WriteAttributeString("time", DateTime.Now.ToString(CultureInfo.InvariantCulture));
      xtwRequest.WriteAttributeString("pageCount", PageCount.ToString(CultureInfo.InvariantCulture));
      xtwRequest.WriteAttributeString("domain", Domain);
      xtwRequest.WriteAttributeString("isAvailable", Availability.ToString(CultureInfo.InvariantCulture));
      xtwRequest.WriteAttributeString("availability", Availability.ToString(CultureInfo.InvariantCulture));

      foreach (int area in AreasToLog)
      {
        xtwRequest.WriteAttributeString("area", area.ToString(CultureInfo.InvariantCulture));
        break;
      }

      if (_suggestedDomains.Count > 0)
      {
        xtwRequest.WriteStartElement("results");

        foreach (var suggestedDomain in _suggestedDomains)
        {
          xtwRequest.WriteStartElement("suggested");
          xtwRequest.WriteAttributeString("domain", suggestedDomain.Domain);
          xtwRequest.WriteAttributeString("registrationType", suggestedDomain.RegistrationType.ToString(CultureInfo.InvariantCulture));
          xtwRequest.WriteAttributeString("area", suggestedDomain.Area.ToString(CultureInfo.InvariantCulture));
          xtwRequest.WriteAttributeString("position", suggestedDomain.Position.ToString(CultureInfo.InvariantCulture));
          xtwRequest.WriteAttributeString("price", suggestedDomain.Price);
          xtwRequest.WriteAttributeString("isPriceDisplayed", suggestedDomain.IsPriceDisplayed.ToString(CultureInfo.InvariantCulture));
          xtwRequest.WriteAttributeString("spunName", suggestedDomain.SpunName.ToString(CultureInfo.InvariantCulture));
          xtwRequest.WriteAttributeString("availability", suggestedDomain.Availability.ToString(CultureInfo.InvariantCulture));
          xtwRequest.WriteEndElement(); //suggested
        }

        xtwRequest.WriteEndElement(); //results

      }

      xtwRequest.WriteEndElement(); //search
      xtwRequest.WriteEndElement(); //domainSearch

      return sbRequest.ToString();

      /*
     * <domainSearch>
     *    <search visitGuid='108753C2-D5BB-4A44-895F-975923ACA480' time='2009-10-19 14:24:56.580' pageCount='1' domain='xyz.com' isAvailable='1'>
     *      <results>
     *        <suggested domain='xyz.net' registrationType='1' area='1' position='1' price='1099'>
     *        <suggested domain='xyz.info' registrationType='1' area='1' position='2' price='1099'>
     *      </results>
     *   </search>
     * </domainSearch>
     * 
     * Description:
     * <search - Contains the domain name being searched. only 1 expected.
     *      visitGUid -- from pathway cookie        
     *      time -- datetime
     *      pageCount -- from pageCount cookie
     *      domain -- domain name being searched. ex: xyz123.com
     *      isAvailable -- 0 = not available, 1 = available 
     *   <results> 
     *      <suggested -- a node for each result
     *          domain -- domain name being suggested. ex: xyz123.net   
     *          registrationType -- 0 , 1, 2, (0=Backorder, 1=PreRegistration, 2=Registration, 3=Transfer, 4=Bulk, 5=BulkTransfer)
     *          area -- strip mall, additional, premium, international, pricing)
     *          position -- position it appears in the list (1,2,3,etc for first, second, ...) 
     *          price -- price displayed to shopper (no decimals)
     *          spunName -- type of spun name (0-not spun, 1-internal, 2-external). Internal represents the  prefix and suffix names 's', 'site', etc.  External are the names obtained by DomainsBot.
     *          availability -- 0 = not available, 1 = available, 2 = backorder
     *   </results>
     * </search>
     */
    }
  }
}