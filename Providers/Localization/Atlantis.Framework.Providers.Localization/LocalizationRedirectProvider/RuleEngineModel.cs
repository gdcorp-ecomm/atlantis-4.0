
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Localization
{
 public class RedirectSiteRuleEngineModel
 {
   const string HAS_COUNTRY_COOKIE_KEY = "hasCookie";
   const string COUNTRY_REDIRECT_MODEL_ID_KEY = "mdlCountryRedirect";
   const string IP_COUNTRY_KEY = "ipCountry";
   const string COUNTRY_PREFERENCE_KEY = "countryPref";

   public const string LANGUAGE_PREFENCE_KEY = "langPref";
   public const string CURRENT_COUNTRY_SITE_KEY = "countrySite";
   
   public string ModelId {
     get { return COUNTRY_REDIRECT_MODEL_ID_KEY; }
   }

   public bool HasCountryCookie { get; set; }
   public bool IsAuthenticated { get; set; }
   public string LanguagePreference { get; set; }
   public string IPCountry { get; set; }
   public string CurrentSubdomainSite { get; set; }
   public string CountryPreference { get; set; }

   private Dictionary<string, Dictionary<string, string>> _model;
   public Dictionary<string, Dictionary<string, string>> Model
   {
     get
     {
       if (_model == null)
       {
         _model = new Dictionary<string, Dictionary<string, string>>(1);
         _model.Add(ModelId, new Dictionary<string, string>
                               {
                                 {LANGUAGE_PREFENCE_KEY, LanguagePreference},
                                 {HAS_COUNTRY_COOKIE_KEY, HasCountryCookie.ToString()},
                                 {IP_COUNTRY_KEY, IPCountry},
                                 {CURRENT_COUNTRY_SITE_KEY, CurrentSubdomainSite},
                                 {COUNTRY_PREFERENCE_KEY, CountryPreference}
                               });
       }
       return _model;
     }
   }
 }
}
