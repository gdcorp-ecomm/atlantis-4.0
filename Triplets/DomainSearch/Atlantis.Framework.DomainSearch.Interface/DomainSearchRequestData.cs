using System.Collections.Generic;
using Atlantis.Framework.Interface;
using System;
using Newtonsoft.Json.Linq;

namespace Atlantis.Framework.DomainSearch.Interface
{
  public class DomainSearchRequestData : RequestData
  {
    public DomainSearchRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
    }
    
    public string SearchPhrase { get; set; }
    public string ClientIp { get; set; }
    public string SourceCode { get; set; }
    public ShopperStatusType ShopperStatus { get; set; }
    public string CountrySite { get; set; }
    public string Language { get; set; }
    public IList<string> Tlds { get; set; }
    public int DomainCount { get; set; }
    public bool IncludeSpins { get; set; }
    public int PrivateLabelId { get; set; }
    public IList<string> DomainSearchDataBases { get; set; }
    
    public string ToJson()
    {
      var jsonSearchData = new JObject(
        new JProperty("SearchPhrase", SearchPhrase),
        new JProperty("RequestingServer", Environment.MachineName), // part of triplet
        new JProperty("ClientIP", ClientIp), // part of provider
        new JProperty("TimeoutInMilliSeconds", (int) RequestTimeout.TotalMilliseconds), // part of provider
        new JProperty("SourceCode", SourceCode),
        new JProperty("PrivateLabelID", PrivateLabelId), // part of provider
        new JProperty("Data",
                      new JArray(
                        new JObject(
                          new JProperty("Name", "searchdatabase"),
                          new JProperty("Data", string.Join(",", DomainSearchDataBases)) // Not an option  for phase 1
                          ),
                        new JObject(
                          new JProperty("Name", "shopperid"), // part of provider
                          new JProperty("Data", ShopperID)
                          ),
                        new JObject(
                          new JProperty("Name", "shopperstatus"), // part of provider
                          new JProperty("Data", ShopperStatus.ToString())
                          ),
                        new JObject(
                          new JProperty("Name", "countrysite"), // part of provider
                          new JProperty("Data", CountrySite.ToLowerInvariant())
                          ),
                        new JObject(
                          new JProperty("Name", "language"), // part of provider
                          new JProperty("Data", Language.ToLowerInvariant())
                          ),
                        new JObject(
                          new JProperty("Name", "includespins"), // Not an option  for phase 1
                          new JProperty("Data", IncludeSpins) // Not an option  for phase 1
                          ),
                        new JObject(
                          new JProperty("Name", "pathway"), // part of provider
                          new JProperty("Data", Pathway)
                          )
                        )
          )
        );

      if (Tlds != null && Tlds.Count > 0)
      {
        var tldsToken = new JObject(
          new JProperty("Name", "tlds"), // Not an option  for phase 1
          new JProperty("Data", string.Join(",", Tlds)) // Not an option  for phase 1
          );
        jsonSearchData["Data"].Last.AddAfterSelf(tldsToken);
      }

      if (DomainCount > 0)
      {
        var domainCountToken = new JObject(
          new JProperty("Name", "maxdomainsperdatabase"), // Not an option  for phase 1
          new JProperty("Data", DomainCount) // Not an option  for phase 1
          );
        jsonSearchData["Data"].Last.AddAfterSelf(domainCountToken);
      }

      return jsonSearchData.ToString();
    }
  }
}
