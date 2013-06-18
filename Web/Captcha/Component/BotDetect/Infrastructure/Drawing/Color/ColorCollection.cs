using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
	[Serializable]
    internal class ColorCollection : IEnumerable
    {
        private Dictionary<string, Color> _colors;

        public ColorCollection()
        {
            _colors = new Dictionary<string, Color>();
        }

        public ColorCollection(Dictionary<string, System.Drawing.Color> gdiColors)
        {
            _colors = new Dictionary<string, Color>(gdiColors.Count);

            foreach (KeyValuePair<string, System.Drawing.Color> kvp in gdiColors)
            {
                _colors.Add(kvp.Key, new Color(kvp.Value));
            }
        }

        public Color this[string name]
        {
            get
            {
                Color color = null;
                if (_colors.ContainsKey(name))
                {
                    color = _colors[name] as Color;
                }
                return color;
            }

            set
            {
                _colors[name] = value;
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("BotDetect.Drawing.ColorCollection {");

            if (null == _colors || 0 == _colors.Count)
            {
                str.Append(" empty }");
            }
            else
            {
                str.AppendLine();
                foreach (string key in _colors.Keys)
                {
                    str.Append(key);
                    str.Append(": ");
                    Color savedValue = _colors[key] as Color;
                    str.AppendLine(StringHelper.ToString(savedValue));
                }
                str.Append("}");
            }

            return str.ToString();
        }

        public IEnumerator GetEnumerator()
        {
            return _colors.GetEnumerator();
        }

        public Dictionary<string, System.Drawing.Color> GdiColors
        {
            get
            {
                Dictionary<string, System.Drawing.Color> gdiColors = new Dictionary<string, System.Drawing.Color>(_colors.Count);

                foreach (KeyValuePair<string, Color> kvp in _colors)
                {
                    gdiColors.Add(kvp.Key, kvp.Value.GdiColor);
                }

                return gdiColors;
            }
        }

        /*IEnumerator<Color> IEnumerable<Color>.GetEnumerator()
        {
            foreach (Color c in _colors.Values)
            {
                yield return c;
            }
        }*/
    }
}
