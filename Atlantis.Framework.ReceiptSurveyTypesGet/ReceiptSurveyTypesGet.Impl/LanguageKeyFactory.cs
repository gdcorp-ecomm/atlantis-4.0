using System.Collections.Generic;

namespace Atlantis.Framework.ReceiptSurveyTypesGet.Impl
{
  public class LanguageKeyFactory
  {
    Dictionary<string, string> _languagePairs = new Dictionary<string, string>();

    public string GetLanguageKeyForSurveyTypeId(string surveyTypeId)
    {
      string languageKey = string.Empty;

      if(_languagePairs.ContainsKey(surveyTypeId))
      {
        languageKey = _languagePairs[surveyTypeId];
      }

      return languageKey;
    }

    public LanguageKeyFactory()
    {
      CreateLanguagePairs();
    }

    private void CreateLanguagePairs()
    {
      //NOTE: These pairs were taken from production.  They SHOULD, but may not, match to DEV/TEST databases.
      _languagePairs["1"] = "surveyTvOther";
      _languagePairs["2"] = "surveyRBy";
      _languagePairs["3"] = "surveySE";
      _languagePairs["4"] = "surveyPA";
      _languagePairs["5"] = "surveyAlreadyCust";
      _languagePairs["6"] = "surveyOther";
      _languagePairs["7"] = "surveyTvSB";
      _languagePairs["8"] = "surveyTvIndy";
      _languagePairs["10"] = "surveyTvNC";
      _languagePairs["14"] = "surveyTvMLB";
      _languagePairs["15"] = "surveyTvCBB";
      _languagePairs["26"] = "surveyTvFB";
      _languagePairs["28"] = "surveyTvCF";
      _languagePairs["29"] = "surveyTvBB";
      _languagePairs["30"] = "surveyTvNBA";
      _languagePairs["42"] = "surveyMNS";
      _languagePairs["47"] = "surveyTvMMA";
      _languagePairs["49"] = "surveyTvSEF";
      _languagePairs["50"] = "surveySM";
      _languagePairs["51"] = "surveyTvBoxing";
      _languagePairs["54"] = "surveyTvTennis";
      _languagePairs["58"] = "surveyBillboard";
      _languagePairs["59"] = "surveyTvHockey";
      _languagePairs["60"] = "surveyTvRugby";
      _languagePairs["61"] = "surveyTvNFL";
      _languagePairs["63"] = "surveyTvDewTour";
      _languagePairs["64"] = "surveyTvGDB";
      _languagePairs["66"] = "surveyTvCricket";
      _languagePairs["67"] = "surveyTvAFL";
      _languagePairs["68"] = "surveyTvSoccer";
      _languagePairs["69"] = "surveyTvFutbol";
      _languagePairs["70"] = "surveyTvFO";
      _languagePairs["71"] = "surveyTvV8";
      _languagePairs["72"] = "surveyTvFooty";
      _languagePairs["73"] = "surveyTvSLTV";
      _languagePairs["74"] = "surveyTvTTS";
      _languagePairs["75"] = "surveyTvYT";
      _languagePairs["76"] = "surveyTvDD";
      _languagePairs["78"]=  "surveyEventWorkshop";
      _languagePairs["79"] ="surveyEventOnlineTraining";
      _languagePairs["80"] = "surveyEventTradeshowExpo";
    }
  }
}
