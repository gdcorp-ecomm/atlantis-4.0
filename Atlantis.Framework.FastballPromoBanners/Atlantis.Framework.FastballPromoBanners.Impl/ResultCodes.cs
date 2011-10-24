
namespace Atlantis.Framework.FastballPromoBanners.Impl
{
  public static class ResultCodes
  {
    public const int NoError = 0;
    
    public const int SqlException = -3;
    public const int WebServiceDisabled = -10;
    public const int ConfigurationErrorMissingTypeConfig = -20;
    public const int WebServiceArgumentMissing = -40;
    public const int InvalidRequestXml = -50;
    public const int InvalidCandidateRequestXml = -51;
    public const int RequestXmlArgumentMissing = -52;
    public const int CandidateXmlArgumentMissing = -53;
    public const int NoFilterMappingConfiguredForPlacement = -80;
    public const int NoCampaignFiltersSpecifiedForPlacement = -81;
    public const int NoOffersConfiguredForPlacement = -82;
    public const int NoMatchingCampaignsFound = -83;
    public const int NoCompatibleFilterForArguments = -84;

    //***** RULE ENGINE ERRORS **********//
    public const int InvalidRuleConfigurationProvider = -100;
    public const int FailedToLoadRuleConfigurationProvider = -110;
    public const int NoRuleSetConfigurationFound = -111;

    public const int NoProductsFoundForProductGroup = -120;

    //***** RULE ENGINE ERRORS **********//
    public const int FailedToLoadProviderConfig = -150;
    public const int FailedToLoadProvider = -151;
    public const int ProviderAssemblyException = -152;
    public const int InvalidExpressionTree = -160;
    public const int InvalidRuleConfigurationData = -161;
    public const int InvalidRuleValueData = -170;
    public const int InvalidRuleCandidateData = -180;
    public const int InvalidRuleData = -190;
    public const int InvalidArgumentsForProvider = -200;
    public const int UnsupportedRuleOperatorForProvider = -220;

    public const int UnspecifiedException = -999;

    public static bool IsResultException(int resultCode)
    {
      bool isResultException = true;

      switch (resultCode)
      {
        case NoError:
        case NoMatchingCampaignsFound:
        case WebServiceDisabled:
          isResultException = false;
          break;
      }

      return isResultException;
    }
  }
}
