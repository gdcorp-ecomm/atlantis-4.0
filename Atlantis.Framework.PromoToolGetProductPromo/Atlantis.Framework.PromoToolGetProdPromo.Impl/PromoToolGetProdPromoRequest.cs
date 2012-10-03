using System;
using System.Linq;
using System.ServiceModel;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PromoToolGetProdPromo.Impl.Svc;
using Atlantis.Framework.PromoToolGetProdPromo.Interface;

namespace Atlantis.Framework.PromoToolGetProdPromo.Impl
{
	public class PromoToolGetProdPromoRequest : IRequest
	{
		public IResponseData RequestHandler(RequestData requestData, ConfigElement oConfig)
		{
			IResponseData result = null;

			PromoToolDomainServiceSOAPClient client = null;
			try
			{
				PromoToolGetProdPromoRequestData promoRequestData = (PromoToolGetProdPromoRequestData)requestData;
				WsConfigElement config = (WsConfigElement)oConfig;
				client = GetWebServiceInstance(config.WSURL, promoRequestData.RequestTimeout);
				QueryResultOfProductPromo promos = client.GetAllProductPromosByPromoCode(promoRequestData.PromoCode);
				result = ConvertSourceData(requestData, promos);
			}
			catch (Exception ex)
			{
				result = new PromoToolGetProdPromoResponseData(requestData, ex);
			}
			finally
			{
				if (client != null && client.State == CommunicationState.Opened)
				{
					client.Close();
				}
			}

			return result;
		}

		private PromoToolGetProdPromoResponseData ConvertSourceData(RequestData requestData, QueryResultOfProductPromo promos)
		{
			PromoToolGetProdPromoResponseData responseData = null;

      if (promos != null && promos.RootResults != null && promos.RootResults.Length > 0)
      {
        ProdPromo[] productPromos = new ProdPromo[promos.RootResults.Length];

        for (int i = 0; i < promos.RootResults.Length; i++)
        {
          ProdPromo promo = new ProdPromo();
          ProductPromo productPromo = promos.RootResults[i];

          int rankValue;
          if (int.TryParse(productPromo.RankValue, out rankValue))
          {
            promo.RankValue = rankValue;
          }
          else
          {
            rankValue = 10; //use 10 as the default
          }

          promo.Description = productPromo.Description;
          promo.StartDate = productPromo.StartDate;
          promo.ExpirationDate = productPromo.ExpirationDate;
          promo.IsActive = productPromo.IsActive;
          promo.Currencies = productPromo.Currencies;
          promo.Restriction = RestrictionType.NoRestriction;
          if (productPromo.Restriction == OrderPromoV2.RestrictedOneUseOnly.Restricted)
            promo.Restriction = RestrictionType.Restricted;
          if (productPromo.Restriction == OrderPromoV2.RestrictedOneUseOnly.RestrictedNewShoppersOnly)
            promo.Restriction = RestrictionType.NewShopperOnly;
          promo.UseLimit = productPromo.UseLimit;
          promo.UsePerPurchase = productPromo.NumberOfUses;
          promo.IsRestrictedByShopperId = (productPromo.ShopperIdRestrictions.Length > 0);
          promo.ShopperPriceTypeExclusion = productPromo.PromoExclusion;

          productPromos[i] = promo;
        }

        responseData = new PromoToolGetProdPromoResponseData(productPromos);
      }
      else
      {
        responseData = new PromoToolGetProdPromoResponseData();
      }

			return responseData;
		}

		private PromoToolDomainServiceSOAPClient GetWebServiceInstance(string webServiceUrl, TimeSpan requestTimeout)
		{
			BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
			basicHttpBinding.SendTimeout = requestTimeout;
			basicHttpBinding.OpenTimeout = requestTimeout;
			basicHttpBinding.CloseTimeout = requestTimeout;
			basicHttpBinding.ReceiveTimeout = TimeSpan.FromMinutes(10); // default
			basicHttpBinding.AllowCookies = false;
			basicHttpBinding.BypassProxyOnLocal = false;
			basicHttpBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
			basicHttpBinding.MessageEncoding = WSMessageEncoding.Text;
			basicHttpBinding.TextEncoding = System.Text.Encoding.UTF8;
			basicHttpBinding.UseDefaultWebProxy = true;

			basicHttpBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
			basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
			basicHttpBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
			basicHttpBinding.Security.Transport.Realm = string.Empty;
			basicHttpBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

			EndpointAddressBuilder endpointAddressBuilder = new EndpointAddressBuilder();
			endpointAddressBuilder.Identity = EndpointIdentity.CreateDnsIdentity("localhost");
			endpointAddressBuilder.Uri = new Uri(webServiceUrl);

			return new PromoToolDomainServiceSOAPClient(basicHttpBinding, endpointAddressBuilder.ToEndpointAddress());
		}


	}
}
