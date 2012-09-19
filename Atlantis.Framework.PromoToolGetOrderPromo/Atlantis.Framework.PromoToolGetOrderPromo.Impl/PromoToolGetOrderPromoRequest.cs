using System;
using System.Linq;
using System.ServiceModel;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PromoToolGetOrderPromo.Impl.Svc;
using Atlantis.Framework.PromoToolGetOrderPromo.Interface;
using System.ServiceModel.Security;

namespace Atlantis.Framework.PromoToolGetOrderPromo.Impl
{
	public class PromoToolGetOrderPromoRequest : IRequest
	{
		public IResponseData RequestHandler(RequestData requestData, ConfigElement oConfig)
		{
			IResponseData result = null;

			PromoToolDomainServiceSOAPClient client = null;
			try
			{
				PromoToolGetOrderPromoRequestData promoRequestData = (PromoToolGetOrderPromoRequestData)requestData;
				WsConfigElement config = (WsConfigElement)oConfig;
				client = GetWebServiceInstance(config.WSURL, promoRequestData.RequestTimeout);
				OrderPromoV2 promo = client.GetOrderPromoByPromoCode(promoRequestData.PromoCode);
				result = ConvertSourceData(requestData, promo);
			}
			catch (Exception ex)
			{
				result = new PromoToolGetOrderPromoResponseData(requestData, ex);
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

		private PromoToolGetOrderPromoResponseData ConvertSourceData(RequestData requestData, OrderPromoV2 promo)
		{
			PromoToolGetOrderPromoResponseData responseData = null;

			if (promo != null)
			{
				responseData = new PromoToolGetOrderPromoResponseData();

				responseData.Description = promo.CartDescription;
				responseData.StartDate = promo.StartDate;
				responseData.ExpirationDate = promo.ExpirationDate;
				responseData.IsActive = promo.IsActive;
				responseData.Currencies = promo.Currencies;
				if (promo.PaymentExclusions != null && promo.PaymentExclusions.Length > 0)
				{
					responseData.ExcludedPaymentTypes = (from pe in promo.PaymentExclusions select pe.PaymentExclusionName).ToArray();
				}
				responseData.UseLimit = promo.UseLimit;
				responseData.Restriction = RestrictionType.NoRestriction;
				if (promo.Restriction == OrderPromoV2.RestrictedOneUseOnly.Restricted)
					responseData.Restriction = RestrictionType.Restricted;
				if (promo.Restriction == OrderPromoV2.RestrictedOneUseOnly.RestrictedNewShoppersOnly)
					responseData.Restriction = RestrictionType.NewShopperOnly;
			}
			else
			{
				responseData = new PromoToolGetOrderPromoResponseData(requestData, new Exception("Web method GetOrderPromoByPromoCode returned a null response."));
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
