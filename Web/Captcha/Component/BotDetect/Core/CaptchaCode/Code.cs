using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

using BotDetect.CaptchaCode;
using BotDetect.Serialization;

namespace BotDetect
{
    /// <summary>
    /// Represents a single randomly generated Captcha code.
    /// </summary>
    [Serializable]
    public class Code : ISerializable
    {
        private string _code;
        /// <summary>
        /// The random Captcha code contained in the object
        /// </summary>
        public string CaptchaCode
        {
            get
            {
                return _code;
            }
        }

        private DateTime _generationTime = DateTime.MaxValue;
        /// <summary>
        /// When was the random Captcha code generated
        /// </summary>
        public DateTime GenerationTime
        {
            get
            {
                return _generationTime;
            }
        }

        /// <summary>
        /// How many seconds have elapsed since the random Captcha code was generated 
        /// </summary>
        public int ElapsedSeconds
        {
            get
            {
                int seconds = 0;
                if (DateTime.UtcNow > _generationTime)
                {
                    seconds = (int)(DateTime.UtcNow - _generationTime).TotalSeconds;
                }
                return seconds;
            }
        }

        private bool _usedForImageGeneration;
        /// <summary>
        /// Has the Captcha code been used to generate a Captcha image
        /// </summary>
        public bool IsUsedForImageGeneration
        {
            get
            {
                return _usedForImageGeneration;
            }
            set
            {
                _usedForImageGeneration = value;
            }
        }

        private bool _usedForSoundGeneration;
        /// <summary>
        /// Has the Captcha code been used to generate a Captcha sound
        /// </summary>
        public bool IsUsedForSoundGeneration
        {
            get
            {
                return _usedForSoundGeneration;
            }
            set
            {
                _usedForSoundGeneration = value;
            }
        }

        // keep track of client-side successful validations
        private List<DateTime> _clientSideValidations;

        internal void RecordClientSideValidation()
        {
            _clientSideValidations.Add(DateTime.UtcNow);
        }

        /// <summary>
        /// The constructor takes the freshly generated random Captcha
        /// code that will be contained in the object
        /// </summary>
        /// <param name="code"></param>
        public Code(string code)
        {
            _code = code;
            _generationTime = DateTime.UtcNow;
            _clientSideValidations = new List<DateTime>();
        }

        /// <summary>
        /// CaptchaCode instances are compared by code value only
        /// </summary>
        public override bool Equals(object obj)
        {
            bool equals = false;

            Code code = obj as Code;
            if (null != code)
            {
                if ( StringHelper.HasValue(_code) &&
                     StringHelper.HasValue(code.CaptchaCode)
                   )
                {
                    string currentCode = _code.Normalize(NormalizationForm.FormC);
                    string compareCode = code.CaptchaCode.Normalize(NormalizationForm.FormC);
                    equals = currentCode.Equals(compareCode, StringComparison.OrdinalIgnoreCase);
                }
            }

            return equals;
        }

        /// <summary>
        /// CaptchaCode hash code depends only on the code value, case and culture invariant
        /// </summary>
        public override int GetHashCode()
        {
            if ( StringHelper.HasValue(_code))
            {
                return _code.ToUpperInvariant().GetHashCode();
            }
            else
            {
                return 0;
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("BotDetect.Code { ");

            str.Append("  value: ");
            str.AppendLine(_code);

            str.Append("  generated at: ");
            str.AppendLine(StringHelper.ToString(_generationTime));

            str.Append("  valid until: ");
            str.AppendLine(StringHelper.ToString(_generationTime.AddSeconds(CodeCollection.CodeTimeout)));

            str.Append("  image requested: ");
            str.AppendLine(StringHelper.ToString(_usedForImageGeneration));

            str.Append("  sound requested: ");
            str.AppendLine(StringHelper.ToString(_usedForSoundGeneration));

            if (null != _clientSideValidations)
            {
                str.Append("  client-side validation passed: {");
                bool first = true;
                foreach (DateTime t in _clientSideValidations)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        str.Append(",");
                    }
                    str.Append(" ");
                    str.Append(StringHelper.ToString(_generationTime));
                }
                str.AppendLine(" }");
            }

            str.Append("}");

            return str.ToString();
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            SerializationWriter writer = new SerializationWriter();
            writer.WriteOptimized(_code);
            writer.Write(_generationTime);
            writer.Write(_usedForImageGeneration);
            writer.Write(_usedForSoundGeneration);
            writer.Write<DateTime>(_clientSideValidations);

            info.AddValue("data", writer.ToArray());
        }

        protected Code(SerializationInfo info, StreamingContext context)
        {
            SerializationReader reader = new SerializationReader((byte[])info.GetValue("data", typeof(byte[])));
            _code = reader.ReadOptimizedString();
            _generationTime = reader.ReadDateTime();
            _usedForImageGeneration = reader.ReadBoolean();
            _usedForSoundGeneration = reader.ReadBoolean();
            _clientSideValidations = reader.ReadList<DateTime>();
        }

        #endregion
    }
}
