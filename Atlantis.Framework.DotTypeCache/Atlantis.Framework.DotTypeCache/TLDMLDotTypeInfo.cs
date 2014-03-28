using System;
using System.Collections.Generic;
using System.Web;
using Atlantis.Framework.DCCDomainsDataCache.Interface;
using Atlantis.Framework.DomainContactFields.Interface;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.DotTypeCache.Static;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PrivateLabel.Interface;
using Atlantis.Framework.Providers.TLDDataCache;
using Atlantis.Framework.RegDotTypeProductIds.Interface;
using Atlantis.Framework.RegDotTypeRegistry.Interface;
using Atlantis.Framework.TLDDataCache.Interface;
using Atlantis.Framework.TLDDataCache.Interface.TLDProductDomainAttributes;

namespace Atlantis.Framework.DotTypeCache
{
    public class TLDMLDotTypeInfo : IDotTypeInfo
    {
        private const string ECOMM_GA_LIVE_PHASE_CODE = "REGREG";
        const string MISSING_ID_ERROR = "Missing ProductId for tld: {0}; productType: {1}; registryid: {2}; registrationLength: {3}; domainCount: {4}";

        private static readonly IDictionary<LaunchPhaseGroupTypes, ITLDLaunchPhaseGroup> _emptyLaunchPhaseGroupsDictionary = new Dictionary<LaunchPhaseGroupTypes, ITLDLaunchPhaseGroup>(0);

        private readonly string _tld;
        private readonly Lazy<ProductIdListResponseData> _productIdList;
        private readonly Lazy<RegDotTypeRegistryResponseData> _dotTypeRegistryData;
        private readonly TLDMLByNameResponseData _tldml;
        private readonly Lazy<int> _tldId;
        private readonly Lazy<DomainContactFieldsResponseData> _domainContactFieldsData;
        private readonly Lazy<TLDLanguageResponseData> _languagesData;
        private readonly Lazy<ValidDotTypesResponseData> _validDotTypes;
        private readonly Lazy<ISiteContext> _siteContext;

        internal static TLDMLDotTypeInfo FromDotType(string dotType, IProviderContainer container)
        {
            return new TLDMLDotTypeInfo(dotType, container);
        }

        private TLDMLDotTypeInfo(string tld, IProviderContainer container)
        {
            _tld = tld;
            _tldId = new Lazy<int>(LoadTldId);
            _productIdList = new Lazy<ProductIdListResponseData>(LoadProductIds);
            _dotTypeRegistryData = new Lazy<RegDotTypeRegistryResponseData>(LoadDotTypeRegistryData);
            _domainContactFieldsData = new Lazy<DomainContactFieldsResponseData>(LoadDomainContactFieldsData);
            _languagesData = new Lazy<TLDLanguageResponseData>(LoadLanguagesData);
            _validDotTypes = new Lazy<ValidDotTypesResponseData>(LoadValidDotTypes);
            _siteContext = new Lazy<ISiteContext>(container.Resolve<ISiteContext>);

            _tldml = LoadTLDML();
        }

        private int LoadTldId()
        {
            int tldId;
            _validDotTypes.Value.TryGetTldId(_tld, out tldId);

            return tldId;
        }

        private TLDMLByNameResponseData LoadTLDML()
        {
            var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, _tld);
            return (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.TLDMLByName);
        }

        private RegDotTypeRegistryResponseData LoadDotTypeRegistryData()
        {
            try
            {
                var request = new RegDotTypeRegistryRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, _tld);
                return (RegDotTypeRegistryResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.Registry);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ProductIdListResponseData LoadProductIds()
        {
            try
            {
                var request = new ProductIdListRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, _tld);
                return (ProductIdListResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.ProductIdList);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private DomainContactFieldsResponseData LoadDomainContactFieldsData()
        {
            var request = new DomainContactFieldsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, _tld);
            return (DomainContactFieldsResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.DomainContactFields);
        }

        private TLDLanguageResponseData LoadLanguagesData()
        {
            var request = new TLDLanguageRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, _tldId.Value);
            return (TLDLanguageResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.Languages);
        }

        private ValidDotTypesResponseData LoadValidDotTypes()
        {
            var request = new ValidDotTypesRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
            return (ValidDotTypesResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.ValidDotTypes);
        }

        private TLDProductDomainAttributesResponseData LoadProductDomainAttributes(string phaseCode, int privateLabelId, int productTypeId)
        {
            var pLTypeResponse = LoadPrivateLabelType(privateLabelId);

            if (!string.IsNullOrEmpty(phaseCode))
            {
                var request = new TLDProductDomainAttributesRequestData(_tldId.Value, phaseCode, pLTypeResponse.PrivateLabelType, productTypeId);
                return (TLDProductDomainAttributesResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.ProductDomainAttributes);
            }
            else
            {
                string message = string.Format("Missing phase code for tld: {0}; productTypeId: {1}; privateLabelId: {2};", _tld, productTypeId.ToString(), privateLabelId.ToString());
                Logging.LogException("TLDMLDotTypeInfo.GetProductId", message, _tld);
            }

            return null;
        }

        private PrivateLabelTypeResponseData LoadPrivateLabelType(int privateLabelId)
        {
            var request = new PrivateLabelTypeRequestData(privateLabelId);
            return (PrivateLabelTypeResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.PrivateLabelType);
        }

        public string DotType
        {
            get { return _tld; }
        }

        public int MinExpiredAuctionRegLength
        {
            get { return _tldml.Product.ExpiredAuctionsYears.Min; }
        }

        public int MaxExpiredAuctionRegLength
        {
            get { return _tldml.Product.ExpiredAuctionsYears.Max; }
        }

        public int MinRegistrationLength
        {
            get { return _tldml.Product.RegistrationYears.Min; }
        }

        public int MaxRegistrationLength
        {
            get { return _tldml.Product.RegistrationYears.Max; }
        }

        public int MinTransferLength
        {
            get
            {
                return _tldml.Product.TransferYears.Min;
            }
        }

        public int MaxTransferLength
        {
            get
            {
                return _tldml.Product.TransferYears.Max;
            }
        }

        public int MinRenewalLength
        {
            get
            {
                return _tldml.Product.RenewalYears.Min;
            }
        }

        public int MaxRenewalLength
        {
            get
            {
                return _tldml.Product.RegistrationYears.Max;
            }
        }

        public bool IsMultiRegistry
        {
            get
            {
                return _tldml.ApplicationControl.IsMultiRegistry;
            }
        }

        public bool IsGtld
        {
            get
            {
                return _tldml.Tld.IsGtld;
            }
        }


        public IEnumerable<RegistryLanguage> RegistryLanguages
        {
            get { return _languagesData.Value.RegistryLanguages; }
        }

        public RegistryLanguage GetLanguageByName(string languageName)
        {
            return _languagesData.Value.GetLanguageDataByName(languageName);
        }

        public RegistryLanguage GetLanguageById(int languageId)
        {
            return _languagesData.Value.GetLanguageDataById(languageId);
        }

        public bool CanRenew(DateTime currentExpirationDate, out int maxValidRenewalLength)
        {
            maxValidRenewalLength = -1;
            var canRenew = false;

            var origExpirationDate = currentExpirationDate;

            for (var i = MaxRenewalLength; i >= MinRenewalLength; i--)
            {
                var d = origExpirationDate;
                var newRenewalDate = d.AddYears(i);
                var maxRenewalDate = DateTime.Now.AddYears(MaxRenewalLength);

                if (!string.IsNullOrEmpty(Tld.RenewProhibitedPeriodForExpirationUnit))
                {
                    switch (Tld.RenewProhibitedPeriodForExpirationUnit)
                    {
                        case "month":
                            maxRenewalDate = DateTime.Now.Date.AddMonths(Tld.RenewProhibitedPeriodForExpiration * -1);
                            break;
                        case "day":
                            maxRenewalDate = DateTime.Now.Date.AddDays(Tld.RenewProhibitedPeriodForExpiration * -1);
                            break;
                    }
                }

                if (newRenewalDate <= maxRenewalDate)
                {
                    maxValidRenewalLength = i;
                    break;
                }
            }

            if (maxValidRenewalLength > 0)
            {
                int? renewalMonthsBeforeExpiration = TLDRenewal.GetRenewalMonthsBeforeExpiration(DotType);
                if (renewalMonthsBeforeExpiration.HasValue)
                {
                    var renewalEligibilityDate = origExpirationDate.AddMonths(renewalMonthsBeforeExpiration.Value * -1);

                    if (DateTime.Now.Date >= renewalEligibilityDate)
                    {
                        canRenew = true;
                    }
                }
                else
                {
                    canRenew = true;
                }
            }

            return canRenew;
        }

        public int GetExpiredAuctionRegProductId(int registrationLength, int domainCount)
        {
            return InternalGetProductId(registrationLength, domainCount, DotTypeProductTypes.ExpiredAuctionReg);
        }

        public int GetExpiredAuctionRegProductId(string registryId, int registrationLength, int domainCount)
        {
            return InternalGetProductId(registryId, registrationLength, domainCount, DotTypeProductTypes.ExpiredAuctionReg);
        }

        public int GetPreRegProductId(LaunchPhases phase, int registrationLength, int domainCount)
        {
            IDomainProductLookup domainProductLookup = DomainProductLookup.Create(registrationLength, domainCount, phase, TLDProductTypes.Registration);

            return GetProductId(domainProductLookup);
        }

        public int GetPreRegProductId(LaunchPhases phase, string registryId, int registrationLength, int domainCount)
        {
            int? registryIdValue = GetRegistryIdFromString(registryId);

            IDomainProductLookup domainProductLookup = DomainProductLookup.Create(registrationLength, domainCount, phase, TLDProductTypes.Registration, null, registryIdValue);

            return GetProductId(domainProductLookup);
        }

        public int GetRegistrationProductId(int registrationLength, int domainCount)
        {
            return InternalGetProductId(registrationLength, domainCount, DotTypeProductTypes.Registration);
        }

        public int GetRegistrationProductId(string registryId, int registrationLength, int domainCount)
        {
            return InternalGetProductId(registryId, registrationLength, domainCount, DotTypeProductTypes.Registration);
        }

        public int GetTransferProductId(int registrationLength, int domainCount)
        {
            return InternalGetProductId(registrationLength, domainCount, DotTypeProductTypes.Transfer);
        }

        public int GetTransferProductId(string registryId, int registrationLength, int domainCount)
        {
            return InternalGetProductId(registryId, registrationLength, domainCount, DotTypeProductTypes.Transfer);
        }

        public int GetRenewalProductId(int registrationLength, int domainCount)
        {
            return InternalGetProductId(registrationLength, domainCount, DotTypeProductTypes.Renewal);
        }

        public int GetRenewalProductId(string registryId, int registrationLength, int domainCount)
        {
            return InternalGetProductId(registryId, registrationLength, domainCount, DotTypeProductTypes.Renewal);
        }

        public List<int> GetTrusteeProductId(TLDProductTypes productType)
        {
            List<int> productIds = new List<int>();
            if (productType == TLDProductTypes.Registration)
            {
                productIds = _tldml.Product.GetPhaseApplicationProductIdList("Trustee");
            }
            else if (productType == TLDProductTypes.Renewal)
            {
                productIds = _tldml.Product.GetPhaseApplicationProductIdList("Trustee Renewal");
            }
            return productIds;
        }

        private int? GetRegistryIdFromString(string registryId)
        {
            int? registryIdValue = null;

            int registryIdInt;
            if (int.TryParse(registryId, out registryIdInt))
            {
                registryIdValue = registryIdInt;
            }

            return registryIdValue;
        }

        private int InternalGetProductId(int registrationLength, int domainCount, DotTypeProductTypes productType)
        {
            var result = 0;

            string registryId = null;

            if (productType == DotTypeProductTypes.Registration && _dotTypeRegistryData.Value != null)
            {
                registryId = _dotTypeRegistryData.Value.RegistrationAPI.Id;
            }
            else if (productType == DotTypeProductTypes.Transfer && _dotTypeRegistryData.Value != null)
            {
                registryId = _dotTypeRegistryData.Value.TransferAPI.Id;
            }


            int? registryIdValue = GetRegistryIdFromString(registryId);

            IDomainProductLookup domainProductLookup;
            switch (productType)
            {
                case DotTypeProductTypes.Registration:
                    domainProductLookup = DomainProductLookup.Create(registrationLength, domainCount, LaunchPhases.GeneralAvailability,
                                                                      TLDProductTypes.Registration, null, registryIdValue);
                    result = GetProductId(domainProductLookup);
                    break;
                case DotTypeProductTypes.Transfer:
                    domainProductLookup = DomainProductLookup.Create(registrationLength, domainCount, LaunchPhases.GeneralAvailability,
                                                                      TLDProductTypes.Transfer, null, registryIdValue);
                    result = GetProductId(domainProductLookup);
                    break;
                case DotTypeProductTypes.Renewal:
                    domainProductLookup = DomainProductLookup.Create(registrationLength, domainCount, LaunchPhases.GeneralAvailability,
                                                                      TLDProductTypes.Renewal, null, registryIdValue);
                    result = GetProductId(domainProductLookup);
                    break;
                 case DotTypeProductTypes.Trustee:
                   domainProductLookup = DomainProductLookup.Create(registrationLength, domainCount, LaunchPhases.GeneralAvailability,
                                                                     TLDProductTypes.Trustee, null, registryIdValue);
                   result = GetProductId(domainProductLookup);
                   break;
                default:
                    result = InternalGetProductId(registryId, registrationLength, domainCount, productType);
                    break;
            }

            return result;
        }

        private int InternalGetProductId(string registryId, int registrationLength, int domainCount, DotTypeProductTypes productType)
        {
            int result = 0;
            DotTypeProductTiers tiers = null;

            if (!string.IsNullOrEmpty(registryId))
            {
                if (_productIdList.Value != null)
                {
                    tiers = _productIdList.Value.GetProductTiersForRegistry(registryId, productType);
                }
            }

            if (tiers == null)
            {
                if (_productIdList.Value != null)
                {
                    tiers = _productIdList.Value.GetDefaultProductTiers(productType);
                }
            }

            if (tiers != null)
            {
                DotTypeProduct product;

                if (tiers.TryGetProduct(registrationLength, domainCount, out product))
                {
                    result = product.ProductId;
                }

            }

            if (result == 0)
            {
                LogMissingProductId(registryId, registrationLength, domainCount, productType);
            }

            return result;
        }

        private void LogMissingProductId(string registryId, int registrationLength, int domainCount, DotTypeProductTypes productType)
        {
            string message = string.Format(MISSING_ID_ERROR, _tld, productType.ToString(), registryId, registrationLength, domainCount);
            Logging.LogException("TLDMLDotTypeInfo.GetProductId", message, _tld);
        }

        public List<int> GetValidExpiredAuctionRegProductIdList(int domainCount, params int[] registrationLengths)
        {
            return InternalGetValidProductIds(DotTypeProductTypes.ExpiredAuctionReg, _tldml.Product.ExpiredAuctionsYears, domainCount, registrationLengths);
        }

        public List<int> GetValidExpiredAuctionRegProductIdList(string registryId, int domainCount, params int[] registrationLengths)
        {
            return InternalGetValidProductIds(DotTypeProductTypes.ExpiredAuctionReg, _tldml.Product.ExpiredAuctionsYears, registryId, domainCount, registrationLengths);
        }

        public List<int> GetValidPreRegProductIdList(LaunchPhases phase, int domainCount, params int[] registrationLengths)
        {
            IDomainProductListLookup domainProductListLookup = DomainProductListLookup.Create(registrationLengths, domainCount, phase, TLDProductTypes.Registration);

            return GetProductIdList(domainProductListLookup);
        }

        public List<int> GetValidPreRegProductIdList(LaunchPhases phase, string registryId, int domainCount, params int[] registrationLengths)
        {
            int? registryIdValue = GetRegistryIdFromString(registryId);

            IDomainProductListLookup domainProductListLookup = DomainProductListLookup.Create(registrationLengths, domainCount, phase, TLDProductTypes.Registration, null, registryIdValue);

            return GetProductIdList(domainProductListLookup);
        }

        public List<int> GetValidRegistrationProductIdList(int domainCount, params int[] registrationLengths)
        {
            IDomainProductListLookup domainProductListLookup = DomainProductListLookup.Create(registrationLengths, domainCount, LaunchPhases.GeneralAvailability, TLDProductTypes.Registration);
            return GetProductIdList(domainProductListLookup);
        }

        public List<int> GetValidRegistrationProductIdList(string registryId, int domainCount, params int[] registrationLengths)
        {
            int? registryIdValue = GetRegistryIdFromString(registryId);

            IDomainProductListLookup domainProductListLookup = DomainProductListLookup.Create(registrationLengths, domainCount, LaunchPhases.GeneralAvailability, TLDProductTypes.Registration, null, registryIdValue);
            return GetProductIdList(domainProductListLookup);
        }

        public List<int> GetValidTransferProductIdList(int domainCount, params int[] registrationLengths)
        {
            IDomainProductListLookup domainProductListLookup = DomainProductListLookup.Create(registrationLengths, domainCount, LaunchPhases.GeneralAvailability, TLDProductTypes.Transfer);
            return GetProductIdList(domainProductListLookup);
        }

        public List<int> GetValidTransferProductIdList(string registryId, int domainCount, params int[] registrationLengths)
        {
            int? registryIdValue = GetRegistryIdFromString(registryId);
            IDomainProductListLookup domainProductListLookup = DomainProductListLookup.Create(registrationLengths, domainCount, LaunchPhases.GeneralAvailability, TLDProductTypes.Transfer, null, registryIdValue);
            return GetProductIdList(domainProductListLookup);
        }

        public List<int> GetValidRenewalProductIdList(int domainCount, params int[] registrationLengths)
        {
            IDomainProductListLookup domainProductListLookup = DomainProductListLookup.Create(registrationLengths, domainCount, LaunchPhases.GeneralAvailability, TLDProductTypes.Renewal);
            return GetProductIdList(domainProductListLookup);
        }

        public List<int> GetValidRenewalProductIdList(string registryId, int domainCount, params int[] registrationLengths)
        {
            int? registryIdValue = GetRegistryIdFromString(registryId);
            IDomainProductListLookup domainProductListLookup = DomainProductListLookup.Create(registrationLengths, domainCount, LaunchPhases.GeneralAvailability, TLDProductTypes.Renewal, null, registryIdValue);
            return GetProductIdList(domainProductListLookup);
        }

        private List<int> InternalGetValidProductIds(DotTypeProductTypes productType, ITLDValidYearsSet validYears, string registryId, int domainCount, params int[] requestedYears)
        {
            List<DotTypeProduct> products = InternalGetValidProducts(productType, validYears, registryId, domainCount, requestedYears);
            return products.ConvertAll(product => product.ProductId);
        }

        private List<int> InternalGetValidProductIds(DotTypeProductTypes productType, ITLDValidYearsSet validYears, int domainCount, params int[] requestedYears)
        {
            List<DotTypeProduct> products = InternalGetValidProducts(productType, validYears, domainCount, requestedYears);
            return products.ConvertAll(product => product.ProductId);
        }

        public List<int> GetValidExpiredAuctionRegLengths(int domainCount, params int[] registrationLengths)
        {
            return InternalGetValidYears(DotTypeProductTypes.ExpiredAuctionReg, _tldml.Product.ExpiredAuctionsYears, domainCount, registrationLengths);
        }

        public List<int> GetValidPreRegLengths(LaunchPhases phase, int domainCount, params int[] registrationLengths)
        {
            IDomainProductListLookup domainProductListLookup = DomainProductListLookup.Create(registrationLengths, domainCount, phase, TLDProductTypes.Registration);
            return TldProducts(domainProductListLookup).ConvertAll(product => product.Years);
        }

        public List<int> GetValidRegistrationLengths(int domainCount, params int[] registrationLengths)
        {
            return InternalGetValidYears(DotTypeProductTypes.Registration, _tldml.Product.RegistrationYears, domainCount, registrationLengths);
        }

        public List<int> GetValidTransferLengths(int domainCount, params int[] registrationLengths)
        {
            return InternalGetValidYears(DotTypeProductTypes.Transfer, _tldml.Product.TransferYears, domainCount, registrationLengths);
        }

        public List<int> GetValidRenewalLengths(int domainCount, params int[] registrationLengths)
        {
            return InternalGetValidYears(DotTypeProductTypes.Renewal, _tldml.Product.RenewalYears, domainCount, registrationLengths);
        }

        private List<int> InternalGetValidYears(DotTypeProductTypes productType, ITLDValidYearsSet validYears, int domainCount, params int[] requestedYears)
        {
            List<DotTypeProduct> products = InternalGetValidProducts(productType, validYears, domainCount, requestedYears);
            return products.ConvertAll(product => product.Years);
        }

        private List<DotTypeProduct> InternalGetValidProducts(DotTypeProductTypes productType, ITLDValidYearsSet validYears, int domainCount, params int[] requestedYears)
        {
            string registryId = null;

            if (productType == DotTypeProductTypes.Registration && _dotTypeRegistryData.Value != null)
            {
                registryId = _dotTypeRegistryData.Value.RegistrationAPI.Id;
            }
            else if (productType == DotTypeProductTypes.Transfer && _dotTypeRegistryData.Value != null)
            {
                registryId = _dotTypeRegistryData.Value.TransferAPI.Id;
            }

            return InternalGetValidProducts(productType, validYears, registryId, domainCount, requestedYears);
        }

        private List<DotTypeProduct> InternalGetValidProducts(DotTypeProductTypes productType, ITLDValidYearsSet validYears, string registryId, int domainCount, params int[] requestedYears)
        {
            List<DotTypeProduct> result = new List<DotTypeProduct>(10);
            DotTypeProductTiers tiers = null;

            if (!string.IsNullOrEmpty(registryId))
            {
                if (_productIdList.Value != null)
                {
                    tiers = _productIdList.Value.GetProductTiersForRegistry(registryId, productType);
                }
            }

            if (tiers == null)
            {
                if (_productIdList.Value != null)
                {
                    tiers = _productIdList.Value.GetDefaultProductTiers(productType);
                }
            }

            if (tiers != null)
            {
                foreach (int registrationLength in requestedYears)
                {
                    if (validYears.IsValid(registrationLength))
                    {
                        DotTypeProduct product;
                        if ((tiers.TryGetProduct(registrationLength, domainCount, out product)) && (product.IsValid))
                        {
                            result.Add(product);
                        }
                    }
                }
            }

            return result;
        }

        public string GetRegistryIdByProductId(int productId)
        {
            string result = string.Empty;

            DotTypeProduct product;
            if (_productIdList.Value != null && _productIdList.Value.TryGetProductByProductId(productId, out product))
            {
                result = product.RegistryId;
            }

            return result;
        }

        public ITLDProduct Product
        {
            get { return _tldml.Product; }
        }

        public int TldId
        {
            get { return _tldId.Value; }
        }

        public ITLDTld Tld
        {
            get { return _tldml.Tld; }
        }

        public ITLDApplicationControl ApplicationControl
        {
            get
            {
                return _tldml.ApplicationControl;
            }
        }

        public ITLDLaunchPhaseGroupCollection GetAllLaunchPhaseGroups(bool activeOnly = true)
        {
            IDictionary<LaunchPhaseGroupTypes, ITLDLaunchPhaseGroup> launchPhasePeriodGroupsDictionary;

            if (_tldml.Phase == null)
            {
                launchPhasePeriodGroupsDictionary = _emptyLaunchPhaseGroupsDictionary;
            }
            else
            {
                IList<ITLDLaunchPhase> launchPhases = _tldml.Phase.GetAllLaunchPhases(activeOnly);
                launchPhasePeriodGroupsDictionary = new Dictionary<LaunchPhaseGroupTypes, ITLDLaunchPhaseGroup>(8);

                foreach (LaunchPhaseGroupTypes phaseGroup in Enum.GetValues(typeof(LaunchPhaseGroupTypes)))
                {
                    string groupPrefix = LaunchPhaseMappings.GetCodePrefix(phaseGroup);

                    foreach (ITLDLaunchPhase launchPhase in launchPhases)
                    {
                        if (launchPhase.Code.StartsWith(groupPrefix, StringComparison.OrdinalIgnoreCase))
                        {
                            ITLDLaunchPhaseGroup launchPhaseGroup;

                            if (!launchPhasePeriodGroupsDictionary.TryGetValue(phaseGroup, out launchPhaseGroup))
                            {
                                launchPhaseGroup = TldLaunchPhaseGroup.CreateEmptyGroup();
                                launchPhasePeriodGroupsDictionary[phaseGroup] = launchPhaseGroup;
                            }

                            launchPhaseGroup.Phases.Add(launchPhase);
                        }
                    }
                }
            }

            return TldLaunchPhaseGroupCollection.CreateCollection(launchPhasePeriodGroupsDictionary);
        }

        public ITLDLaunchPhase GetLaunchPhase(LaunchPhases phase)
        {
            ITLDLaunchPhase launchPhase = null;
            if (_tldml.Phase != null)
            {
                launchPhase = _tldml.Phase.GetLaunchPhase(LaunchPhaseMappings.GetCode(phase));
            }
            return launchPhase;
        }

        public bool IsPreRegPhaseActive
        {
            get
            {
                bool result = false;
                if (_tldml.Phase != null)
                {
                    result = _tldml.Phase.IsPreRegPhaseActive;
                }
                return result;
            }
        }

        public IList<string> GetTuiFormTypes(LaunchPhases launchPhase)
        {
            var result = new List<string>(16);

            if (_tldml.ApplicationControl != null)
            {
                string launchPhaseCode = LaunchPhaseMappings.GetCode(launchPhase);

                bool needsClaimCheck = false;

                if (HttpContext.Current != null)
                {
                    var tuiClaimsStarted = HttpContext.Current.Request.Params["qa--claimsstarted"];
                    if (!string.IsNullOrEmpty(tuiClaimsStarted) && tuiClaimsStarted == "1")
                    {
                        var overRideTlds = TldsHelper.OverrideTlds();
                        if (overRideTlds.Contains(DotType))
                        {
                            needsClaimCheck = true;
                        }
                    }
                }

                if (!needsClaimCheck && _tldml.Phase != null)
                {
                    var phase = _tldml.Phase.GetLaunchPhase(launchPhaseCode);
                    if (phase != null)
                    {
                        needsClaimCheck = phase.NeedsClaimCheck;
                    }
                }

                var formGroups = _tldml.ApplicationControl.TuiFormGroups;
                if (formGroups != null)
                {
                    foreach (var tldTuiFormGroup in formGroups)
                    {
                        var formgroupLaunchPhases = tldTuiFormGroup.Value.FormGrouplaunchPhases;
                        foreach (var tldTuiFormGroupLaunchPhase in formgroupLaunchPhases)
                        {
                            if (tldTuiFormGroupLaunchPhase.Code.Equals(launchPhaseCode))
                            {
                                if (!string.IsNullOrEmpty(tldTuiFormGroupLaunchPhase.PeriodType) &&
                                    tldTuiFormGroupLaunchPhase.PeriodType.Equals("claimsacceptance", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (needsClaimCheck)
                                    {
                                        result.Add(tldTuiFormGroup.Key);
                                    }
                                }
                                else
                                {
                                    result.Add(tldTuiFormGroup.Key);
                                }
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        public bool RequiresTuiForm(LaunchPhases launchPhase)
        {
            bool result = false;

            if (_tldml.Product != null && _tldml.Product.Trustee != null)
            {
                result = _tldml.Product.Trustee.IsRequired;
            }

            if (!result)
            {
                var tuiFormTypes = GetTuiFormTypes(launchPhase);
                result = tuiFormTypes.Count > 0;
            }

            return result;
        }

        public int GetMinPreRegLength(LaunchPhases phase)
        {
            return _tldml.Product.PreregistrationYears(LaunchPhaseMappings.GetCode(phase)).Min;
        }

        public int GetMaxPreRegLength(LaunchPhases phase)
        {
            return _tldml.Product.PreregistrationYears(LaunchPhaseMappings.GetCode(phase)).Max;
        }

        public bool HasPhaseApplicationFee(LaunchPhases phase, out string applicationProductType)
        {
            bool result = _tldml.Product.HasPhaseApplicationFee(LaunchPhaseMappings.GetCode(phase), out applicationProductType);

            return result;
        }

        public List<int> GetPhaseApplicationProductIdList(LaunchPhases phase)
        {
            var result = new List<int>();

            string applicationProductType;

            if (HasPhaseApplicationFee(phase, out applicationProductType))
            {
                result = _tldml.Product.GetPhaseApplicationProductIdList(applicationProductType);
            }

            return result;
        }

        public int GetProductId(IDomainProductLookup domainProductLookup)
        {
            var result = 0;

            var productTypeId = InternalGetTldProductTypeId(domainProductLookup.DomainCount, domainProductLookup.ProductType);
            var priceTierId = domainProductLookup.PriceTierId;

            if (priceTierId == null)
            {
                if (_tldml.Product != null && _tldml.Product.RegistryPremiumDomains != null && _tldml.Product.RegistryPremiumDomains.DefaultPremiumTier > 0)
                {
                    priceTierId = _tldml.Product.RegistryPremiumDomains.DefaultPremiumTier;
                }
            }

            var productTiers = InternalGetProductTiers(domainProductLookup.TldPhase, productTypeId, domainProductLookup.RegistryId, priceTierId);
            if (productTiers != null && !productTiers.ValidTierExist(domainProductLookup.DomainCount))
            {
                productTypeId = InternalGetTldProductTypeId(domainProductLookup.DomainCount, domainProductLookup.ProductType, false);

                productTiers = InternalGetProductTiers(domainProductLookup.TldPhase, productTypeId, domainProductLookup.RegistryId, priceTierId);
            }

            if (productTiers != null)
            {
                TLDProduct product;
                if (productTiers.TryGetProduct(domainProductLookup.Years, domainProductLookup.DomainCount, out product))
                {
                    result = product.UnifiedProductId;
                }
            }

            return result;
        }

        public List<int> GetProductIdList(IDomainProductListLookup domainProductListLookup)
        {
            var result = TldProducts(domainProductListLookup);

            return result.ConvertAll(product => product.UnifiedProductId);
        }

        private List<TLDProduct> TldProducts(IDomainProductListLookup domainProductListLookup)
        {
            var result = new List<TLDProduct>(8);
            var priceTierId = domainProductListLookup.PriceTierId;

            if (priceTierId == null)
            {
                if (_tldml.Product != null && _tldml.Product.RegistryPremiumDomains != null && _tldml.Product.RegistryPremiumDomains.DefaultPremiumTier > 0)
                {
                    priceTierId = _tldml.Product.RegistryPremiumDomains.DefaultPremiumTier;
                }
            }


            var productTypeId = InternalGetTldProductTypeId(domainProductListLookup.DomainCount,
              domainProductListLookup.ProductType);
            var productTiers = InternalGetProductTiers(domainProductListLookup.TldPhase, productTypeId,
              domainProductListLookup.RegistryId, priceTierId);
            if (productTiers != null && !productTiers.ValidTierExist(domainProductListLookup.DomainCount))
            {
                productTypeId = InternalGetTldProductTypeId(domainProductListLookup.DomainCount, domainProductListLookup.ProductType,
                  false);
                productTiers = InternalGetProductTiers(domainProductListLookup.TldPhase, productTypeId,
                  domainProductListLookup.RegistryId, priceTierId);
            }

            if (productTiers != null)
            {
                foreach (int year in domainProductListLookup.Years)
                {
                    TLDProduct product;
                    if ((productTiers.TryGetProduct(year, domainProductListLookup.DomainCount, out product)) && (product.IsValid))
                    {
                        result.Add(product);
                    }
                }
            }
            return result;
        }

        private static int InternalGetTldProductTypeId(int domainCount, TLDProductTypes productType, bool getBulkBasedOnDomainCount = true)
        {
            var productTypeId = TLDProductTypeIds.TLD_PRODUCT_TYPE_ID_REGISTRATION;

            switch (productType)
            {
                case TLDProductTypes.Registration:
                    productTypeId = domainCount > 1 && getBulkBasedOnDomainCount ?
                      TLDProductTypeIds.TLD_PRODUCT_TYPE_ID_BULK_REGISTRATION : TLDProductTypeIds.TLD_PRODUCT_TYPE_ID_REGISTRATION;
                    break;
                case TLDProductTypes.Transfer:
                    productTypeId = domainCount > 1 && getBulkBasedOnDomainCount ?
                      TLDProductTypeIds.TLD_PRODUCT_TYPE_ID_BULK_TRANSFER : TLDProductTypeIds.TLD_PRODUCT_TYPE_ID_TRANSFER;
                    break;
                case TLDProductTypes.Renewal:
                    productTypeId = domainCount > 1 && getBulkBasedOnDomainCount ?
                      TLDProductTypeIds.TLD_PRODUCT_TYPE_ID_BULK_RENEWAL : TLDProductTypeIds.TLD_PRODUCT_TYPE_ID_RENEWAL;
                    break;
            }

            return productTypeId;
        }

        private TLDProductTiers InternalGetProductTiers(LaunchPhases phase, int productTypeId, int? registryId, int? priceTierId)
        {
            var phaseCode = GetPhaseCode(phase, productTypeId);

            var productDomainAttributesResponse = LoadProductDomainAttributes(phaseCode, _siteContext.Value.PrivateLabelId, productTypeId);

            TLDProductTiers productTiers = null;

            if (productDomainAttributesResponse != null)
            {
                if (registryId.HasValue && priceTierId.HasValue)
                {
                    productTiers = productDomainAttributesResponse.GetProductTiersByRegistryAndPriceTier(registryId.Value, priceTierId.Value);
                }
                else if (registryId.HasValue)
                {
                    productTiers = productDomainAttributesResponse.GetProductTiersByRegistry(registryId.Value);
                }
                else if (priceTierId.HasValue)
                {
                    productTiers = productDomainAttributesResponse.GetProductTiersByPriceTier(priceTierId.Value);
                }

                if (productTiers == null)
                {
                    productTiers = productDomainAttributesResponse.GetDefaultProductTiers();
                }
            }

            return productTiers;
        }

        private string GetPhaseCode(LaunchPhases phase, int productTypeId)
        {
            string phaseCode = LaunchPhaseMappings.GetCode(phase);
            ITLDLaunchPhase lookupPhase = _tldml.Phase.GetLaunchPhase(phaseCode);
            if (lookupPhase != null)
            {
                if ((productTypeId != TLDProductTypeIds.TLD_PRODUCT_TYPE_ID_REGISTRATION &&
                     productTypeId != TLDProductTypeIds.TLD_PRODUCT_TYPE_ID_BULK_REGISTRATION) ||
                    (phase == LaunchPhases.GeneralAvailability && lookupPhase.LivePeriod.IsActive))
                {
                    phaseCode = ECOMM_GA_LIVE_PHASE_CODE;
                }
            }
            return phaseCode;
        }

        public string GetRegistrationFieldsXml()
        {
            return _domainContactFieldsData.Value.DomainContactFields;
        }
    }
}
