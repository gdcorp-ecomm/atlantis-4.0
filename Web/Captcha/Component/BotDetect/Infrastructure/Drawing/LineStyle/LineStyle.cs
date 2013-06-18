using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
	[Serializable]
    internal class LineStyle : IEnumerable<LineLayer>
    {
        private List<LineLayer> _lineLayers;

        public LineLayer this[int index]
        {
            get
            {
                LineLayer element = null;

                if (index < _lineLayers.Count)
                {
                    element = _lineLayers[index];
                }

                return element;
            }

            set
            {
                if (index < _lineLayers.Count)
                {
                    _lineLayers[index] = value;
                }
                else
                {
                    _lineLayers.Add(value);
                }
            }
        }

        public int LayerCount
        {
            get
            {
                if (null == _lineLayers)
                {
                    return 0;
                }

                return _lineLayers.Count;
            }
        }

        public int Thickness
        {
            get
            {
                int thickness = 0;
                foreach (LineLayer layer in _lineLayers)
                {
                    thickness += layer.Thickness;
                }

                return thickness;
            }
        }

        public LineStyle()
        {
            _lineLayers = new List<LineLayer>();
        }

        public LineStyle(LineLayer layer)
        {
            _lineLayers = new List<LineLayer>();
            this[0] = layer;
        }

        public static bool HasValue(LineStyle style)
        {
            if (null == style)
            {
                return false;
            }

            if (null == style._lineLayers)
            {
                return false;
            }

            if (0 == style._lineLayers.Count)
            {
                return false;
            }

            if (0 == style._lineLayers[0].Thickness)
            {
                return false;
            }

            if (null == style._lineLayers[0][0])
            {
                return false;
            }

            if (null == style._lineLayers[0][0].Color)
            {
                return false;
            }

            return true;
        }

        public static LineStyle Empty
        {
            get
            {
                return new LineStyle();
            }
        }

        public static LineStyle Single(LineLayer layer)
        {
            return new LineStyle(layer);
        }

        public static LineStyle Double(LineLayer layer1, LineLayer layer2)
        {
            LineStyle doubleLine = new LineStyle();
            doubleLine[0] = layer1;
            doubleLine[1] = layer2;

            return doubleLine;
        }

        public static LineStyle Triple(LineLayer layer1, LineLayer layer2, LineLayer layer3)
        {
            LineStyle tripleLine = new LineStyle();
            tripleLine[0] = layer1;
            tripleLine[1] = layer2;
            tripleLine[2] = layer3;

            return tripleLine;
        }

        #region IEnumerable<LineLayer> Members

        public IEnumerator<LineLayer> GetEnumerator()
        {
            return _lineLayers.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _lineLayers.GetEnumerator();
        }

        #endregion
    }
}
