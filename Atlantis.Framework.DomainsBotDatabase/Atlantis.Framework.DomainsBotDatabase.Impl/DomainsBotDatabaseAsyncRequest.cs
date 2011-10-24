using System;
using Atlantis.Framework.DomainsBotDatabase.Impl.domainsBotDatabase;
using Atlantis.Framework.DomainsBotDatabase.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsBotDatabase.Impl
{
  public class DomainsBotDatabaseAsyncRequest : IAsyncRequest
  {
    public IAsyncResult BeginHandleRequest(RequestData domainsDatabaseRequestData, ConfigElement configElement, AsyncCallback asyncCallback, object state)
    {
      DomainsBotDatabaseRequestData requestData = (DomainsBotDatabaseRequestData)domainsDatabaseRequestData;
      FirstImpact3 firstImpact = new FirstImpact3();
      firstImpact.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
      firstImpact.Url = ((WsConfigElement)configElement).WSURL;
      AsyncState asyncState = new AsyncState(domainsDatabaseRequestData, configElement, firstImpact, state);
      IAsyncResult asyncResult = firstImpact.BeginSearchDatabaseDomains(requestData.DatabaseToUse,
                                                                        requestData.DomainNameToSearch,
                                                                        requestData.GetDotTypesString(),
                                                                        requestData.MaxResults,
                                                                        requestData.RemoveKeys,
                                                                        String.Empty,
                                                                        String.Empty,
                                                                        String.Empty,
                                                                        asyncCallback,
                                                                        asyncState);
      return asyncResult;
    }

    public IResponseData EndHandleRequest(IAsyncResult asyncResult)
    {
      IResponseData responseData = null;
      AsyncState asyncState = (AsyncState)asyncResult.AsyncState; 
      try
      {
        FirstImpact3 firstImpact = (FirstImpact3)asyncState.Request;
        Domain[] searchResponse = firstImpact.EndSearchDatabaseDomains(asyncResult);        
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
        responseData = new DomainsBotDatabaseResponseData(asyncState.RequestData, exception);
      }
      return responseData;
    }
  }
}
