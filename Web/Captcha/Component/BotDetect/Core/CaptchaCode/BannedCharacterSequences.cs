using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace BotDetect
{
    /// <summary>
    /// Represents a collection of banned character sequences that will be 
    /// excluded from all randomly generated Captcha codes.
    /// </summary>
	[Serializable]
    public class BannedCharacterSequences
    {
        Dictionary<int, Set<string>> _sequencesByLength = new Dictionary<int, Set<string>>();
        private int _maxSequenceLenght = 0;

        internal static Regex WhitespaceChars = new Regex(@"\W", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        
        internal static Regex ValidChars = new Regex(@"^(\p{N}|\p{L})+$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        /// The default BannedCharacterSequences constructor takes a list of 
        /// banned sequences as simple strings
        /// </summary>
        /// <param name="input"></param>
        public BannedCharacterSequences(List<string> input)
        {
            if (null == input || 0 == input.Count)
            {
                return;
            }

            // sorted by length and alphabetical order
            string[] inputs = input.ToArray();
            Array.Sort(inputs, new BannedSequenceComparer());

            foreach (string value in inputs)
            {
                ProcessDeclaration(value);
            }
        }

        private void ProcessDeclaration(string value)
        {
            string sequence = GetSequence(value);

            // avoid invalid values
            if (!IsValidSequence(sequence)) { return; }

            // avoid duplicate sequences
            if (IsDuplicate(sequence)) { return; }

            // add new sequence to dictionary
            if (!AlreadyMatchedByShorterSequence(sequence))
            {
                AddSequence(sequence);
            }
        }

        private string GetSequence(string value)
        {
            // ignore whitespace chars
            string sequence = WhitespaceChars.Replace(value.ToUpperInvariant(), "");

            return sequence;
        }

        private bool IsValidSequence(string sequence)
        {
            if (!StringHelper.HasValue(sequence)) { return false; }

            if (sequence.Length > CaptchaDefaults.MaxCodeLength) { return false; }

            if (!ValidChars.IsMatch(sequence)) { return false; }

            return true;
        }

        private bool IsDuplicate(string sequence)
        {
            int length = sequence.Length;

            return (_sequencesByLength.ContainsKey(length) &&
                    _sequencesByLength[length].Contains(sequence));
        }

        private bool AlreadyMatchedByShorterSequence(string sequence)
        {
            bool alreadyMatchedByShorterSequence = false;
            
            // avoid redundant sequences
            int length = sequence.Length;
            for (int i = 1; i < length; i++)
            {
                if (!_sequencesByLength.ContainsKey(i)) { continue; }

                // specifying "abcd" after "abc" has already been specified 
                // is redundant due to the nature of the matching algorithm
                Set<string> matchesByLength = _sequencesByLength[i];
                foreach (string shorterSequence in matchesByLength)
                {
                    if (sequence.StartsWith(shorterSequence))
                    {
                        alreadyMatchedByShorterSequence = true;
                        break;
                    }
                }

                if (alreadyMatchedByShorterSequence) { break; }
            }

            return alreadyMatchedByShorterSequence;
        }

        private void AddSequence(string sequence)
        {
            int length = sequence.Length;

            // lazy dictionary set initialization
            if (!_sequencesByLength.ContainsKey(length))
            {
                _sequencesByLength[length] = new Set<string>();
            }

            _sequencesByLength[length].Add(sequence);

            // track max sequence length for utility purposes
            if (length > _maxSequenceLenght)
            {
                _maxSequenceLenght = length;
            }
        }

        /// <summary>
        /// Returns the full set of individual banned Unicode code points 
        /// (which can also be specified this way)
        /// </summary>
        /// <returns></returns>
        public Set<Int32> GetBannedCharacters()
        {
            // 1-character length sequences are interpreted as charset customization
            Set<Int32> bannedCodePoints = new Set<Int32>();
            if (null == _sequencesByLength || !_sequencesByLength.ContainsKey(1)) 
            {
                return bannedCodePoints;
            }

            Set<string> matchesByLength = _sequencesByLength[1];

            foreach (string value in matchesByLength)
            {
                int nextChar = Char.ConvertToUtf32(value, 0);
                bannedCodePoints.Add(nextChar);
            }

            return bannedCodePoints;
        }

        /// <summary>
        /// Returns the set of possible Unicode code points that must not be 
        /// used to randomly choose the next character, given the current 
        /// already generated character sequence
        /// </summary>
        /// <param name="length"></param>
        /// <param name="startingWith"></param>
        /// <returns></returns>
        public Set<int> GetBannedCharacters(int length, string startingWith)
        {
            // already covered by another overload above
            if (length <= 1)
            {
                throw new CaptchaCode.CaptchaCodeGenerationException("This overload should only be called for lengths of 2 or more.", length, startingWith);
            }

            Set<int> bannedCodePoints = new Set<int>();
            if (null == _sequencesByLength || !_sequencesByLength.ContainsKey(length))
            {
                return bannedCodePoints;
            }
            
            Set<string> matchesByLength = _sequencesByLength[length];
            foreach (string value in matchesByLength)
            {
                if (value.StartsWith(startingWith, false, CultureInfo.InvariantCulture))
                {
                    int nextChar = Char.ConvertToUtf32(value, length-1);
                    bannedCodePoints.Add(nextChar);
                }
            }

            return bannedCodePoints;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("BotDetect.BannedCharacterSequences { ");

            for (int i = 1; i < _maxSequenceLenght; i++)
            {
                if (!_sequencesByLength.ContainsKey(i)) { continue; }
                Set<string> matchesByLength = _sequencesByLength[i];
                foreach (string sequence in matchesByLength)
                {
                    str.Append(sequence);
                    str.Append(", ");
                }
                str.Remove(str.Length - 2, 2);
                str.AppendLine("; ");
            }

            str.Append("}");

            return str.ToString();
        }
    }

    internal class BannedSequenceComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x.Length == y.Length)
            {
                return x.ToUpperInvariant().CompareTo(y.ToUpperInvariant());
            }

            return x.Length.CompareTo(y.Length);
        }
    }
}
