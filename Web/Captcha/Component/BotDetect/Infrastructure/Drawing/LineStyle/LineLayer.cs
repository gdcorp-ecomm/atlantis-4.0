using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
	[Serializable]
    internal class LineLayer : IEnumerable<LineElement>
    {
        private int _thickness;
        public int Thickness
        {
            get
            {
                return _thickness;
            }

            set
            {
                if (0 >= value)
                {
                    throw new DrawingException("Line thickness has to be a positive integer >= 1", value);
                }

                _thickness = value;
            }
        }

        private List<LineElement> _lineElements;

        public LineElement this[int index]
        {
            get
            {
                LineElement element = null;

                if (index < _lineElements.Count)
                {
                    element = _lineElements[index];
                }

                return element;
            }

            set
            {
                if (index < _lineElements.Count)
                {
                    _lineElements[index] = value;
                }
                else
                {
                    _lineElements.Add(value);
                }
            }
        }

        public LineLayer()
        {
            _lineElements = new List<LineElement>();
        }

        public LineLayer(int thickness)
        {
            this.Thickness = thickness;
            _lineElements = new List<LineElement>();
        }

        public LineLayer(int thickness, LineElement element)
        {
            this.Thickness = thickness;
            _lineElements = new List<LineElement>();
            this[0] = element;
        }

        public static LineLayer Solid(int thickness, Color color)
        {
            return new LineLayer(thickness, new LineElement(color));
        }

        public static LineLayer Dashed(int thickness, Color color1, Color color2, int length)
        {
            LineElement element1 = new LineElement(color1, length);
            LineElement element2 = new LineElement(color2, length);

            LineLayer dashed = new LineLayer(thickness);
            dashed[0] = element1;
            dashed[1] = element2;

            return dashed;
        }

        #region IEnumerable<LineElement> Members

        public IEnumerator<LineElement> GetEnumerator()
        {
            return _lineElements.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _lineElements.GetEnumerator();
        }

        #endregion
    }
}
