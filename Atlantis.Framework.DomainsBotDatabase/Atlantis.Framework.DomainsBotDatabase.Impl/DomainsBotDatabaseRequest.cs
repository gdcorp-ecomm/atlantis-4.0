using System;
using Atlantis.Framework.DomainsBotDatabase.Impl.domainsBotDatabase;
using Atlantis.Framework.DomainsBotDatabase.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsBotDatabase.Impl
{
  public class DomainsBotDatabaseRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData domainsBotDatabaseRequestData,
      ConfigElement configElement)
    {
      IResponseData responseData = null;
      try
      {
        DomainsBotDatabaseRequestData requestData
          = (DomainsBotDatabaseRequestData)domainsBotDatabaseRequestData;
        Domain[] searchResponse;
        using (FirstImpact3 firstImpact = new FirstImpact3())
        {
          firstImpact.Url = ((WsConfigElement)configElement).WSURL;
          firstImpact.Timeout = (int)Math.Truncate(requestData.RequestTimeout.TotalMilliseconds);
          searchResponse = firstImpact.SearchDatabaseDomains(requestData.DatabaseToUse,
                                                             requestData.DomainNameToSearch,
                                                             requestData.GetDotTypesString(),
                                                             requestData.MaxResults,
                                                             requestData.RemoveKeys,
                                                             String.Empty,
                                                             String.Empty,
                                                             String.Empty);
        }

        responseData = new DomainsBotDatabaseResponseData(searchResponse.Length);

        foreach (Domain domain in searchResponse)
        {
          if (domain.Data.Length > 0)
          {
            int price = 0;
            int commission = 0;
            DateTime auctionEndTime = DateTime.MinValue;
            bool priceIsValid = true;
            bool commissionIsValid = true;

            foreach (DomainData domainData in domain.Data[0])
            {
              if (domainData.Name.Equals("price", StringComparison.InvariantCulture))
              {
                priceIsValid = int.TryParse(domainData.Data.ToString(), out price);
              }
              else if (domainData.Name.Equals("commission", StringComparison.InvariantCulture))
              {
                commissionIsValid = int.TryParse(domainData.Data.ToString(), out commission);
              }
              else if (domainData.Name.Equals("auctionendtime", StringComparison.InvariantCulture))
              {
                if (!DateTime.TryParse(domainData.Data.ToString(), out auctionEndTime))
                {
                  auctionEndTime = DateTime.MinValue;
                }
              }
            }

            if (priceIsValid && commissionIsValid)
            {
              ((DomainsBotDatabaseResponseData)responseData).AddDomain(domain.DomainName,
              new DatabaseDomainAttributes(price, commission, auctionEndTime));
            }
          }
        }
      }
      catch (AtlantisException atlantisException)
      {
        responseData = new DomainsBotDatabaseResponseData(atlantisException);
      }
      catch (Exception exception)
      {
        responseData = new DomainsBotDatabaseResponseData(domainsBotDatabaseRequestData, exception);
      }
      return responseData;
    }
  }
}
