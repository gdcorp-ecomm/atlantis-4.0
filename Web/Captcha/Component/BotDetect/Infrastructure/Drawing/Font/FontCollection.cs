using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
    [Serializable]
    internal class FontCollection : IEnumerable
    {
        protected List<Font> fonts;

        public FontCollection()
        {
            fonts = new List<Font>();
        }

        public FontCollection Clone()
        {
            FontCollection clone = new FontCollection();
            clone.fonts.AddRange(this.fonts);
            return clone;
        }

        public Font this[int index]
        {
            get
            {
                if (index < fonts.Count)
                {
                    return fonts[index];
                }

                return null;
            }

            set
            {
                if (index < fonts.Count)
                {
                    fonts[index] = value;
                }

                fonts.Add(value);
            }
        }

        private RandomizationStyle _randomizationStyle = RandomizationStyle.AlwaysRandomize;

        public RandomizationStyle RandomizationStyle
        {
            get
            {
                return _randomizationStyle;
            }
            set
            {
                _randomizationStyle = value;
            }
        }

        public Font Next
        {
            get
            {
                if (null == fonts || 0 == fonts.Count)
                {
                    return null;
                }

                Font chosen = fonts[RandomGenerator.Next(fonts.Count)];

                if (RandomizationStyle.RandomizeOnce == _randomizationStyle)
                {
                    fonts.Clear();
                    fonts.Add(chosen);
                }

                return chosen;
            }
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return fonts.GetEnumerator();
        }

        #endregion
    }
}
