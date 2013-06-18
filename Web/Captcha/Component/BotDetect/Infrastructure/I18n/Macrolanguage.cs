using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;

namespace BotDetect
{
    internal enum Macrolanguage
    {
        None = 0,
        Albanian,
        Arabic,
        Chinese,
        Estonian,
        Latvian,
        Malay,
        Norwegian,
        SerboCroatian
    }

    internal sealed class MacrolanguageCodes
    {
        private MacrolanguageCodes() { }

        public static Macrolanguage GetMacrolanguage(string code)
        {
            Macrolanguage language = Macrolanguage.None;

            switch (code.ToLowerInvariant())
            {
                case "sq":
                case "sqi":
                    language = Macrolanguage.Albanian;
                    break;

                case "ar":
                case "ara":
                    language = Macrolanguage.Arabic;
                    break;

                case "zh":
                case "zho":
                    language = Macrolanguage.Chinese;
                    break;

                case "et":
                case "est":
                    language = Macrolanguage.Estonian;
                    break;

                case "lv":
                case "lav":
                    language = Macrolanguage.Latvian;
                    break;

                case "ms":
                case "msa":
                case "may":
                    language = Macrolanguage.Malay;
                    break;

                case "no":
                case "nor":
                    language = Macrolanguage.Norwegian;
                    break;

                case "sh":
                case "hbs":
                    language = Macrolanguage.SerboCroatian;
                    break;
            }

            return language;
        }

        public static string GetCode(Macrolanguage language)
        {
            string code = "";

            switch (language)
            {
                case Macrolanguage.Albanian:
                    code = "sq";
                    break;

                case Macrolanguage.Arabic:
                    code = "ar";
                    break;

                case Macrolanguage.Chinese:
                    code = "zh";
                    break;

                case Macrolanguage.Estonian:
                    code = "et";
                    break;

                case Macrolanguage.Latvian:
                    code = "lv";
                    break;

                case Macrolanguage.Malay:
                    code = "ms";
                    break;

                case Macrolanguage.Norwegian:
                    code = "no";
                    break;

                case Macrolanguage.SerboCroatian:
                    code = "sh";
                    break;

                default:
                    code = "";
                    break;
            }

            return code;
        }
    }
}
