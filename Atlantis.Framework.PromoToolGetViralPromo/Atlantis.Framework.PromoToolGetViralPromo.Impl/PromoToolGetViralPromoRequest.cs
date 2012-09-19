using System;
using System.Linq;
using System.ServiceModel;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PromoToolGetViralPromo.Impl.Svc;
using Atlantis.Framework.PromoToolGetViralPromo.Interface;

namespace Atlantis.Framework.PromoToolGetViralPromo.Impl
{
	public class PromoToolGetViralPromoRequest : IRequest
	{
		public IResponseData RequestHandler(RequestData requestData, ConfigElement oConfig)
		{
			IResponseData result = null;

			PromoToolDomainServiceSOAPClient client = null;
			try
			{
				PromoToolGetViralPromoRequestData promoRequestData = (PromoToolGetViralPromoRequestData)requestData;
				WsConfigElement config = (WsConfigElement)oConfig;
				client = GetWebServiceInstance(config.WSURL, promoRequestData.RequestTimeout);
				QueryResultOfViralPromo promos = client.GetAllViralPromosByPromoCode(promoRequestData.PromoCode);
				result = ConvertSourceData(requestData, promos);
			}
			catch (Exception ex)
			{
				result = new PromoToolGetViralPromoResponseData(requestData, ex);
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

		private PromoToolGetViralPromoResponseData ConvertSourceData(RequestData requestData, QueryResultOfViralPromo promos)
		{
			PromoToolGetViralPromoResponseData responseData = null;

			if (promos != null && promos.RootResults != null && promos.RootResults.Length > 0)
			{
				responseData = new PromoToolGetViralPromoResponseData();
				responseData.ViralPromos = new OutputViralPromo[promos.RootResults.Length];

				for (int i = 0; i < promos.RootResults.Length; i++)
				{
					OutputViralPromo promo = new OutputViralPromo();

					ViralPromo viralPromo = promos.RootResults[i];
					promo.Description = viralPromo.Description;
					promo.StartDate = viralPromo.StartDate;
					promo.ExpirationDate = viralPromo.ExpirationDate;
					promo.IsActive = viralPromo.IsActive;
					promo.Currencies = viralPromo.Currencies;
					promo.NewShopperOnly = viralPromo.NewShoppersOnly;
					promo.UseLimit = viralPromo.UseLimit;
					if (viralPromo.PaymentExclusions != null && viralPromo.PaymentExclusions.Length > 0)
					{
						promo.ExcludedPaymentTypes = (from pe in viralPromo.PaymentExclusions select pe.PaymentExclusionName).ToArray();
					}
					promo.RequiredYard = viralPromo.RequiredYard;

					responseData.ViralPromos[i] = promo;
				}
			}
			else
			{
				responseData = new PromoToolGetViralPromoResponseData(requestData, new Exception("Web method GetAllViralPromosByPromoCode returned a null or empty response."));
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
