using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace BotDetect.CaptchaCode
{
	[Serializable]
    internal class CharacterSet : ICodeGenerator
    {
        private Set<Int32> _alpha;
        private Set<Int32> _numeric;
        private Set<Int32> _alphanumeric;
        private string _name;

        /// <summary>
        /// used for predefined charset creation
        /// </summary>
        public CharacterSet(Int32[] alpha, Int32[] numeric, Int32[] alphanumeric, string name)
        {
            _alpha = new Set<Int32>(alpha);
            _numeric = new Set<Int32>(numeric);
            _alphanumeric = new Set<Int32>(alphanumeric);
            _name = name;
        }

        /// <summary>
        /// used for locale-specific charset creation
        /// </summary>
        public CharacterSet(CharacterSet predefined, Set<Int32> diff, string name)
        {
            _alpha = predefined._alpha - diff;
            _numeric = predefined._numeric - diff;
            _alphanumeric = predefined._alphanumeric - diff;
            _name = name;
        }

        /// <summary>
        /// used for custom charset creation
        /// </summary>
        public CharacterSet(StringCollection alphaChars, StringCollection numericChars, StringCollection alphanumericChars, string name)
        {
            // alphanumeric chars are mandatory
            _alphanumeric = StringHelper.GetCodePoints(alphanumericChars);

            // alpha chars are used only if specified
            if (null == alphaChars || 0 == alphaChars.Count)
            {
                _alpha = _alphanumeric;
            }
            else
            {
                _alpha = StringHelper.GetCodePoints(alphaChars);
            }

            // numeric chars are used only if specified
            if (null == numericChars || 0 == numericChars.Count)
            {
                _numeric= _alphanumeric;
            }
            else
            {
                _numeric = StringHelper.GetCodePoints(numericChars);
            }

            _name = name;
        }

        public Code GenerateCode(CodeStyle codeStyle, int length)
        {
            if ((length > CaptchaDefaults.MaxCodeLength) || (length < CaptchaDefaults.MinCodeLength))
            {
                length = CaptchaDefaults.CodeLength;
            }

            if (CaptchaConfiguration.CaptchaCodes.TestMode.Enabled)
            {
                return new Code("TEST");
            }

            string code = this.GenerateRandomCode(length, codeStyle);

            return new Code(code);
        }

        public static bool IsCodeFilteringEnabled
        {
            get
            {
                return (null != CaptchaBase.BannedCharacterSequences);
            }
        }

        public string GenerateRandomCode(int codeLength, CodeStyle codeStyle)
        {
            if (IsCodeFilteringEnabled)
            {
                return GenerateFilteredRandomCode(codeLength, codeStyle);
            }

            Set<Int32> characterCandidates = GetCandidates(codeStyle);
            string code = String.Empty;

            // randomly select codeLength of characters and add them to the code
            StringBuilder builder = new StringBuilder(CaptchaDefaults.MaxCodeLength);
            for (int i = 0; i < codeLength; i++)
            {
                string selected = Char.ConvertFromUtf32(characterCandidates.Next());
                builder.Append(selected);
            }

            code = builder.ToString();
            return code;
        }

        protected string GenerateFilteredRandomCode(int codeLength, CodeStyle codeStyle)
        {
            Set<Int32> characterCandidates = GetCandidates(codeStyle);
            // single characters that are banned in all codes
            characterCandidates -= CaptchaBase.BannedCharacterSequences.GetBannedCharacters();

            StringBuilder builder = new StringBuilder(CaptchaDefaults.MaxCodeLength);
            string code = String.Empty;

            // randomly select codeLength of characters and add them to the code
            builder.Append(Char.ConvertFromUtf32(characterCandidates.Next())); // 1st char
            for (int i = 1; i < codeLength; i++)
            {
                // all further chars take filtered sequences into account and
                // check all substrings of current code for banned sequences of appropriate length
                Set<Int32> candidates = new Set<Int32>(characterCandidates);

                // filtering algorithm example:
                // - if the first character is "a", for second character generation we check:
                //   all banned sequences of length = 2 starting with "a";
                //   giving us a set of characters to exclude for second character generation
                // - if the first two characters are "ab", for third character generation we check:
                //   all banned sequences of length = 3 starting with "ab" AND 
                //   all banned sequences of length = 2 starting with "b";
                //   giving us a set of characters to exclude for third character generation
                // - if the first three characters are "abc", for fourth character generation we check:
                //   all banned sequences of length = 4 starting with "abc" AND
                //   all banned sequences of length = 3 starting with "bc" AND 
                //   all banned sequences of length = 2 starting with "c";
                //   giving us a set of characters to exclude for fourth character generation
                //
                // note: since Captcha codes are short (usually 4-8 characters), such recursive 
                // processing is acceptable performance-wise

                string current = builder.ToString();
                Set<Int32> bannedCharacters = new Set<Int32>();
                for (int j = i; j >= 1; j--)
                {
                    string substring = current.Substring(i - j, j);
                    bannedCharacters += CaptchaBase.BannedCharacterSequences.GetBannedCharacters(j + 1, substring);
                }

                // exclude characters resulting in banned sequences from current char generation
                candidates = characterCandidates - bannedCharacters;
                if (0 == candidates.Count)
                {
                    throw new CaptchaCodeGenerationException("Banned Sequences list is too restrictive, no non-banned character choices!", current);
                }

                // randomly select one of the acceptable character choices
                string selected = Char.ConvertFromUtf32(candidates.Next());
                builder.Append(selected);
            }

            code = builder.ToString();
            return code;
        }

        protected Set<Int32> GetCandidates(CodeStyle codeStyle)
        {
            Set<Int32> characterCandidates;

            // select the possible characters set depending on the codeStyle
            switch (codeStyle)
            {
                case CodeStyle.Alpha:
                    characterCandidates = _alpha;
                    break;

                case CodeStyle.Numeric:
                    characterCandidates = _numeric;
                    break;

                case CodeStyle.Alphanumeric:
                    characterCandidates = _alphanumeric;
                    break;

                default:
                    throw new CaptchaCodeGenerationException("CodeStyle not implemented!", codeStyle);
            }

            return characterCandidates;
        }
    }
}
