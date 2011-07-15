using System;
using System.Collections.Generic;
using Atlantis.Framework.DCCGetDomainByShopper.Interface;
using Atlantis.Framework.DCCGetDomainByShopper.Interface.Paging;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DCCGetDomainByShopper.Tests
{
  [TestClass]
  public class DCCGetDomainByShopperTest
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCGetAllDomainsByDomainNamePaging()
    {
      IDomainPaging paging = new DomainNamePaging(SortOrderType.Ascending);

      DCCGetDomainByShopperRequestData request = new DCCGetDomainByShopperRequestData("856907", string.Empty, string.Empty, string.Empty, 0, paging, "MOBILE_CSA_DCC");
      request.RequestTimeout = TimeSpan.FromMinutes(1);
      DCCGetDomainByShopperResponseData response = (DCCGetDomainByShopperResponseData)Engine.Engine.ProcessRequest(request, 100);

      string lastDomainName = null;

      foreach (IDictionary<string, string> domainAttributesDictionary in response.Domains.Values)
      {
        if (lastDomainName == null)
        {
          lastDomainName = domainAttributesDictionary["domainname"];
        }

        if (lastDomainName.CompareTo(domainAttributesDictionary["domainname"]) > 0)
        {
          Assert.Fail("Domains are not in domain name order.");
        }

        lastDomainName = domainAttributesDictionary["domainname"];
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCGetAllDomainsByExpirationDatePaging()
    {
      IDomainPaging paging = new ExpirationDatePaging(SortOrderType.Ascending);

      DCCGetDomainByShopperRequestData request = new DCCGetDomainByShopperRequestData("847235", string.Empty, string.Empty, string.Empty, 0, paging, "MOBILE_CSA_DCC");
      request.RequestTimeout = TimeSpan.FromMinutes(1);
      DCCGetDomainByShopperResponseData response = (DCCGetDomainByShopperResponseData)Engine.Engine.ProcessRequest(request, 100);

      DateTime lastExpirationDate = DateTime.MinValue;

      foreach (IDictionary<string, string> domainAttributesDictionary in response.Domains.Values)
      {
        if(lastExpirationDate == DateTime.MinValue)
        {
          lastExpirationDate = DateTime.Parse(domainAttributesDictionary["sortexpirationdate"]);
        }

        if (lastExpirationDate > DateTime.Parse(domainAttributesDictionary["sortexpirationdate"]))
        {
          Assert.Fail("Domains are not in expiration date order.");
        }

        lastExpirationDate = DateTime.Parse(domainAttributesDictionary["sortexpirationdate"]);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCGet5DomainsByDomainNamePaging()
    {
      IDomainPaging paging = new DomainNamePaging(5, string.Empty, SortOrderType.Ascending);

      DCCGetDomainByShopperRequestData request = new DCCGetDomainByShopperRequestData("847235", string.Empty, string.Empty, string.Empty, 0, paging, "MOBILE_CSA_DCC");
      request.RequestTimeout = TimeSpan.FromMinutes(1);
      DCCGetDomainByShopperResponseData response = (DCCGetDomainByShopperResponseData)Engine.Engine.ProcessRequest(request, 100);

      if(response.Domains.Count != 5)
      {
        Assert.Fail("5 domains were not returned");
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCGet5DomainsByExpriationDatePaging()
    {
      IDomainPaging paging = new ExpirationDatePaging(5, string.Empty, SortOrderType.Ascending);

      DCCGetDomainByShopperRequestData request = new DCCGetDomainByShopperRequestData("847235", string.Empty, string.Empty, string.Empty, 0, paging, "MOBILE_CSA_DCC");
      request.RequestTimeout = TimeSpan.FromMinutes(1);
      DCCGetDomainByShopperResponseData response = (DCCGetDomainByShopperResponseData)Engine.Engine.ProcessRequest(request, 100);

      if (response.Domains.Count != 5)
      {
        Assert.Fail("5 domains were not returned");
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCGetSingleDomain()
    {
      IDomainPaging paging = new DomainNamePaging();
      paging.SearchTerm = "ARVINDGURU.NL";

      DCCGetDomainByShopperRequestData request = new DCCGetDomainByShopperRequestData("859148", string.Empty, string.Empty, string.Empty, 0, paging, "MOBILE_CSA_DCC");
      request.RequestTimeout = TimeSpan.FromMinutes(1);
      DCCGetDomainByShopperResponseData response = (DCCGetDomainByShopperResponseData)Engine.Engine.ProcessRequest(request, 100);

      if (response.Domains.Count != 1)
      {
        Assert.Fail("1 domain is not returned");
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void FilterByStatysTypeTest()
    {
      IDomainPaging paging = new DomainNamePaging(SortOrderType.Ascending);
      paging.RowsPerPage = 20;
      paging.StatusType = 7;

      DCCGetDomainByShopperRequestData request = new DCCGetDomainByShopperRequestData("842103", string.Empty, string.Empty, string.Empty, 0, paging, "MOBILE_CSA_DCC");
      request.RequestTimeout = TimeSpan.FromMinutes(1);
      DCCGetDomainByShopperResponseData response = (DCCGetDomainByShopperResponseData)Engine.Engine.ProcessRequest(request, 100);

      foreach (IDictionary<string, string> domainAttributesDictionary in response.Domains.Values)
      {
        if (domainAttributesDictionary["status"] != "0")
        {
          Assert.Fail("domains with status not 0 found.");
        }
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void SummaryOnlyTest()
    {
      IDomainPaging paging = new ExpirationDatePaging(5, string.Empty, SortOrderType.Ascending);
      paging.SummaryOnly = true;
      DCCGetDomainByShopperRequestData request = new DCCGetDomainByShopperRequestData("847235", string.Empty, string.Empty, string.Empty, 0, paging, "MOBILE_CSA_DCC");
      request.RequestTimeout = TimeSpan.FromMinutes(1);
      DCCGetDomainByShopperResponseData response = (DCCGetDomainByShopperResponseData)Engine.Engine.ProcessRequest(request, 100);

      if (!(response.Domains.Count == 0 && response.FullSummary.ResultCount > 0))
      {
        Assert.Fail("Result is not just summary.");
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AllCOMAndNETDomainsTest()
    {
      IDomainPaging paging = new ExpirationDatePaging();

      paging.StatusType = 7;
      paging.TldIdList.Add(1);
      paging.TldIdList.Add(2);

      DCCGetDomainByShopperRequestData request = new DCCGetDomainByShopperRequestData("856907", string.Empty, string.Empty, string.Empty, 0, paging, "MOBILE_CSA_DCC");
      request.RequestTimeout = TimeSpan.FromMinutes(1);
      DCCGetDomainByShopperResponseData response = (DCCGetDomainByShopperResponseData)Engine.Engine.ProcessRequest(request, 100);

      foreach (string domain in response.Domains.Keys)
      {
        string[] domainParts = domain.Split(".".ToCharArray());
        if (domainParts[1] != "COM" && domainParts[1] != "NET")
        {
          Assert.Fail("Non COM and NET domain returned.");
        }
      }      
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCGetSingleDomainSerializeTest()
    {
      MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);

      IDomainPaging paging = new DomainNamePaging();
      paging.SearchTerm = "ARVINDGURU.NL";

      DCCGetDomainByShopperRequestData request = new DCCGetDomainByShopperRequestData("859148", string.Empty, string.Empty, string.Empty, 0, paging, "MOBILE_CSA_DCC");
      request.RequestTimeout = TimeSpan.FromMinutes(1);

      DCCGetDomainByShopperResponseData response1 = SessionCache.SessionCache.GetProcessRequest<DCCGetDomainByShopperResponseData>(request, 100);
      DCCGetDomainByShopperResponseData response2 = SessionCache.SessionCache.GetProcessRequest<DCCGetDomainByShopperResponseData>(request, 100);

      Assert.AreEqual(response1.FullSummary.FirstDomain, response2.FullSummary.FirstDomain);
    }
  }
}
