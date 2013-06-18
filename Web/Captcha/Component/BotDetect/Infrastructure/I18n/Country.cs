using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;

namespace BotDetect
{
    internal enum Country
    {
        Unknown = 0,
        Albania,
        Algeria,
        Argentina,
        Australia,
        Austria,
        Bahrain,
        Belarus,
        Belgium,
        Belize,
        BiH,
        Bolivia,
        Brazil,
        Bulgaria,
        Canada,
        Chile,
        Colombia,
        CostaRica,
        Croatia,
        CzechRepublic,
        Denmark,
        DominicanRepublic,
        Ecuador,
        Egypt,
        ElSalvador,
        Estonia,
        FaroeIslands,
        Finland,
        France,
        Germany,
        Greece,
        Greenland,
        Guatemala,
        Honduras,
        HongKong,
        Hungary,
        Iceland,
        India,
        Iraq,
        Ireland,
        Israel,
        Italy,
        Jamaica,
        Japan,
        Jordan,
        Korea,
        Kuwait,
        Latvia,
        Lebanon,
        Libya,
        Liechtenstein,
        Lithuania,
        Luxembourg,
        Macao,
        Macedonia,
        Malaysia,
        Malta,
        Mexico,
        Monaco,
        Morocco,
        Netherlands,
        NewZealand,
        Nicaragua,
        Norway,
        Oman,
        Panama,
        Paraguay,
        Peru,
        Philippines,
        Poland,
        Portugal,
        PRC,
        PuertoRico,
        Qatar,
        Romania,
        Russia,
        SaudiArabia,
        Serbia,
        Singapore,
        Slovakia,
        Slovenia,
        SouthAfrica,
        Spain,
        Sweden,
        Switzerland,
        Syria,
        Taiwan,
        TrinidadTobago,
        Tunisia,
        Turkey,
        UAE,
        Ukraine,
        UK,
        USA,
        Uruguay,
        Venezuela,
        Vietnam,
        Yemen,
        Zimbabwe
    }

    internal sealed class CountryCodes
    {
        private CountryCodes() { }

        public const int CountryCodeLength = 2;

        public static string GetCode(Country country)
        {
            string code = "";

            switch (country)
            {
                case Country.Albania:
                    code = "AL";
                    break;

                case Country.Algeria:
                    code = "DZ";
                    break;

                case Country.Argentina:
                    code = "AR";
                    break;

                case Country.Australia:
                    code = "AU";
                    break;

                case Country.Austria:
                    code = "AT";
                    break;

                case Country.Bahrain:
                    code = "BH";
                    break;

                case Country.Belarus:
                    code = "BY";
                    break;

                case Country.Belgium:
                    code = "BE";
                    break;

                case Country.Belize:
                    code = "BZ";
                    break;

                case Country.BiH:
                    code = "BA";
                    break;

                case Country.Bolivia:
                    code = "BO";
                    break;

                case Country.Brazil:
                    code = "BR";
                    break;

                case Country.Bulgaria:
                    code = "BG";
                    break;

                case Country.Canada:
                    code = "CA";
                    break;

                case Country.Chile:
                    code = "CL";
                    break;

                case Country.Colombia:
                    code = "CO";
                    break;

                case Country.CostaRica:
                    code = "CR";
                    break;

                case Country.Croatia:
                    code = "HR";
                    break;

                case Country.CzechRepublic:
                    code = "CZ";
                    break;

                case Country.Denmark:
                    code = "DK";
                    break;

                case Country.DominicanRepublic:
                    code = "DO";
                    break;

                case Country.Ecuador:
                    code = "EC";
                    break;

                case Country.Egypt:
                    code = "EG";
                    break;

                case Country.ElSalvador:
                    code = "SV";
                    break;

                case Country.Estonia:
                    code = "EE";
                    break;

                case Country.FaroeIslands:
                    code = "FO";
                    break;

                case Country.Finland:
                    code = "FI";
                    break;

                case Country.France:
                    code = "FR";
                    break;

                case Country.Germany:
                    code = "DE";
                    break;

                case Country.Greece:
                    code = "GR";
                    break;

                case Country.Greenland:
                    code = "GL";
                    break;

                case Country.Guatemala:
                    code = "GT";
                    break;

                case Country.Honduras:
                    code = "HN";
                    break;

                case Country.HongKong:
                    code = "HK";
                    break;

                case Country.Hungary:
                    code = "HU";
                    break;

                case Country.Iceland:
                    code = "IS";
                    break;

                case Country.India:
                    code = "IN";
                    break;

                case Country.Iraq:
                    code = "IQ";
                    break;

                case Country.Ireland:
                    code = "IE";
                    break;

                case Country.Israel:
                    code = "IL";
                    break;

                case Country.Italy:
                    code = "IT";
                    break;

                case Country.Jamaica:
                    code = "JM";
                    break;

                case Country.Japan:
                    code = "JP";
                    break;

                case Country.Jordan:
                    code = "JO";
                    break;

                case Country.Korea:
                    code = "KR";
                    break;

                case Country.Kuwait:
                    code = "KW";
                    break;

                case Country.Latvia:
                    code = "LV";
                    break;

                case Country.Lebanon:
                    code = "LB";
                    break;

                case Country.Libya:
                    code = "LY";
                    break;

                case Country.Liechtenstein:
                    code = "LI";
                    break;

                case Country.Lithuania:
                    code = "LT";
                    break;

                case Country.Luxembourg:
                    code = "LU";
                    break;

                case Country.Macao:
                    code = "MO";
                    break;

                case Country.Macedonia:
                    code = "MK";
                    break;

                case Country.Malaysia:
                    code = "MY";
                    break;

                case Country.Malta:
                    code = "MT";
                    break;

                case Country.Mexico:
                    code = "MX";
                    break;

                case Country.Monaco:
                    code = "MC";
                    break;

                case Country.Morocco:
                    code = "MA";
                    break;

                case Country.Netherlands:
                    code = "NL";
                    break;

                case Country.NewZealand:
                    code = "NZ";
                    break;

                case Country.Nicaragua:
                    code = "NI";
                    break;

                case Country.Norway:
                    code = "NO";
                    break;

                case Country.Oman:
                    code = "OM";
                    break;

                case Country.Panama:
                    code = "PA";
                    break;

                case Country.Paraguay:
                    code = "PY";
                    break;

                case Country.Peru:
                    code = "PE";
                    break;

                case Country.Philippines:
                    code = "PH";
                    break;

                case Country.Poland:
                    code = "PL";
                    break;

                case Country.Portugal:
                    code = "PT";
                    break;

                case Country.PRC:
                    code = "CN";
                    break;

                case Country.PuertoRico:
                    code = "PR";
                    break;

                case Country.Qatar:
                    code = "QA";
                    break;

                case Country.Romania:
                    code = "RO";
                    break;

                case Country.Russia:
                    code = "RU";
                    break;

                case Country.SaudiArabia:
                    code = "SA";
                    break;

                case Country.Serbia:
                    code = "RS";
                    break;

                case Country.Singapore:
                    code = "SG";
                    break;

                case Country.Slovakia:
                    code = "SK";
                    break;

                case Country.Slovenia:
                    code = "SI";
                    break;

                case Country.SouthAfrica:
                    code = "ZA";
                    break;

                case Country.Spain:
                    code = "ES";
                    break;

                case Country.Sweden:
                    code = "SE";
                    break;

                case Country.Switzerland:
                    code = "CH";
                    break;

                case Country.Syria:
                    code = "SY";
                    break;

                case Country.Taiwan:
                    code = "TW";
                    break;

                case Country.TrinidadTobago:
                    code = "TT";
                    break;

                case Country.Tunisia:
                    code = "TN";
                    break;

                case Country.Turkey:
                    code = "TR";
                    break;

                case Country.UAE:
                    code = "AE";
                    break;

                case Country.Ukraine:
                    code = "UA";
                    break;

                case Country.UK:
                    code = "GB";
                    break;

                case Country.USA:
                    code = "US";
                    break;

                case Country.Uruguay:
                    code = "UY";
                    break;

                case Country.Venezuela:
                    code = "VE";
                    break;

                case Country.Vietnam:
                    code = "VN";
                    break;

                case Country.Yemen:
                    code = "YE";
                    break;

                case Country.Zimbabwe:
                    code = "ZW";
                    break;

                default:
                    code = "";
                    break;
            }

            return code;
        }

        public static Country GetCountry(string code)
        {
            Country country = Country.Unknown;

            switch (code.ToUpperInvariant())
            {
                case "AL":
                    country = Country.Albania;
                    break;

                case "DZ":
                    country = Country.Algeria;
                    break;

                case "AR":
                    country = Country.Argentina;
                    break;

                case "AU":
                    country = Country.Australia;
                    break;

                case "AT":
                    country = Country.Austria;
                    break;

                case "BH":
                    country = Country.Bahrain;
                    break;

                case "BY":
                    country = Country.Belarus;
                    break;

                case "BE":
                    country = Country.Belgium;
                    break;

                case "BZ":
                    country = Country.Belize;
                    break;

                case "BA":
                    country = Country.BiH;
                    break;

                case "BO":
                    country = Country.Bolivia;
                    break;

                case "BR":
                    country = Country.Brazil;
                    break;

                case "BG":
                    country = Country.Bulgaria;
                    break;

                case "CA":
                    country = Country.Canada;
                    break;

                case "CL":
                    country = Country.Chile;
                    break;

                case "CO":
                    country = Country.Colombia;
                    break;

                case "CR":
                    country = Country.CostaRica;
                    break;

                case "HR":
                    country = Country.Croatia;
                    break;

                case "CZ":
                    country = Country.CzechRepublic;
                    break;

                case "DK":
                    country = Country.Denmark;
                    break;

                case "DO":
                    country = Country.DominicanRepublic;
                    break;

                case "EC":
                    country = Country.Ecuador;
                    break;

                case "EG":
                    country = Country.Egypt;
                    break;

                case "SV":
                    country = Country.ElSalvador;
                    break;

                case "EE":
                    country = Country.Estonia;
                    break;

                case "FO":
                    country = Country.FaroeIslands;
                    break;

                case "FI":
                    country = Country.Finland;
                    break;

                case "FR":
                    country = Country.France;
                    break;

                case "DE":
                    country = Country.Germany;
                    break;

                case "GR":
                    country = Country.Greece;
                    break;

                case "GL":
                    country = Country.Greenland;
                    break;

                case "GT":
                    country = Country.Guatemala;
                    break;

                case "HN":
                    country = Country.Honduras;
                    break;

                case "HK":
                    country = Country.HongKong;
                    break;

                case "HU":
                    country = Country.Hungary;
                    break;

                case "IS":
                    country = Country.Iceland;
                    break;

                case "IN":
                    country = Country.India;
                    break;

                case "IQ":
                    country = Country.Iraq;
                    break;

                case "IE":
                    country = Country.Ireland;
                    break;

                case "IL":
                    country = Country.Israel;
                    break;

                case "IT":
                    country = Country.Italy;
                    break;

                case "JM":
                    country = Country.Jamaica;
                    break;

                case "JP":
                    country = Country.Japan;
                    break;

                case "JO":
                    country = Country.Jordan;
                    break;

                case "KR":
                    country = Country.Korea;
                    break;

                case "KW":
                    country = Country.Kuwait;
                    break;

                case "LV":
                    country = Country.Latvia;
                    break;

                case "LB":
                    country = Country.Lebanon;
                    break;

                case "LY":
                    country = Country.Libya;
                    break;

                case "LI":
                    country = Country.Liechtenstein;
                    break;

                case "LT":
                    country = Country.Lithuania;
                    break;

                case "LU":
                    country = Country.Luxembourg;
                    break;

                case "MO":
                    country = Country.Macao;
                    break;

                case "MK":
                    country = Country.Macedonia;
                    break;

                case "MY":
                    country = Country.Malaysia;
                    break;

                case "MT":
                    country = Country.Malta;
                    break;

                case "MX":
                    country = Country.Mexico;
                    break;

                case "MC":
                    country = Country.Monaco;
                    break;

                case "MA":
                    country = Country.Morocco;
                    break;

                case "NL":
                    country = Country.Netherlands;
                    break;

                case "NZ":
                    country = Country.NewZealand;
                    break;

                case "NI":
                    country = Country.Nicaragua;
                    break;

                case "NO":
                    country = Country.Norway;
                    break;

                case "OM":
                    country = Country.Oman;
                    break;

                case "PA":
                    country = Country.Panama;
                    break;

                case "PY":
                    country = Country.Paraguay;
                    break;

                case "PE":
                    country = Country.Peru;
                    break;

                case "PH":
                    country = Country.Philippines;
                    break;

                case "PL":
                    country = Country.Poland;
                    break;

                case "PT":
                    country = Country.Portugal;
                    break;

                case "CN":
                    country = Country.PRC;
                    break;

                case "PR":
                    country = Country.PuertoRico;
                    break;

                case "QA":
                    country = Country.Qatar;
                    break;

                case "RO":
                    country = Country.Romania;
                    break;

                case "RU":
                    country = Country.Russia;
                    break;

                case "SA":
                    country = Country.SaudiArabia;
                    break;

                case "RS":
                    country = Country.Serbia;
                    break;

                case "SG":
                    country = Country.Singapore;
                    break;

                case "SK":
                    country = Country.Slovakia;
                    break;

                case "SI":
                    country = Country.Slovenia;
                    break;

                case "ZA":
                    country = Country.SouthAfrica;
                    break;

                case "ES":
                    country = Country.Spain;
                    break;

                case "SE":
                    country = Country.Sweden;
                    break;

                case "CH":
                    country = Country.Switzerland;
                    break;

                case "SY":
                    country = Country.Syria;
                    break;

                case "TW":
                    country = Country.Taiwan;
                    break;

                case "TT":
                    country = Country.TrinidadTobago;
                    break;

                case "TN":
                    country = Country.Tunisia;
                    break;

                case "TR":
                    country = Country.Turkey;
                    break;

                case "AE":
                    country = Country.UAE;
                    break;

                case "UA":
                    country = Country.Ukraine;
                    break;

                case "GB":
                    country = Country.UK;
                    break;

                case "US":
                    country = Country.USA;
                    break;

                case "UY":
                    country = Country.Uruguay;
                    break;

                case "VE":
                    country = Country.Venezuela;
                    break;

                case "VN":
                    country = Country.Vietnam;
                    break;

                case "YE":
                    country = Country.Yemen;
                    break;

                case "ZW":
                    country = Country.Zimbabwe;
                    break;

                default:
                    Debug.Assert(false, "Unkown country code");
                    break;
            }

            return country;
        }

    }
}
